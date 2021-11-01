namespace Run2
{
  public static class Parser
  {
    private static Token currentToken;

    public static void Parse(Globals.Tokens tokens)
    {
      currentToken = tokens.Dequeue();
      while (currentToken.TokenKind is Globals.TokenKind.PragmaCommand or Globals.TokenKind.PragmaReadScript)
      {
        switch (currentToken.TokenKind)
        {
          case Globals.TokenKind.PragmaCommand:
            currentToken = tokens.Dequeue();
            currentToken.CheckToken(Globals.TokenKind.CommandName);
            var commandName = currentToken.Value.ToString();
            (!string.IsNullOrEmpty(commandName)).Check("Command names must not be null or empty");
            Globals.Commands.TryGetValue(commandName, out var command).Check($"Command '{commandName}' not found while parsing");
            var userCommand = command as UserCommand;
            (userCommand != null).Check($"Expected command '{commandName}' to be a user-command");
            currentToken = tokens.Dequeue();
            if (currentToken.TokenKind == Globals.TokenKind.Quote)
            {
              userCommand.Description = currentToken.Value.ToString();
              currentToken = tokens.Dequeue();
            }
            if (currentToken.TokenKind == Globals.TokenKind.Quote)
            {
              userCommand.Returns = currentToken.Value.ToString();
              currentToken = tokens.Dequeue();
            }
            if (currentToken.TokenKind == Globals.TokenKind.Quote)
            {
              userCommand.Remarks = currentToken.Value.ToString();
              currentToken = tokens.Dequeue();
            }
            ParseParameters(tokens, userCommand);
            ParseStatements(tokens, userCommand.SubCommands);
            break;
          case Globals.TokenKind.PragmaReadScript:
            currentToken = tokens.Dequeue();
            currentToken = tokens.Dequeue();
            break;
        }
      }
      (currentToken.TokenKind == Globals.TokenKind.EOF).Check("Unexpected eof of file while parsing");
    }

    private static void CheckToken(this Token token, Globals.TokenKind tokenKind)
    {
      (token.TokenKind == tokenKind).Check($"Expected {tokenKind}");
    }

    private static void ParseParameters(Globals.Tokens tokens, UserCommand userCommand)
    {
      while (currentToken.TokenKind is Globals.TokenKind.Element or Globals.TokenKind.Text or Globals.TokenKind.LeftParenthesis)
      {
        var parameterName = "";
        switch (currentToken.TokenKind)
        {
          case Globals.TokenKind.Element:
          case Globals.TokenKind.Text:
            parameterName = currentToken.ToString();
            userCommand.ParameterNames.Add(parameterName);
            currentToken = tokens.Dequeue();
            break;
          case Globals.TokenKind.LeftParenthesis:
            currentToken = tokens.Dequeue();
            (currentToken.TokenKind is Globals.TokenKind.Element or Globals.TokenKind.Text).Check("Expected name of a parameter");
            parameterName = currentToken.ToString();
            var items = new Items();
            items.Enqueue(parameterName);
            userCommand.ParameterNames.Add(items);
            currentToken = tokens.Dequeue();
            if (currentToken.TokenKind is Globals.TokenKind.Element or Globals.TokenKind.Text or Globals.TokenKind.Quote)
            {
              items.Enqueue(currentToken.Value, new Properties { IsQuote = currentToken.TokenKind == Globals.TokenKind.Quote, ScriptPath = currentToken.ScriptPath, LineNumber = currentToken.LineNumber });
              currentToken = tokens.Dequeue();
            }
            (currentToken.TokenKind == Globals.TokenKind.RightParenthesis).Check("Expected right parenthesis");
            currentToken = tokens.Dequeue();
            break;
        }
        if (currentToken.TokenKind == Globals.TokenKind.Quote)
        {
          userCommand.ParameterDescriptions.Add(parameterName, currentToken.ToString());
          currentToken = tokens.Dequeue();
        }
      }
    }

    private static void ParseStatement(Globals.Tokens tokens, Globals.SubCommand subCommand)
    {
      while (true)
      {
        switch (currentToken.TokenKind)
        {
          case Globals.TokenKind.LeftParenthesis:
            currentToken = tokens.Dequeue();
            var subCommands = new Globals.SubCommands();
            subCommand.Arguments.Enqueue(subCommands);
            ParseStatements(tokens, subCommands);
            (currentToken.TokenKind == Globals.TokenKind.RightParenthesis).Check("Expected right parenthesis");
            currentToken = tokens.Dequeue();
            break;
          case Globals.TokenKind.Element or Globals.TokenKind.Text or Globals.TokenKind.Quote:
            subCommand.Arguments.Enqueue(currentToken.Value, new Properties { IsQuote = currentToken.TokenKind == Globals.TokenKind.Quote, ScriptPath = currentToken.ScriptPath, LineNumber = currentToken.LineNumber });
            currentToken = tokens.Dequeue();
            break;
          default:
            return;
        }
      }
    }

    private static void ParseStatements(Globals.Tokens tokens, Globals.SubCommands subCommands)
    {
      while (currentToken.TokenKind == Globals.TokenKind.CommandName)
      {
        var subCommand = new Globals.SubCommand();
        subCommands.Add(subCommand);
        subCommand.CommandName = currentToken.ToString();
        subCommand.CommandName.AddProperties(new Properties { ScriptPath = currentToken.ScriptPath, LineNumber = currentToken.LineNumber });
        currentToken = tokens.Dequeue();
        ParseStatement(tokens, subCommand);
      }
    }
  }
}