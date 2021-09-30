using System;
using System.Collections.Generic;
using System.Linq;

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
      var attribute = (CommandActionAttribute) Attribute.GetCustomAttribute(action.Method, typeof(CommandActionAttribute));
      return attribute != null ? attribute.Description : "";
    }

    public override bool GetHideHelp()
    {
      return false;
    }

    public override string GetParameterDescription(string name)
    {
      var attribute = (CommandActionAttribute) Attribute.GetCustomAttribute(action.Method, typeof(CommandActionAttribute));
      return attribute != null ? attribute.ParameterDescriptions[name] : "";
    }

    public override List<string> GetParameterNames()
    {
      var attribute = (CommandActionAttribute) Attribute.GetCustomAttribute(action.Method, typeof(CommandActionAttribute));
      return attribute?.ParameterDescriptions.Keys.ToList() ?? new List<string>();
    }

    public override object Run(Tokens arguments)
    {
      var method = action.Method;
      var attribute = (CommandActionAttribute) Attribute.GetCustomAttribute(method, typeof(CommandActionAttribute));
      if (attribute != null)
      {
        var actualCount = arguments.Count;
        var expectedCountFrom = attribute.ArgumentsCountFrom;
        var expectedCountTo = attribute.ArgumentsCountTo;
        (expectedCountFrom <= actualCount && actualCount <= expectedCountTo).Check($"Command '{method.Name}' has {actualCount} arguments, but from {expectedCountFrom} to {expectedCountTo} were expected");
      }
      return action(arguments);
    }
  }
}