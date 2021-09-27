namespace Run2
{
  internal sealed class UserCommand : Command
  {
    public string Name { get; init; }

    public Tokens ParameterNames { get; } = new();

    public SubCommands SubCommands { get; set; } = new();

    public override object Run(Tokens arguments)
    {
      Run2.EnterScope();
      (ParameterNames.Count == 0 || arguments.Count == ParameterNames.Count).Check($"Command '{Name}' has {arguments.Count} arguments, but {ParameterNames.Count} were expected");
      foreach (var parameter in ParameterNames)
      {
        if (parameter is Tokens tokensValue)
        {
          (tokensValue.Count == 1).Check("Expected exactly one token for parameters");
          Run2.SetLocalVariable(tokensValue.PeekString(), arguments.DequeueBestType(false));
        }
        else
        {
          Run2.SetLocalVariable(parameter.ToString(), arguments.DequeueBestType());
        }
      }
      Run2.SetLocalVariable("arguments", arguments.ToList(false));
      var result = Run2.RunSubCommands(SubCommands);
      Run2.LeaveScope();
      return result;
    }
  }
}