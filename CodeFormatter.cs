using System;
using System.Linq;
using System.Text;

namespace Run2
{
  // ReSharper disable once MemberCanBeInternal
  public static class CodeFormatter
  {
    // ReSharper disable once MemberCanBeInternal
    public static string GetCode(string filter)
    {
      var result = new StringBuilder();
      foreach (var command in Globals.Commands.Values.OrderBy(static item => item.Name))
      {
        if (command is UserCommand userCommandValue)
        {
          if (!userCommandValue.ScriptPath.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
          {
            continue;
          }
          if (0 < result.Length)
          {
            result.Append("\n\n");
          }
          result.Append(userCommandValue.QuoteArguments ? $"command \"{userCommandValue.Name}\"" : $"command {userCommandValue.Name}");
          if (!string.IsNullOrEmpty(userCommandValue.Description))
          {
            result.Append($"\n  '{userCommandValue.Description}'");
          }
          if (!string.IsNullOrEmpty(userCommandValue.Returns))
          {
            result.Append($"\n  '{userCommandValue.Returns}'");
          }
          if (!string.IsNullOrEmpty(userCommandValue.Remarks))
          {
            result.Append($"\n  '{userCommandValue.Remarks}'");
          }
          foreach (var item in userCommandValue.ParameterNames)
          {
            var parameterName = item.GetNameFromItem();
            var parameterDeclaration = parameterName;
            if (item is Items itemsValue)
            {
              parameterDeclaration = $"({ToCode(itemsValue, 0, false)} )";
            }
            result.AppendNewLine(true);
            result.AppendIndented(parameterDeclaration, 2, true);
            if (userCommandValue.ParameterDescriptions.TryGetValue(parameterName, out var parameterDescription))
            {
              result.Append($" '{parameterDescription}'");
            }
          }
          result.Append($"\n{userCommandValue.SubCommands.ToCode(2, true)}");
        }
      }
      return result.ToString();
    }

    // ReSharper disable once MemberCanBeInternal
    public static string ToCode(this SubCommand subCommand, int indent, bool newLine)
    {
      var result = DoToCode(subCommand, indent, false);
      return !newLine && Globals.MaxCodeLineLength < result.Length ? DoToCode(subCommand, indent, true) : result;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static string ToCode(this Items items, int indent, bool newLine)
    {
      var result = DoToCode(items, indent, false);
      return !newLine && Globals.MaxCodeLineLength < result.Length ? DoToCode(items, indent, true) : result;
    }

    public static string ToCode(this object value)
    {
      var tokens = new Items();
      tokens.Enqueue(value);
      return ToCode(tokens, 0, false);
    }

    private static string DoToCode(Items items, int indent, bool newLine)
    {
      var multilineSubCommandWritten = false;
      var result = new StringBuilder();
      foreach (var item in items)
      {
        switch (item)
        {
          case SubCommands subCommandsValue:
          {
            var subCommandsCode = subCommandsValue.ToCode(indent, newLine);
            if (Globals.MaxCodeLineLength < subCommandsCode.Length || subCommandsCode.Contains('\n'))
            {
              var localNewLine = subCommandsCode.Contains('\n') || newLine;
              result.Append(" (");
              result.AppendNewLine(localNewLine);
              result.Append($"{subCommandsCode}");
              result.AppendNewLine(localNewLine);
              result.AppendIndented(")", indent, localNewLine);
              multilineSubCommandWritten = true;
            }
            else
            {
              result.AppendNewLine(multilineSubCommandWritten && newLine);
              result.AppendIndented("( ", indent, multilineSubCommandWritten && newLine);
              result.Append($"{subCommandsCode.Trim(' ')}");
              result.Append(" )");
            }
            break;
          }
          default:
            if (item.GetProperties().IsQuote)
            {
              result.AppendNewLine(multilineSubCommandWritten);
              result.AppendIndented($"'{item.ToString()?.Replace("\n", "~n")}'", indent, multilineSubCommandWritten);
              multilineSubCommandWritten = false;
            }
            else
            {
              result.AppendNewLine(multilineSubCommandWritten);
              string itemString;
              switch (item)
              {
                case double doubleValue:
                  itemString = doubleValue.ToString("0.0############################");
                  break;
                default:
                  itemString = item.ToString();
                  break;
              }
              result.AppendIndented($"{itemString}", indent, multilineSubCommandWritten);
              multilineSubCommandWritten = false;
            }
            break;
        }
      }
      return result.ToString();
    }

    private static string DoToCode(SubCommand subCommand, int indent, bool newLine)
    {
      var result = new StringBuilder();
      result.AppendIndented(subCommand.CommandName, indent, newLine);
      result.Append(subCommand.Arguments.ToCode(indent + 2, newLine));
      return result.ToString();
    }

    private static string DoToCode(SubCommands subCommands, int indent, bool newLine)
    {
      var result = new StringBuilder();
      if (1 < subCommands.Count)
      {
        newLine = true;
      }
      for (var i = 0; i < subCommands.Count; ++i)
      {
        var subCommand = subCommands[i] as SubCommand;
        if (0 < result.Length)
        {
          result.AppendNewLine(newLine);
        }
        result.AppendIndented(subCommand.ToCode(indent, newLine), newLine || i == 0 ? indent : 1, newLine);
      }
      return result.ToString();
    }

    private static string ToCode(this SubCommands subCommands, int indent, bool newLine)
    {
      var result = DoToCode(subCommands, indent, newLine);
      return !newLine && Globals.MaxCodeLineLength < result.Length ? DoToCode(subCommands, indent, true) : result;
    }
  }
}