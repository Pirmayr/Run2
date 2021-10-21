using System.Diagnostics;

namespace Run2
{
  [DebuggerDisplay("{TokenKind} {Value.GetType().Name} {Value}")]
  public sealed class Token
  {
    public int LineNumber { get; }

    public TokenKind TokenKind { get; set; }

    public object Value { get; init; }

    public Token(TokenKind tokenKind, object value, int lineNumber)
    {
      TokenKind = tokenKind;
      Value = value;
      LineNumber = lineNumber;
    }

    public override string ToString()
    {
      return Value.ToString();
    }
  }
}