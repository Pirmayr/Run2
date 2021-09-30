using System.Collections.Generic;

namespace Run2
{
  internal sealed class UserCommand : Command
  {
    public string CommandDescription { get; set; }

    public string Name { get; init; }

    public Dictionary<string, string> ParameterDescriptions { get; } = new();

    public List<string> ParameterNames { get; } = new();

    public SubCommands SubCommands { get; set; } = new();

    public override string GetDescription()
    {
      return CommandDescription;
    }

    public bool IsQuoted { get; set; } 

    public override bool GetHideHelp()
    {
      return false;
    }

    public override string GetParameterDescription(string name)
    {
      return ParameterDescriptions.TryGetValue(name, out var result) ? result : "";
    }

    public override List<string> GetParameterNames()
    {
      return ParameterNames;
    }

    public override object Run(Tokens arguments)
    {
      Run2.EnterScope();
      (ParameterNames.Count == 0 || arguments.Count == ParameterNames.Count).Check($"Command '{Name}' has {arguments.Count} arguments, but {ParameterNames.Count} were expected");
      foreach (var parameterName in ParameterNames)
      {
        if (parameterName.IsStronglyQuotedString(out var stronglyQuotedStringValue))
        {
          Run2.SetLocalVariable(stronglyQuotedStringValue, arguments.DequeueBestType(false));
        }
        else
        {
          Run2.SetLocalVariable(parameterName, arguments.DequeueBestType());
        }
      }
      var argumentsList = arguments.ToList(!IsQuoted);
      Run2.SetLocalVariable("arguments", argumentsList);
      var result = Run2.RunSubCommands(SubCommands);
      Run2.LeaveScope();
      return result;
    }
  }
}