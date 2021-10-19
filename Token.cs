using System.Diagnostics;

namespace Run2
{
  [DebuggerDisplay("{TokenKind} {Value.GetType().Name} {Value}")]
  public class Token
  {
    public TokenKind TokenKind { get; set; }

    public object Value { get; set; }

    public Token(TokenKind tokenKind, object value)
    {
      TokenKind = tokenKind;
      Value = value;
    }

    public override string ToString()
    {
      return Value.ToString();
    }
  }
}