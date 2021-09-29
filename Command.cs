using System.Collections.Generic;

namespace Run2
{
  internal abstract class Command
  {
    public abstract bool GetHideHelp();

    public abstract string GetDescription();

    public abstract string GetParameterDescription(string name);

    public abstract List<string> GetParameterNames();

    public abstract object Run(Tokens arguments);
  }
}