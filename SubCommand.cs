namespace Run2
{
  public sealed class SubCommand
  {
    internal Items Arguments { get; } = new();

    internal string CommandName { get; set; }
  }
}