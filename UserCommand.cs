using System.Collections.Generic;
using System.Linq;

namespace Run2
{
  internal sealed class UserCommand : Command
  {
    public string Description { get; set; }

    public bool IsQuoted { get; init; }

    public string Name { get; init; }

    public Dictionary<string, string> ParameterDescriptions { get; } = new();

    public List ParameterNames { get; } = new();

    public string Remarks { get; set; }

    public string Returns { get; set; }

    public string ScriptPath { get; init; }

    public SubCommands SubCommands { get; } = new();

    public override string GetDescription()
    {
      return Description;
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

    public override List GetParameterNames()
    {
      return ParameterNames;
    }

    public override string GetRemarks()
    {
      return Remarks;
    }

    public override string GetReturns()
    {
      return Returns;
    }

    public override object Run(Items arguments)
    {
      Run2.EnterScope();
      var actualCount = arguments.Count;
      GetParameterCounts(out var expectedCountFrom, out var expectedCountTo);
      (ParameterNames.Count == 0 || expectedCountFrom <= actualCount && actualCount <= expectedCountTo).Check(Helpers.GetInvalidParametersCountErrorMessage(Name, actualCount, expectedCountFrom, expectedCountTo));
      foreach (var item in ParameterNames)
      {
        object defaultValue = null;
        var isOptional = false;
        if (item is Items tokensValue)
        {
          isOptional = true;
          if (2 <= tokensValue.Count)
          {
            defaultValue = Run2.Evaluate(tokensValue.ElementAtOrDefault(1));
          }
        }
        var parameterName = item.GetNameFromToken();
        var isQuotedParameter = parameterName.IsStrongQuote(out var unquotedParameterName);
        var actualParameterName = isQuotedParameter ? unquotedParameterName : parameterName;
        object value;
        if (0 < arguments.Count)
        {
          value = arguments.DequeueObject(!isQuotedParameter);
        }
        else
        {
          isOptional.Check("Missing arguments must be declared as optional");
          value = defaultValue;
        }
        Run2.SetLocalVariable(actualParameterName, value);
      }
      var argumentsList = arguments.ToList(!IsQuoted);
      Run2.SetLocalVariable(Globals.VariableNameArguments, argumentsList);
      var result = Run2.RunSubCommands(SubCommands);
      Run2.LeaveScope();
      return result;
    }

    private void GetParameterCounts(out int countFrom, out int countTo)
    {
      countTo = ParameterNames.Count;
      countFrom = 0;
      foreach (var parameterName in ParameterNames)
      {
        if (parameterName is Items)
        {
          break;
        }
        ++countFrom;
      }
    }
  }
}