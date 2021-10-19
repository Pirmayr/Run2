using System;
using System.Collections.Generic;
using System.Text;

namespace Run2
{
  internal sealed class InvokeCommand : ICommand
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

    public List ParameterNames
    {
      get { return new List(); }
    }

    public string Remarks
    {
      get { return ""; }
    }

    public string Returns
    {
      get { return ""; }
    }

    public object Run(Items arguments)
    {
      return Helpers.Invoke(MemberName, Type ?? arguments.DequeueObject(), arguments.ToList(true).ToArray());
    }
  }
}