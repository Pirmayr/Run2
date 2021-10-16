using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Run2
{
  [SuppressMessage("ReSharper", "MemberCanBeInternal")]
  public sealed class Tokens : List
  {
    public new int Count => base.Count - DequeueIndex;

    public int DequeueIndex { get; set; }

    internal Tokens()
    {
    }

    internal Tokens(IEnumerable<object> values) : base(values)
    {
    }

    private Tokens(Tokens tokens) : base(tokens)
    {
    }

    public object Dequeue()
    {
      return this[DequeueIndex++];
    }

    public object DequeueObject(bool evaluate = true)
    {
      return Process(Dequeue(), evaluate);
    }

    public void Enqueue(object value, Properties properties = null)
    {
      Add(value);
      value.AddProperties(properties);
    }

    public object Peek()
    {
      return this[DequeueIndex];
    }

    public bool TryDequeue(out object result)
    {
      if (DequeueIndex < base.Count)
      {
        result = Dequeue();
        return true;
      }
      result = null;
      return false;
    }

    public bool TryPeek(out object result)
    {
      if (DequeueIndex < base.Count)
      {
        result = Peek();
        return true;
      }
      result = null;
      return false;
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
      if (result.IsAnyString(out var stringValue))
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

    internal List ToList(bool evaluate)
    {
      var result = new List();
      for (var i = DequeueIndex; i < base.Count; ++i)
      {
        result.Add(Process(this[i], evaluate));
      }
      return result;
    }

    private static object Process(object value, bool evaluate)
    {
      return evaluate ? Run2.Evaluate(value) : value;
    }
  }
}