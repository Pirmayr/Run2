using System;

namespace Run2
{
  internal sealed class InvokeCommand : Command
  {
    private string MemberName { get; }

    private Type Type { get; }

    public InvokeCommand(string memberName, Type type)
    {
      MemberName = memberName;
      Type = type;
    }

    public override object Run(Tokens arguments)
    {
      return Helpers.Invoke(MemberName, Type ?? arguments.DequeueBestType(), arguments.ToList(true).ToArray());
    }
  }
}