using System.Collections.Generic;
using System.Linq;

namespace Run2
{
  public sealed class UserCommand : ICommand
  {
    public string Description { get; set; }

    public bool HideHelp => false;

    public string Name { get; init; }

    public Dictionary<string, string> ParameterDescriptions { get; } = new();

    public List ParameterNames { get; } = new();

    public bool QuoteArguments { get; init; }

    public string Remarks { get; set; }

    public string Returns { get; set; }

    public string ScriptPath { get; init; }

    public SubCommands SubCommands { get; } = new();

    public string GetParameterDescription(string name)
    {
      return ParameterDescriptions.TryGetValue(name, out var result) ? result : "";
    }

    public object Run(Items arguments)
    {
      Globals.Variables.EnterScope();
      var actualCount = arguments.QueueCount;
      GetParameterCounts(out var expectedCountFrom, out var expectedCountTo);
      (ParameterNames.Count == 0 || expectedCountFrom <= actualCount && actualCount <= expectedCountTo).Check(Helpers.GetInvalidParametersCountErrorMessage(Name, actualCount, expectedCountFrom, expectedCountTo));
      foreach (var item in ParameterNames)
      {
        object defaultValue = null;
        var isOptional = false;
        if (item is Items itemsValue)
        {
          isOptional = true;
          if (2 <= itemsValue.QueueCount)
          {
            defaultValue = Run2.Evaluate(itemsValue.ElementAtOrDefault(1));
          }
        }
        var parameterName = item.GetNameFromItem();
        var isQuotedParameter = parameterName.IsStrongQuote(out var unquotedParameterName);
        var actualParameterName = isQuotedParameter ? unquotedParameterName : parameterName;
        object value;
        if (0 < arguments.QueueCount)
        {
          value = arguments.DequeueObject(!isQuotedParameter);
        }
        else
        {
          isOptional.Check("Missing arguments must be declared as optional");
          value = defaultValue;
        }
        Globals.Variables.SetLocal(actualParameterName, value);
      }
      var argumentsList = arguments.ToList(!QuoteArguments);
      Globals.Variables.SetLocal(Globals.VariableNameArguments, argumentsList);
      var result = Run2.ExecuteSubCommands(SubCommands);
      Globals.Variables.LeaveScope();
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