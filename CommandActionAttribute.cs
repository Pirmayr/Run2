using System;
using System.Collections.Generic;

namespace Run2
{
  internal sealed class CommandActionAttribute : Attribute
  {
    public int ArgumentsCountFrom { get; }

    public int ArgumentsCountTo { get; }

    public string CommandName { get; }

    public string Description { get; }

    public Dictionary<string, string> ParameterDescriptions { get; } = new();

    public CommandActionAttribute(int argumentsCountFrom, int argumentsCountTo, string commandName = null, string description = "", params string[] parameters)
    {
      ArgumentsCountFrom = argumentsCountFrom;
      ArgumentsCountTo = argumentsCountTo;
      CommandName = commandName;
      Description = description;
      (parameters.Length % 2 == 0).Check("The number of strings describing command-parameters must be odd (= parameter-name plus parameter-description)");
      for (var i = 0; i < parameters.Length; i += 2)
      {
        ParameterDescriptions.Add(parameters[i], parameters[i + 1]);
      }
    }
  }
}