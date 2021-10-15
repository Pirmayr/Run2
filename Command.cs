namespace Run2
{
  internal abstract class Command
  {
    public abstract string GetDescription();

    public abstract bool GetHideHelp();

    public abstract string GetName();

    public abstract string GetParameterDescription(string name);

    public abstract List GetParameterNames();

    public abstract string GetRemarks();

    public abstract string GetReturns();

    public abstract object Run(TokensList arguments);
  }
}