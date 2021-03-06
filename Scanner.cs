using System;
using System.Collections.Generic;
using System.IO;

namespace Run2
{
  public static class Scanner
  {
    public const char TextDelimiter = '"';
    private const char BlockBeginCharacter = '(';
    private const char BlockEndCharacter = ')';
    private const char CommentCharacter = ';';
    private const char EOF = (char) 0;
    private const char EscapeCharacter = '~';
    private const char QuoteDelimiter = '\'';

    public static Globals.Tokens Scan(string scriptPath)
    {
      var previousScriptPath = Globals.CurrentScriptPath;
      var previousLineNumber = Globals.CurrentLineNumber;
      try
      {
        return DoScan(scriptPath);
      }
      catch (Exception exception)
      {
        if (exception is Globals.RuntimeException)
        {
          throw;
        }
        Globals.HandleException(exception, Globals.CurrentScriptPath, Globals.CurrentLineNumber);
        throw new Globals.RuntimeException("Runtime error");
      }
      finally
      {
        Globals.CurrentLineNumber = previousLineNumber;
        Globals.CurrentScriptPath = previousScriptPath;
      }
    }

    private static Globals.Tokens DoScan(string scriptPath)
    {
      var code = File.ReadAllText(scriptPath);
      var characters = new Globals.CharacterQueue(code);
      var lineNumber = 1;
      Globals.CurrentScriptPath = scriptPath;
      Globals.CurrentLineNumber = lineNumber;
      var result = new Globals.Tokens();
      var currentCharacter = GetNextCharacter(characters, ref lineNumber);
      var blockLevel = 0;
      while (currentCharacter != EOF)
      {
        if (!char.IsWhiteSpace(currentCharacter))
        {
          switch (currentCharacter)
          {
            case CommentCharacter:
              currentCharacter = characters.GetNextCharacter(ref lineNumber);
              while (currentCharacter != '\n' && currentCharacter != EOF)
              {
                currentCharacter = characters.GetNextCharacter(ref lineNumber);
              }
              if (currentCharacter == '\n')
              {
                currentCharacter = characters.GetNextCharacter(ref lineNumber);
              }
              break;
            case BlockBeginCharacter:
              result.Enqueue(new Token(Globals.TokenKind.LeftParenthesis, '(', scriptPath, lineNumber));
              ++blockLevel;
              currentCharacter = characters.GetNextCharacter(ref lineNumber);
              break;
            case BlockEndCharacter:
              result.Enqueue(new Token(Globals.TokenKind.RightParenthesis, ')', scriptPath, lineNumber));
              --blockLevel;
              currentCharacter = characters.GetNextCharacter(ref lineNumber);
              break;
            case TextDelimiter:
              var text = TextDelimiter.ToString();
              currentCharacter = GetNextCharacter(characters, ref lineNumber);
              while (currentCharacter != TextDelimiter && currentCharacter != EOF)
              {
                text += Unescape(characters, currentCharacter);
                currentCharacter = GetNextCharacter(characters, ref lineNumber);
              }
              (currentCharacter != EOF).Check("Unexpected end of file while reading a text");
              text += TextDelimiter.ToString();
              result.Enqueue(new Token(Globals.TokenKind.Text, text, scriptPath, lineNumber));
              currentCharacter = characters.GetNextCharacter(ref lineNumber);
              break;
            case QuoteDelimiter:
              var quote = "";
              currentCharacter = GetNextCharacter(characters, ref lineNumber);
              while (currentCharacter != QuoteDelimiter && currentCharacter != EOF)
              {
                quote += Unescape(characters, currentCharacter);
                currentCharacter = GetNextCharacter(characters, ref lineNumber);
              }
              (currentCharacter != EOF).Check("Unexpected end of file while reading a quote");
              result.Enqueue(new Token(Globals.TokenKind.Quote, quote, scriptPath, lineNumber));
              currentCharacter = characters.GetNextCharacter(ref lineNumber);
              break;
            default:
              var element = "";
              while (IsElementCharacter(currentCharacter))
              {
                element += currentCharacter;
                currentCharacter = GetNextCharacter(characters, ref lineNumber);
              }
              result.Enqueue(new Token(Globals.TokenKind.Element, element.ToBestType(), scriptPath, lineNumber));
              break;
          }
        }
        else
        {
          currentCharacter = GetNextCharacter(characters, ref lineNumber);
        }
      }
      (blockLevel == 0).Check(0 < blockLevel ? "There are more left than right block-delimiter" : "There are more right than left block-delimiter");
      ImportScripts(result, Path.GetFileNameWithoutExtension(scriptPath));
      IdentifyCommands(result);
      result.Enqueue(new Token(Globals.TokenKind.EOF, "EOF", scriptPath, lineNumber));
      return result;
    }

