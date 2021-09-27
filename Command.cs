using System.Collections.Generic;

namespace Run2
{
  internal abstract class Command
  {
    public string CommandDescription { get; set; }

    public Dictionary<string, string> ParameterDescriptions { get; } = new();

    public abstract object Run(Tokens arguments);
  }
}