namespace Run2
{
  internal abstract class Command
  {
    public abstract object Run(Tokens arguments);
  }
}