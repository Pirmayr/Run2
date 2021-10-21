namespace Run2
{
  public interface ICommand
  {
    string Description { get; }

    bool HideHelp { get; }

    string Name { get; }

    List ParameterNames { get; }

    string Remarks { get; }

    string Returns { get; }

    string GetParameterDescription(string name);

    object Run(Items arguments);
  }
}