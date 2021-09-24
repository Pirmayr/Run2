using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Run2
{
  [SuppressMessage("ReSharper", "MemberCanBeInternal")]
  public sealed class SubCommands : List<SubCommand>
  {
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public string ToCode()
    {
      var result = new StringBuilder();
      foreach (var subCommand in this)
      {
        if (0 < result.Length)
        {
          result.Append(' ');
        }
        result.Append(subCommand.CommandName);
        result.Append(' ');
        result.Append(subCommand.Arguments.ToCode());
      }
      return result.ToString();
    }
  }
}