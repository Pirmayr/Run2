using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Run2
{
  [SuppressMessage("ReSharper", "MemberCanBeInternal")]
  public sealed class SubCommands : List<SubCommand>
  {
    public int LineNumber { get; set; } = -1;
  }
}