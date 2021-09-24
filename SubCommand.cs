namespace Run2
{
  public sealed class SubCommand
  {
    public Tokens Arguments { get; } = new();

    public string CommandName { get; init; }
  }
}