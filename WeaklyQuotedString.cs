#pragma warning disable 660, 661
namespace Run2
{
  internal sealed class WeaklyQuotedString : object
  {
    public string Value { get; }

    public WeaklyQuotedString(string value)
    {
      Value = value;
    }

    public static bool operator ==(string lhs, WeaklyQuotedString rhs)
    {
      return lhs == rhs?.Value;
    }

    public static bool operator ==(WeaklyQuotedString lhs, string rhs)
    {
      return lhs?.Value == rhs;
    }

    public static bool operator !=(string lhs, WeaklyQuotedString rhs)
    {
      return lhs != rhs?.Value;
    }

    public static bool operator !=(WeaklyQuotedString lhs, string rhs)
    {
      return lhs?.Value != rhs;
    }

    public override string ToString()
    {
      return Value;
    }
  }
}