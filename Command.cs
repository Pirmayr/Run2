using System.Collections.Generic;

namespace Run2
{
  internal abstract class Command
  {
    public abstract string GetDescription();

    public abstract bool GetHideHelp();

    public abstract string GetName();

    public abstract string GetParameterDescription(string name);

    public abstract List<string> GetParameterNames();

    public abstract object Run(Tokens arguments);
  }
}