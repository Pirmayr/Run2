using System.Diagnostics;

namespace Run2
{
  [DebuggerDisplay("{TokenKind} {Value.GetType().Name} {Value}")]
  public sealed class Token
  {
    public int LineNumber { get; }

    public string ScriptPath { get; set; }

    public TokenKind TokenKind { get; set; }

    public object Value { get; init; }

    public Token(TokenKind tokenKind, object value, string scriptPath, int lineNumber)
    {
      TokenKind = tokenKind;
      Value = value;
      ScriptPath = scriptPath;
      LineNumber = lineNumber;
    }

    public override string ToString()
    {
      return Value.ToString();
    }
  }
}