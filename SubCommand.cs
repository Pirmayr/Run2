namespace Run2
{
  public sealed class SubCommand
  {
    internal Tokens Arguments { get; } = new();

    internal string CommandName { get; init; }
  }
}