using System.Collections.Generic;

namespace Run2
{
  internal sealed class UserCommand : Command
  {
    public List<string> ParameterNames { get; } = new();

    public List<SubCommand> SubCommands { get; } = new();

    public override object Run(Tokens arguments)
    {
      object result = null;
      Run2.EnterScope();
      (ParameterNames.Count == 0 || arguments.Count == ParameterNames.Count).Check($"Arguments-count {arguments.Count} is different from parameters-count {ParameterNames.Count}");
      foreach (var parameter in ParameterNames)
      {
        Run2.SetLocalVariable(parameter, arguments.DequeueBestType());
      }
      Run2.SetLocalVariable("arguments", arguments);
      foreach (var subCommand in SubCommands)
      {
        result = Run2.RunCommand(subCommand.CommandName, subCommand.Arguments);
      }
      Run2.LeaveScope();
      return result;
    }
  }
}