﻿using System.Collections.Generic;

namespace Run2
{
  internal sealed class UserCommand : Command
  {
    public Tokens ParameterNames { get; } = new();

    public SubCommands SubCommands { get; set; } = new();

    public override object Run(Tokens arguments)
    {
      Run2.EnterScope();
      (ParameterNames.Count == 0 || arguments.Count == ParameterNames.Count).Check($"Arguments-count {arguments.Count} is different from parameters-count {ParameterNames.Count}");
      foreach (var parameter in ParameterNames)
      {
        if (parameter is Tokens tokensValue)
        {
          (tokensValue.Count == 1).Check("Expected exactly one token for parameters");
          Run2.SetLocalVariable(tokensValue.PeekString(), arguments.DequeueBestType(false));
        }
        else if (parameter is SubCommands subCommands)
        {
          (subCommands.Count == 1).Check("Expected exactly 1 sub-command for parameters");
          var subCommand = subCommands[0];
          (subCommand.Arguments.Count == 0).Check("Expected sub-command without arguments for parameters");
          Run2.SetLocalVariable(subCommand.CommandName, arguments.DequeueBestType(false));
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