using System.Collections.Generic;
using System.IO;

namespace Run2
{
  public static class Scanner
  {
    public const char TextDelimiter = '"';
    private const char BlockBeginCharacter = '(';
    private const char BlockEndCharacter = ')';
    private const char EOF = (char) 0;
    private const char EscapeCharacter = '~';
    private const char QuoteDelimiter = '\'';

    public static Tokens Scan(string scriptPath)
    {
      var code = File.ReadAllText(scriptPath);
      var characters = new CharacterQueue(code);
      var lineNumber = 1;
      var result = new Tokens();
      var currentCharacter = GetNextCharacter(characters, ref lineNumber);
      var blockLevel = 0;
      while (currentCharacter != EOF)
      {
        if (!char.IsWhiteSpace(currentCharacter))
        {
          switch (currentCharacter)
          {
            case BlockBeginCharacter:
              result.Enqueue(new Token(TokenKind.LeftParenthesis, '(', lineNumber));
              ++blockLevel;
              currentCharacter = characters.GetNextCharacter(ref lineNumber);
              break;
            case BlockEndCharacter:
              result.Enqueue(new Token(TokenKind.RightParenthesis, ')', lineNumber));
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
              result.Enqueue(new Token(TokenKind.Text, text, lineNumber));
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
              result.Enqueue(new Token(TokenKind.Quote, quote, lineNumber));
              currentCharacter = characters.GetNextCharacter(ref lineNumber);
              break;
            default:
              var element = "";
              while (IsElementCharacter(currentCharacter))
              {
                element += currentCharacter;
                currentCharacter = GetNextCharacter(characters, ref lineNumber);
              }
              result.Enqueue(new Token(TokenKind.Element, element.ToBestType(), lineNumber));
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
      IdentifyCommands(result, scriptPath);
      result.Enqueue(new Token(TokenKind.EOF, "EOF", lineNumber));
      return result;
    }

    private static char GetNextCharacter(this CharacterQueue characters, ref int lineNumber)
    {
      var result = 0 < characters.Count ? characters.Dequeue() : EOF;
      if (result == '\n')
      {
        ++lineNumber;
      }
      return result;
    }

    private static void IdentifyCommands(Tokens tokens, string scriptPath)
    {
      var pragmaCommandRead = false;
      foreach (var token in tokens)
      {
        if (pragmaCommandRead)
        {
          pragmaCommandRead = false;
          token.TokenKind = TokenKind.CommandName;
          var commandName = token.Value.ToString();
          (!string.IsNullOrEmpty(commandName)).Check("Command-name must not be null");
          Globals.Commands.Add(commandName, new UserCommand { Name = commandName, ScriptPath = scriptPath, QuoteArguments = token.TokenKind == TokenKind.Text });
        }
        else if (token.Value.ToString() == Globals.PragmaCommand)
        {
          pragmaCommandRead = true;
          token.TokenKind = TokenKind.PragmaCommand;
        }
      }
      (!pragmaCommandRead).Check("Unexpected end of file while identifying commands");
      foreach (var token in tokens)
      {
        var valueString = token.Value.ToString();
        (valueString != null).Check("The value must not be null");
        if (token.TokenKind == TokenKind.Element && Globals.Commands.ContainsKey(valueString))
        {
          token.TokenKind = TokenKind.CommandName;
        }
      }
    }

    private static void ImportScripts(Tokens tokens, string scriptName)
    {
      var pragmaImportRead = false;
      foreach (var token in tokens)
      {
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
          token.TokenKind = TokenKind.PragmaReadScript;
        }
      }
      (!pragmaImportRead).Check("Unexpected end of file while identifying commands");
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

    private static char Unescape(CharacterQueue characters, char character)
    {
      if (character == EscapeCharacter)
      {
        (0 < characters.Count).Check("Unexpected end of file while escaping character");
        var result = characters.Dequeue();
        return result == 'n' ? '\n' : result;
      }
      return character;
    }
  }
}