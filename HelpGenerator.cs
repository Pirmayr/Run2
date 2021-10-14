using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Run2
{
  internal static class HelpGenerator
  {
    public static string GetHelp()
    {
      var commandReferences = GetCommandReferences(Globals.TestCommand);
      var result = new StringBuilder();
      var missingReferences = "";
      var missingDocumentation = "";
      var insertLine = false;
      result.Append("# Predefined Run2-Commands");
      foreach (var (name, command) in Globals.Commands.Where(static item => !item.Value.GetHideHelp()).OrderBy(static item => item.Key))
      {
        if (insertLine)
        {
          result.Append("\n\n---");
        }
        insertLine = true;
        result.Append($"\n\n#### {name}");
        if (!string.IsNullOrEmpty(command.GetDescription()))
        {
          result.Append($"\n\n{command.GetDescription()}");
        }
        var commandDocumentationMissing = string.IsNullOrEmpty(command.GetDescription());
        var missingDocumentationParameters = new List<string>();
        HandleParameters(result, command, missingDocumentationParameters);
        if (!string.IsNullOrEmpty(command.GetReturns()))
        {
          result.Append("\n\n**Returns**\n");
          result.Append($"\n\n{command.GetReturns()}");
        }
        if (!string.IsNullOrEmpty(command.GetRemarks()))
        {
          result.Append("\n\n**Remarks**\n");
          result.Append($"\n\n{command.GetRemarks()}");
        }
        if (commandDocumentationMissing || 0 < missingDocumentationParameters.Count)
        {
          missingDocumentation += 0 < missingDocumentation.Length ? "\n" : "";
          missingDocumentation += $"\n* {name}";
          foreach (var parameterName in missingDocumentationParameters)
          {
            missingDocumentation += $"\n  - {parameterName}";
          }
        }
        if (commandReferences.TryGetValue(name, out var references))
        {
          result.Append("\n\n**Examples**\n");
          foreach (var reference in references)
          {
            result.Append("\n~~~");
            result.Append($"\n{reference.ToCode(0, true)}");
            result.Append("\n~~~");
          }
        }
        else if (name != Globals.TestCommand && name != Path.GetFileNameWithoutExtension(Globals.ScriptName) && command is UserCommand)
        {
          if (0 < missingReferences.Length)
          {
            missingReferences += '\n';
          }
          missingReferences += $"* {name}";
        }
      }
      if (!string.IsNullOrEmpty(missingDocumentation))
      {
        result.Append($"\n\n#### Missing Documentation:\n\n{missingDocumentation}");
      }
      if (!string.IsNullOrEmpty(missingReferences))
      {
        result.Append($"\n\n#### Missing Examples:\n\n{missingReferences}");
      }
      return result.ToString();
    }

    private static void GetCommandNames(SubCommand subCommand, ref HashSet<string> commandNames)
    {
      commandNames.Add(subCommand.CommandName);
      foreach (var token in subCommand.Arguments)
      {
        if (token is SubCommands subCommandsValue)
        {
          foreach (var subCommandValue in subCommandsValue)
          {
            GetCommandNames(subCommandValue, ref commandNames);
          }
        }
      }
    }

    private static Dictionary<string, List<SubCommand>> GetCommandReferences(string filterCommandName)
    {
      var filteredSubCommands = new List<SubCommand>();
      foreach (var command in Globals.Commands.Values)
      {
        if (command is UserCommand userCommand)
        {
          foreach (var subCommand in userCommand.SubCommands)
          {
            if (subCommand.CommandName == filterCommandName)
            {
              filteredSubCommands.Add(subCommand);
            }
          }
        }
      }
      var result = new Dictionary<string, List<SubCommand>>();
      foreach (var filteredSubCommand in filteredSubCommands)
      {
        var commandNames = new HashSet<string>();
        GetCommandNames(filteredSubCommand, ref commandNames);
        foreach (var commandName in commandNames)
        {
          if (commandName != filterCommandName)
          {
            if (!result.TryGetValue(commandName, out var references))
            {
              references = new List<SubCommand>();
              result.Add(commandName, references);
            }
            references.Add(filteredSubCommand);
          }
        }
      }
      return result;
    }

    private static void HandleParameters(StringBuilder builder, Command command, ICollection<string> missingDocumentationParameters)
    {
      var parameterNames = command.GetParameterNames();
      if (0 < parameterNames.Count)
      {
        builder.Append("\n");
        foreach (var item in parameterNames)
        {
          item.GetParameterInformation(out var parameterName, out var isOptional, out var defaultValue);
          var parameterDescription = command.GetParameterDescription(parameterName);
          builder.Append($"\n* {parameterName}" + (string.IsNullOrEmpty(parameterDescription) ? "" : $": {parameterDescription}"));
          if (isOptional)
          {
            if (defaultValue == null)
            {
              builder.Append($" (optional)");
            }
            else
            {
              builder.Append($" (optional; default: {CodeFormatter.ToCode(defaultValue)})");
            }
          }
          if (string.IsNullOrEmpty(parameterDescription))
          {
            missingDocumentationParameters.Add(parameterName);
          }
        }
      }
    }
  }
}