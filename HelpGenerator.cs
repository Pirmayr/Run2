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
        var parameterNames = command.GetParameterNames();
        if (0 < parameterNames.Count)
        {
          result.Append("\n");
          foreach (var parameterName in parameterNames)
          {
            var parameterDescription = command.GetParameterDescription(parameterName);
            result.Append($"\n* {parameterName}" + (string.IsNullOrEmpty(parameterDescription) ? "" : $": {parameterDescription}"));
            if (string.IsNullOrEmpty(parameterDescription))
            {
              missingDocumentationParameters.Add(parameterName);
            }
          }
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
          result.Append("\n\nExamples:\n");
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
  }
}