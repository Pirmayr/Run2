using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Run2
{
  internal sealed class SystemCommand : Command
  {
    private readonly CommandAction action;

    public SystemCommand(CommandAction action)
    {
      this.action = action;
    }

    public override string GetDescription()
    {
      var attribute = (DocumentationAttribute) Attribute.GetCustomAttribute(action.Method, typeof(DocumentationAttribute));
      return attribute != null ? attribute.Description : "";
    }

    public override bool GetHideHelp()
    {
      return false;
    }

    public override string GetName()
    {
      return action.Method.Name;
    }

    public override string GetParameterDescription(string name)
    {
      var attribute = (DocumentationAttribute) Attribute.GetCustomAttribute(action.Method, typeof(DocumentationAttribute));
      return attribute != null ? attribute.ParameterDescriptions[name] : "";
    }

    public override List<string> GetParameterNames()
    {
      var attribute = (DocumentationAttribute) Attribute.GetCustomAttribute(action.Method, typeof(DocumentationAttribute));
      return attribute?.ParameterDescriptions.Keys.ToList() ?? new List<string>();
    }

    public override string GetRemarks()
    {
      return "";
    }

    public override string GetReturns()
    {
      return "";
    }

    public override object Run(Tokens arguments)
    {
      var method = action.Method;
      var attribute = (DocumentationAttribute) Attribute.GetCustomAttribute(method, typeof(DocumentationAttribute));
      if (attribute != null)
      {
        var actualCount = arguments.Count;
        var expectedCountFrom = attribute.ArgumentsCountFrom;
        var expectedCountTo = attribute.ArgumentsCountTo;
        (expectedCountFrom <= actualCount && actualCount <= expectedCountTo).Check(GetInvalidParametersCountErrorMessage(method, actualCount, expectedCountFrom, expectedCountTo));
      }
      return action(arguments);
    }

    private static string GetInvalidParametersCountErrorMessage(MemberInfo memberInfo, int actualCount, int expectedCountFrom, int expectedCountTo)
    {
      var expected = expectedCountFrom == expectedCountTo ? $"{expectedCountFrom}" : $"{expectedCountFrom} to {expectedCountTo}";
      return $"Number of arguments encountered for command '{memberInfo.Name.ToLower()}': {actualCount}; number expected: {expected}";
    }
  }
}