using System.Collections.Generic;
using System.Globalization;

namespace Run2
{
  internal sealed class Tokens : Queue<object>
  {
    public Tokens()
    {
    }

    public Tokens(IEnumerable<string> values) : base(values)
    {
    }

    private Tokens(Tokens tokens) : base(tokens)
    {
    }

    public Tokens Clone()
    {
      return new Tokens(this);
    }

    public object DequeueBestType(bool evaluate = true)
    {
      return BestType(Dequeue(evaluate));
    }

    public bool DequeueBool(bool evaluate = true)
    {
      var result = Dequeue(evaluate);
      if (result is string stringValue)
      {
        return bool.Parse(stringValue);
      }
      return (bool) result;
    }

    public dynamic DequeueDynamic(bool evaluate = true)
    {
      return DequeueBestType(evaluate);
    }

    public string DequeueString(bool evaluate = true)
    {
      return Dequeue(evaluate).ToString();
    }

    public string PeekString()
    {
      return Peek().ToString();
    }

    public List<object> ToList(bool evaluate)
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

    private object Dequeue(bool evaluate)
    {
      return Evaluate(Dequeue(), evaluate);
    }
  }
}