    private static char GetNextCharacter(this Globals.CharacterQueue characters, ref int lineNumber)
    {
      var result = 0 < characters.Count ? characters.Dequeue() : EOF;
      if (result == '\n')
      {
        ++lineNumber;
        Globals.CurrentLineNumber = lineNumber;
      }
      return result;
    }

    private static void IdentifyCommands(Globals.Tokens tokens)
    {
      Token mostRecentToken = null;
      var pragmaCommandRead = false;
      foreach (var token in tokens)
      {
        mostRecentToken = token;
        if (pragmaCommandRead)
        {
          pragmaCommandRead = false;
          token.TokenKind = Globals.TokenKind.CommandName;
          var commandName = token.Value.ToString();
          (!string.IsNullOrEmpty(commandName)).Check("Command-name must not be null");
          var newCommand = new UserCommand { Name = commandName, ScriptPath = token.ScriptPath, QuoteArguments = token.TokenKind == Globals.TokenKind.Text };
          newCommand.AddProperties(new Properties { ScriptPath = token.ScriptPath, LineNumber = token.LineNumber });
          Globals.Commands.Add(commandName, newCommand);
        }
        else if (token.Value.ToString() == Globals.PragmaCommand)
        {
          pragmaCommandRead = true;
          token.TokenKind = Globals.TokenKind.PragmaCommand;
        }
      }
      (mostRecentToken != null && !pragmaCommandRead).Check("Unexpected end of file while identifying commands");
      foreach (var token in tokens)
      {
        var valueString = token.Value.ToString();
        (valueString != null).Check("The value must not be null");
        if (token.TokenKind == Globals.TokenKind.Element && Globals.Commands.ContainsKey(valueString))
        {
          token.TokenKind = Globals.TokenKind.CommandName;
        }
      }
    }

    private static void ImportScripts(Globals.Tokens tokens, string scriptName)
    {
      Token mostRecentToken = null;
      var pragmaImportRead = false;
      foreach (var token in tokens)
      {
        mostRecentToken = token;
        if (pragmaImportRead)
        {
          pragmaImportRead = false;
          var importName = token.Value.ToString();
          (!string.IsNullOrEmpty(importName)).Check("Script-name must not be null");
          Run2.LoadScript(importName);
          if (!Globals.Imports.TryGetValue(scriptName, out var importsList))
          {
            importsList = new List<string>();
            Globals.Imports.Add(scriptName, importsList);
          }
          importsList.Add(importName);
        }
        else if (token.Value.ToString() == Globals.PragmaImport)
        {
          pragmaImportRead = true;
          token.TokenKind = Globals.TokenKind.PragmaReadScript;
        }
      }
      (mostRecentToken != null && !pragmaImportRead).Check("Unexpected end of file while identifying commands");
    }

    private static bool IsElementCharacter(char character)
    {
      switch (character)
      {
        case BlockBeginCharacter:
        case BlockEndCharacter:
        case QuoteDelimiter:
        case TextDelimiter:
        case EOF:
          return false;
      }
      return !char.IsWhiteSpace(character);
    }

    private static char Unescape(Globals.CharacterQueue characters, char character)
    {
      if (character == EscapeCharacter)
      {
        (0 < characters.Count).Check("Unexpected end of file while escaping character");
        var result = characters.Dequeue();
        switch (result)
        {
          case 'n':
            return '\n';
          case 'r':
            return '\r';
        }
        return result;
      }
      return character;
    }
  }
}