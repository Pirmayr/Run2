using System.Collections.Generic;

namespace Run2
{
  internal sealed class UserCommand : Command
  {
    public string Description { get; set; }

    public string Remarks { get; set; }

    public string Returns { get; set; }

    public bool IsQuoted { get; init; }

    public string Name { get; init; }

    public Dictionary<string, string> ParameterDescriptions { get; } = new();

    public List<string> ParameterNames { get; } = new();

    public string ScriptPath { get; init; }

    public SubCommands SubCommands { get; set; } = new();

    public override string GetDescription()
    {
      return Description;
    }

    public override string GetRemarks()
    {
      return Remarks;
    }

    public override string GetReturns()
    {
      return Returns;
    }

    public override bool GetHideHelp()
    {
      return false;
    }

    public override string GetName()
    {
      return Name;
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
      (ParameterNames.Count == 0 || arguments.Count <= ParameterNames.Count).Check($"Command '{Name}' has {arguments.Count} arguments, but {ParameterNames.Count} were expected");
      foreach (var parameterName in ParameterNames)
      {
        var isQuotedParameter = parameterName.IsStrongQuote(out var unquotedParameterName);
        var actualParameterName = isQuotedParameter ? unquotedParameterName : parameterName;
        var value = 0 < arguments.Count ? arguments.DequeueObject(!isQuotedParameter) : null;
        Run2.SetLocalVariable(actualParameterName, value);
      }
      var argumentsList = arguments.ToList(!IsQuoted);
      Run2.SetLocalVariable(Globals.VariableNameArguments, argumentsList);
      var result = Run2.RunSubCommands(SubCommands);
      Run2.LeaveScope();
      return result;
    }
  }
}