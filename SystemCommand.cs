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