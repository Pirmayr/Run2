using System.IO;

namespace Run2
{
  public static class Scanner
  {
    private const char BlockBeginCharacter = '(';
    private const char BlockEndCharacter = ')';
    private const char EscapeCharacter = '~';
    private const char EOF = (char) 0;
    private const char QuoteDelimiter = '\'';
    private const char TextDelimiter = '"';

    public static Tokens Scan(string scriptPath)
    {
      var code = File.ReadAllText(scriptPath);
      var characters = new CharacterQueue(code);
      var result = new Tokens();
      var currentCharacter = GetNextCharacter(characters);
      var blockLevel = 0;
      while (currentCharacter != EOF)
      {
        if (!char.IsWhiteSpace(currentCharacter))
        {
          switch (currentCharacter)
          {
            case BlockBeginCharacter:
              result.Enqueue(new Token(TokenKind.LeftParenthesis, '('));
              ++blockLevel;
              break;
            case BlockEndCharacter:
              result.Enqueue(new Token(TokenKind.RightParenthesis, ')'));
              --blockLevel;
              break;
            case TextDelimiter:
              var text = TextDelimiter.ToString();
              currentCharacter = GetNextCharacter(characters);
              while (currentCharacter != TextDelimiter && currentCharacter != EOF)
              {
                text += Unescape(characters, currentCharacter);
                currentCharacter = GetNextCharacter(characters);
              }
              (currentCharacter != EOF).Check("Unexpected end of file while reading a text");
              text += TextDelimiter.ToString();
              result.Enqueue(new Token(TokenKind.Text, text));
              break;
            case QuoteDelimiter:
              var quote = "";
              currentCharacter = GetNextCharacter(characters);
              while (currentCharacter != QuoteDelimiter && currentCharacter != EOF)
              {
                quote += Unescape(characters, currentCharacter);
                currentCharacter = GetNextCharacter(characters);
              }
              (currentCharacter != EOF).Check("Unexpected end of file while reading a quote");
              result.Enqueue(new Token(TokenKind.Quote, quote));
              break;
            default:
              var element = "";
              while (IsElementCharacter(currentCharacter))
              {
                element += currentCharacter;
                currentCharacter = GetNextCharacter(characters);
              }
              result.Enqueue(new Token(TokenKind.Element, element.GetBestTypedObject()));
              break;
          }
        }
        currentCharacter = GetNextCharacter(characters);
      }
      (blockLevel == 0).Check(0 < blockLevel ? "There are more left than right block-delimiter" : "There are more right than left block-delimiter");
      IdentifyCommands(result, scriptPath);
      result.Enqueue(new Token(TokenKind.EOF, "EOF"));
      return result;
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

    private static char GetNextCharacter(this CharacterQueue characters)
    {
      return 0 < characters.Count ? characters.Dequeue() : EOF;
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
          Globals.Commands.Add(commandName, new UserCommand { Name = commandName, ScriptPath = scriptPath, IsQuoted = token.TokenKind == TokenKind.Text });
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
  }
}