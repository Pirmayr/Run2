#pragma warning disable 660, 661
namespace Run2
{
  internal sealed class WeakQuote : object
  {
    public string Value { get; }

    public WeakQuote(string value)
    {
      Value = value;
    }

    public static bool operator ==(string lhs, WeakQuote rhs)
    {
      return lhs == rhs?.Value;
    }

    public static bool operator ==(WeakQuote lhs, string rhs)
    {
      return lhs?.Value == rhs;
    }

    public static bool operator !=(string lhs, WeakQuote rhs)
    {
      return lhs != rhs?.Value;
    }

    public static bool operator !=(WeakQuote lhs, string rhs)
    {
      return lhs?.Value != rhs;
    }

    public override string ToString()
    {
      return Value;
    }
  }
}