using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Run2
{
  [SuppressMessage("ReSharper", "MemberCanBeInternal")]
  public sealed class Tokens : Queue<object>
  {
    internal Tokens()
    {
    }

    internal Tokens(IEnumerable<object> values) : base(values)
    {
    }

    private Tokens(Tokens tokens) : base(tokens)
    {
    }

    public object DequeueObject(bool evaluate = true)
    {
      return Process(Dequeue(), evaluate);
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

    internal bool DequeueBool(bool evaluate = true)
    {
      var result = DequeueObject(evaluate);
      if (Helpers.IsAnyString(result, out var stringValue))
      {
        return bool.Parse(stringValue);
      }
      return (bool) result;
    }

    internal dynamic DequeueDynamic(bool evaluate = true)
    {
      return DequeueObject(evaluate);
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
        result.Add(Process(token, evaluate));
      }
      return result;
    }

    private static object Process(object value, bool evaluate)
    {
      return evaluate ? Run2.Evaluate(value) : value;
    }

    private new object Dequeue()
    {
      return base.Dequeue();
    }
  }
}