using System.Collections.Generic;

namespace Run2
{
  public sealed class Tokens : Queue<Token>
  {
    public new Token Dequeue()
    {
      var result = base.Dequeue();
      result.SetCurrentScriptPathAndLineNumber();
      return result;
    }
  }
}