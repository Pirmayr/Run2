namespace Run2
{
  internal sealed class WeakQuotedString : object
  {
    public string Value { get; init; }

    public static bool operator ==(string lhs, WeakQuotedString rhs)
    {
      return lhs == rhs?.Value;
    }

    public static bool operator ==(WeakQuotedString lhs, string rhs)
    {
      return lhs?.Value == rhs;
    }

    public static bool operator !=(string lhs, WeakQuotedString rhs)
    {
      return lhs != rhs?.Value;
    }

    public static bool operator !=(WeakQuotedString lhs, string rhs)
    {
      return lhs?.Value != rhs;
    }

    public override string ToString()
    {
      return Value;
    }
  }
}