﻿using System;
using System.Collections.Generic;

namespace Run2
{
  internal sealed class InvokeCommand : Command
  {
    private string MemberName { get; }

    private Type Type { get; }

    private string FullName { get; set; }

    public InvokeCommand(string memberName, Type type, string fullName)
    {
      MemberName = memberName;
      Type = type;
      FullName = fullName;
    }

    public override List<string> GetParameterNames()
    {
      return new List<string>();
    }

    public override object Run(Tokens arguments)
    {
      return Helpers.Invoke(MemberName, Type ?? arguments.DequeueBestType(), arguments.ToList(true).ToArray());
    }

    public override string GetDescription()
    {
      return string.IsNullOrEmpty(FullName) ? "" : $"See: https://docs.microsoft.com/en-us/dotnet/api/{FullName}";
    }

    public override string GetParameterDescription(string name)
    {
      return "";
    }
  }
}