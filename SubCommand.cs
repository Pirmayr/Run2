using System.Text;

namespace Run2
{
  public sealed class SubCommand
  {
    internal Tokens Arguments { get; } = new();

    internal string CommandName { get; init; }

    public string ToCode()
    {
      var result = new StringBuilder();
      result.Append(CommandName);
      result.Append(' ');
      result.Append(Arguments.ToCode());
      return result.ToString();
    }
  }
}