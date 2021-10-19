using System;

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

    public override List GetParameterNames()
    {
      var attribute = (DocumentationAttribute) Attribute.GetCustomAttribute(action.Method, typeof(DocumentationAttribute));
      // return attribute?.ParameterDescriptions.Keys.ToList() ?? new List();
      return attribute == null ? new List() : new List(attribute.ParameterDescriptions.Keys);
    }

    public override string GetRemarks()
    {
      return "";
    }

    public override string GetReturns()
    {
      return "";
    }

    public override object Run(Items arguments)
    {
      var method = action.Method;
      var attribute = (DocumentationAttribute) Attribute.GetCustomAttribute(method, typeof(DocumentationAttribute));
      if (attribute != null)
      {
        var actualCount = arguments.Count;
        var expectedCountFrom = attribute.ArgumentsCountFrom;
        var expectedCountTo = attribute.ArgumentsCountTo;
        (expectedCountFrom <= actualCount && actualCount <= expectedCountTo).Check(Helpers.GetInvalidParametersCountErrorMessage(method.Name.ToLower(), actualCount, expectedCountFrom, expectedCountTo));
      }
      return action(arguments);
    }
  }
}