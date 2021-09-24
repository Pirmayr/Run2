namespace Run2
{
  internal sealed class SubCommand
  {
    public Tokens Arguments { get; } = new();

    public string CommandName { get; init; }
  }
}