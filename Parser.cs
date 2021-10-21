namespace Run2
{
  public static class Parser
  {
    private static Token currentToken;

    public static void Parse(Tokens tokens)
    {
      currentToken = tokens.Dequeue();
      while (currentToken.TokenKind == TokenKind.PragmaCommand)
      {
        currentToken = tokens.Dequeue();
        currentToken.CheckToken(TokenKind.CommandName);
        var commandName = currentToken.Value.ToString();
        (!string.IsNullOrEmpty(commandName)).Check("Command names must not be null or empty");
        Globals.Commands.TryGetValue(commandName, out var command).Check($"Command {commandName} not found while parsing");
        var userCommand = command as UserCommand;
        (userCommand != null).Check($"Expected command {commandName} to be a user-command");
        currentToken = tokens.Dequeue();
        if (currentToken.TokenKind == TokenKind.Quote)
        {
          userCommand.Description = currentToken.Value.ToString();
          currentToken = tokens.Dequeue();
        }
        if (currentToken.TokenKind == TokenKind.Quote)
        {
          userCommand.Returns = currentToken.Value.ToString();
          currentToken = tokens.Dequeue();
        }
        if (currentToken.TokenKind == TokenKind.Quote)
        {
          userCommand.Remarks = currentToken.Value.ToString();
          currentToken = tokens.Dequeue();
        }
        ParseParameters(tokens, userCommand);
        ParseStatements(tokens, userCommand.SubCommands);
      }
      (currentToken.TokenKind == TokenKind.EOF).Check("Unexpected eof of file while parsing");
    }

    private static void CheckToken(this Token token, TokenKind tokenKind)
    {
      (token.TokenKind == tokenKind).Check($"Expected {tokenKind}");
    }

    private static void ParseParameters(Tokens tokens, UserCommand userCommand)
    {
      while (currentToken.TokenKind is TokenKind.Element or TokenKind.Text or TokenKind.LeftParenthesis)
      {
        var parameterName = "";
        switch (currentToken.TokenKind)
        {
          case TokenKind.Element:
          case TokenKind.Text:
            parameterName = currentToken.ToString();
            userCommand.ParameterNames.Add(parameterName);
            currentToken = tokens.Dequeue();
            break;
          case TokenKind.LeftParenthesis:
            currentToken = tokens.Dequeue();
            (currentToken.TokenKind is TokenKind.Element or TokenKind.Text).Check("Expected name of a parameter");
            parameterName = currentToken.ToString();
            var items = new Items();
            items.Enqueue(parameterName);
            userCommand.ParameterNames.Add(items);
            currentToken = tokens.Dequeue();
            if (currentToken.TokenKind is TokenKind.Element or TokenKind.Text or TokenKind.Quote)
            {
              items.Enqueue(currentToken.Value, new Properties { IsQuote = currentToken.TokenKind == TokenKind.Quote, LineNumber = currentToken.LineNumber });
              currentToken = tokens.Dequeue();
            }
            (currentToken.TokenKind == TokenKind.RightParenthesis).Check("Expected right parenthesis");
            currentToken = tokens.Dequeue();
            break;
        }
        if (currentToken.TokenKind == TokenKind.Quote)
        {
          userCommand.ParameterDescriptions.Add(parameterName, currentToken.ToString());
          currentToken = tokens.Dequeue();
        }
      }
    }

    private static void ParseStatement(Tokens tokens, SubCommand subCommand)
    {
      while (true)
      {
        switch (currentToken.TokenKind)
        {
          case TokenKind.LeftParenthesis:
            currentToken = tokens.Dequeue();
            var subCommands = new SubCommands();
            subCommand.Arguments.Enqueue(subCommands);
            ParseStatements(tokens, subCommands);
            (currentToken.TokenKind == TokenKind.RightParenthesis).Check("Expected right parenthesis");
            currentToken = tokens.Dequeue();
            break;
          case TokenKind.Element or TokenKind.Text or TokenKind.Quote:
            subCommand.Arguments.Enqueue(currentToken.Value, new Properties { IsQuote = currentToken.TokenKind == TokenKind.Quote, LineNumber = currentToken.LineNumber });
            currentToken = tokens.Dequeue();
            break;
          default:
            return;
        }
      }
    }

    private static void ParseStatements(Tokens tokens, SubCommands subCommands)
    {
      while (currentToken.TokenKind == TokenKind.CommandName)
      {
        var subCommand = new SubCommand();
        subCommands.Add(subCommand);
        subCommand.CommandName = currentToken.ToString();
        subCommand.CommandName.AddProperties(new Properties { LineNumber = currentToken.LineNumber });
        currentToken = tokens.Dequeue();
        ParseStatement(tokens, subCommand);
      }
    }
  }
}