using System;
using System.Collections.Generic;
using System.Text;

namespace Run2
{
  internal sealed class InvokeCommand : Command
  {
    public HashSet<string> FullNames { get; } = new();

    private string MemberName { get; }

    private Type Type { get; }

    public InvokeCommand(string memberName, Type type)
    {
      MemberName = memberName;
      Type = type;
    }

    public override string GetDescription()
    {
      var result = new StringBuilder("See:\n");
      foreach (var fullName in FullNames)
      {
        result.Append($"\n* https://docs.microsoft.com/en-us/dotnet/api/{fullName}");
      }
      return result.ToString();
    }

    public override bool GetHideHelp()
    {
      return FullNames.Count == 0;
    }

    public override string GetParameterDescription(string name)
    {
      return "";
    }

    public override List<string> GetParameterNames()
    {
      return new List<string>();
    }

    public override object Run(Tokens arguments)
    {
      return Helpers.Invoke(MemberName, Type ?? arguments.DequeueObject(), arguments.ToList(true).ToArray());
    }
  }
}