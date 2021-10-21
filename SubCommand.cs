namespace Run2
{
  public sealed class SubCommand
  {
    public Items Arguments { get; } = new();

    public string CommandName { get; set; }
  }
}