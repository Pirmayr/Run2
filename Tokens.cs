using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Run2
{
  [SuppressMessage("ReSharper", "MemberCanBeInternal")]
  public sealed class Tokens : Queue<object>
  {
    internal Tokens()
    {
    }

    internal Tokens(IEnumerable<string> values) : base(values)
    {
    }

    private Tokens(Tokens tokens) : base(tokens)
    {
    }

    public object DequeueObject(bool evaluate = true)
    {
      return Evaluate(Dequeue(), evaluate);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public string ToCode()
    {
      var result = new StringBuilder();
      foreach (var item in this)
      {
        if (0 < result.Length)
        {
          result.Append(' ');
        }
        result.Append(item);
      }
      return result.ToString();
    }

    internal Tokens Clone(int skip = 0)
    {
      var result = new Tokens(this);
      while (0 < skip)
      {
        result.Dequeue();
        --skip;
      }
      return result;
    }

    internal object DequeueBestType(bool evaluate = true)
    {
      return BestType(DequeueObject(evaluate));
    }

    internal bool DequeueBool(bool evaluate = true)
    {
      var result = DequeueObject(evaluate);
      if (result is string stringValue)
      {
        return bool.Parse(stringValue);
      }
      return (bool) result;
    }

    internal dynamic DequeueDynamic(bool evaluate = true)
    {
      return DequeueBestType(evaluate);
    }

    internal string DequeueString(bool evaluate = true)
    {
      return DequeueObject(evaluate).ToString();
    }

    internal string PeekString()
    {
      return Peek().ToString();
    }

    internal List<object> ToList(bool evaluate)
    {
      var result = new List<object>();
      foreach (var token in this)
      {
        result.Add(Evaluate(token, evaluate));
      }
      return result;
    }

    private static object BestType(object value)
    {
      if (value is string stringResult)
      {
        if (bool.TryParse(stringResult, out var boolResult))
        {
          return boolResult;
        }
        if (int.TryParse(stringResult, out var intResult))
        {
          return intResult;
        }
        if (double.TryParse(stringResult, NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleResult))
        {
          return doubleResult;
        }
        return stringResult;
      }
      return value;
    }

    private static object Evaluate(object value, bool evaluate)
    {
      return evaluate ? Run2.Evaluate(value) : value;
    }
  }
}