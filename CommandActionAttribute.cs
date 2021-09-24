using System;

namespace Run2
{
  internal sealed class CommandActionAttribute : Attribute
  {
    public int ArgumentsCountFrom { get; }

    public int ArgumentsCountTo { get; }

    public string CommandName { get; }

    public CommandActionAttribute(int argumentsCountFrom, int argumentsCountTo, string commandName = null)
    {
      ArgumentsCountFrom = argumentsCountFrom;
      ArgumentsCountTo = argumentsCountTo;
      CommandName = commandName;
    }
  }
}