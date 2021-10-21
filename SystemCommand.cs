﻿using System;

namespace Run2
{
  public sealed class SystemCommand : ICommand
  {
    private readonly CommandAction action;

    public string Description
    {
      get
      {
        var attribute = (DocumentationAttribute) Attribute.GetCustomAttribute(action.Method, typeof(DocumentationAttribute));
        return attribute != null ? attribute.Description : "";
      }
    }

    public bool HideHelp => false;

    public string Name => action.Method.Name;

    public List ParameterNames
    {
      get
      {
        var attribute = (DocumentationAttribute) Attribute.GetCustomAttribute(action.Method, typeof(DocumentationAttribute));
        return attribute == null ? new List() : new List(attribute.ParameterDescriptions.Keys);
      }
    }

    public string Remarks => "";

    public string Returns => "";

    public SystemCommand(CommandAction action)
    {
      this.action = action;
    }

    public string GetParameterDescription(string name)
    {
      var attribute = (DocumentationAttribute) Attribute.GetCustomAttribute(action.Method, typeof(DocumentationAttribute));
      return attribute != null ? attribute.ParameterDescriptions[name] : "";
    }

    public object Run(Items arguments)
    {
      var method = action.Method;
      var attribute = (DocumentationAttribute) Attribute.GetCustomAttribute(method, typeof(DocumentationAttribute));
      if (attribute != null)
      {
        var actualCount = arguments.QueueCount;
        var expectedCountFrom = attribute.ArgumentsCountFrom;
        var expectedCountTo = attribute.ArgumentsCountTo;
        (expectedCountFrom <= actualCount && actualCount <= expectedCountTo).Check(Helpers.GetInvalidParametersCountErrorMessage(method.Name.ToLower(), actualCount, expectedCountFrom, expectedCountTo));
      }
      return action(arguments);
    }
  }
}