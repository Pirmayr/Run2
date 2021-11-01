using System;
using System.Collections.Generic;
using System.Text;

namespace Run2
{
  public sealed class InvokeCommand : ICommand
  {
    public string Description
    {
      get
      {
        var result = new StringBuilder("See:\n");
        foreach (var fullName in FullNames)
        {
          result.Append($"\n* https://docs.microsoft.com/en-us/dotnet/api/{fullName}");
        }
        return result.ToString();
      }
    }

    public HashSet<string> FullNames { get; } = new();

    public bool HideHelp => FullNames.Count == 0;

    public string Name => MemberName;

    public Globals.List ParameterNames => new();

    public string Remarks => "";

    public string Returns => "";

    private string MemberName { get; }

    private Type Type { get; }

    public InvokeCommand(string memberName, Type type)
    {
      MemberName = memberName;
      Type = type;
    }

    public string GetParameterDescription(string name)
    {
      return "";
    }

    public object Run(Items arguments)
    {
      return Globals.Invoke(MemberName, Type ?? arguments.DequeueObject(), arguments.ToList(true).ToArray());
    }
  }
}