using System.Collections.Generic;

namespace Run2
{
  internal sealed class UserCommand : Command
  {
    public Tokens ParameterNames { get; } = new();

    public List<SubCommand> SubCommands { get; } = new();

    public override object Run(Tokens arguments)
    {
      object result = null;
      Run2.EnterScope();
      (ParameterNames.Count == 0 || arguments.Count == ParameterNames.Count).Check($"Arguments-count {arguments.Count} is different from parameters-count {ParameterNames.Count}");
      foreach (var parameter in ParameterNames)
      {
        if (parameter is Tokens tokensValue)
        {
          Run2.SetLocalVariable(tokensValue.PeekString(), arguments.DequeueBestType(false));
        }
        else
        {
          Run2.SetLocalVariable(parameter.ToString(), arguments.DequeueBestType());
        }
      }
      Run2.SetLocalVariable("arguments", arguments.ToList(false));
      foreach (var subCommand in SubCommands)
      {
        result = Run2.RunCommand(subCommand.CommandName, subCommand.Arguments);
      }
      Run2.LeaveScope();
      return result;
    }
  }
}