namespace Run2
{
  internal interface ICommand
  {
    string Description { get; }

    bool HideHelp { get; }

    string Name { get; }

    string GetParameterDescription(string name);

    List ParameterNames { get; }

    string Remarks { get; }

    string Returns { get; }

    object Run(Items arguments);
  }
}