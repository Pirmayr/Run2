using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Run2
{
  [SuppressMessage("ReSharper", "MemberCanBeInternal")]
  public sealed class SubCommands : List<SubCommand>
  {
    public string ToCode(int indent, bool newLine)
    {
      var result = DoToCode(indent, newLine);
      return !newLine && Globals.MaxCodeLineLength < result.Length ? DoToCode(indent, true) : result;
    }

    private string DoToCode(int indent, bool newLine)
    {
      var result = new StringBuilder();
      for (var i = 0; i < Count; ++i)
      {
        var subCommand = this[i];
        if (0 < result.Length)
        {
          result.AppendNewLine(newLine);
        }
        result.AppendIndented(subCommand.ToCode(indent, newLine), newLine || i == 0 ? indent : 1, newLine);
      }
      return result.ToString();
    }
  }
}