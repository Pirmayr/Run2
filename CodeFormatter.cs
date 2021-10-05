using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Run2
{
  internal static class CodeFormatter
  {
    public static string ToCode()
    {
      var result = new StringBuilder();
      foreach (var command in Globals.Commands.Values.OrderBy(static item => item.GetName()))
      {
        if (command is UserCommand userCommandValue)
        {
          if (0 < result.Length)
          {
            result.Append("\n\n");
          }
          result.Append(userCommandValue.IsQuoted ? $"command \"{userCommandValue.Name}\"" : $"command {userCommandValue.Name}");
          if (!string.IsNullOrEmpty(userCommandValue.CommandDescription))
          {
            result.Append($" '{userCommandValue.CommandDescription}'");
          }
          foreach (var parameterName in userCommandValue.GetParameterNames())
          {
            result.AppendNewLine(true);
            result.AppendIndented(parameterName, 2, true);
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

    public static string ToCode(this SubCommand subCommand, int indent, bool newLine)
    {
      var result = DoToCode(subCommand, indent, false);
      return !newLine && Globals.MaxCodeLineLength < result.Length ? DoToCode(subCommand, indent, true) : result;
    }

    private static string DoToCode(Tokens tokens, int indent, bool newLine)
    {
      var multilineSubCommandWritten = false;
      var result = new StringBuilder();
      foreach (var item in tokens)
      {
        switch (item)
        {
          case SubCommands subCommandsValue:
          {
            var subCommandsCode = subCommandsValue.ToCode(indent + 2, newLine);
            if (Globals.MaxCodeLineLength < subCommandsCode.Length || subCommandsCode.Contains('\n'))
            {
              var localNewLine = subCommandsCode.Contains('\n') || newLine;
              result.AppendNewLine(localNewLine);
              result.AppendIndented("(", indent, localNewLine);
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
          case WeaklyQuotedString:
          {
            result.AppendNewLine(multilineSubCommandWritten && newLine);
            result.AppendIndented($"'{item.ToString()?.Replace("\n", "~n")}'", indent, multilineSubCommandWritten && newLine);
            multilineSubCommandWritten = false;
            break;
          }
          default:
            result.AppendNewLine(multilineSubCommandWritten && newLine);
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
            result.AppendIndented($"{itemString}", indent, multilineSubCommandWritten && newLine);
            multilineSubCommandWritten = false;
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
        var subCommand = subCommands[i];
        if (0 < result.Length)
        {
          result.AppendNewLine(newLine);
        }
        result.AppendIndented(subCommand.ToCode(indent, newLine), newLine || i == 0 ? indent : 1, newLine);
      }
      return result.ToString();
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    private static string ToCode(this Tokens tokens, int indent, bool newLine)
    {
      var result = DoToCode(tokens, indent, false);
      return !newLine && Globals.MaxCodeLineLength < result.Length ? DoToCode(tokens, indent, true) : result;
    }

    private static string ToCode(this SubCommands subCommands, int indent, bool newLine)
    {
      var result = DoToCode(subCommands, indent, newLine);
      return !newLine && Globals.MaxCodeLineLength < result.Length ? DoToCode(subCommands, indent, true) : result;
    }
  }
}