using System.Text;

namespace Run2
{
  public sealed class SubCommand
  {
    internal Tokens Arguments { get; } = new();

    internal string CommandName { get; init; }

    internal string ToCode(int indent, bool newLine)
    {
      var result = DoToCode(indent, false);
      return !newLine && Globals.MaxCodeLineLength < result.Length ? DoToCode(indent, true) : result;
    }

    private string DoToCode(int indent, bool newLine)
    {
      var result = new StringBuilder();
      result.AppendIndented(CommandName, indent, newLine);
      result.Append(Arguments.ToCode(indent + 2, newLine));
      return result.ToString();
    }
  }
}