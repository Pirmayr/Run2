namespace Run2
{
  public sealed class SubCommand
  {
    // ReSharper disable once UnusedMember.Global
    public int Count => Arguments.Count + 1;

    internal TokensList Arguments { get; } = new();

    internal string CommandName { get; init; }

    // ReSharper disable once UnusedMember.Global
    public object this[int index] => index == 0 ? CommandName : Arguments.ToArray()[index - 1];
  }
}