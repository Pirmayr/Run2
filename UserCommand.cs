using System.Collections.Generic;
using System.Linq;

namespace Run2
{
  internal sealed class UserCommand : Command
  {
    public string CommandDescription { get; set; }

    public string Name { get; init; }

    public Dictionary<string, string> ParameterDescriptions { get; } = new();

    public Tokens ParameterNames { get; } = new();

    public SubCommands SubCommands { get; set; } = new();

    public override string GetDescription()
    {
      return CommandDescription;
    }

    public override string GetParameterDescription(string name)
    {
      return ParameterDescriptions.TryGetValue(name, out var result) ? result : "";
    }

    public override List<string> GetParameterNames()
    {
      return ParameterNames.Select(Helpers.ParameterName).ToList();
    }

    public override object Run(Tokens arguments)
    {
      Run2.EnterScope();
      (ParameterNames.Count == 0 || arguments.Count == ParameterNames.Count).Check($"Command '{Name}' has {arguments.Count} arguments, but {ParameterNames.Count} were expected");
      foreach (var parameter in ParameterNames)
      {
        if (parameter is Tokens tokensValue)
        {
          (tokensValue.Count == 1).Check("Expected exactly one token for parameters");
          Run2.SetLocalVariable(tokensValue.PeekString(), arguments.DequeueBestType(false));
        }
        else
        {
          Run2.SetLocalVariable(parameter.ToString(), arguments.DequeueBestType());
        }
      }
      Run2.SetLocalVariable("arguments", arguments.ToList(false));
      var result = Run2.RunSubCommands(SubCommands);
      Run2.LeaveScope();
      return result;
    }
  }
}