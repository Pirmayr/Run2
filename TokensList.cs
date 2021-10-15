using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Run2
{
  [SuppressMessage("ReSharper", "MemberCanBeInternal")]
  public sealed class TokensList : List
  {
    private int dequeueIndex;

    public new int Count => base.Count - dequeueIndex;

    public int LineNumber { get; set; } = -1;

    internal TokensList()
    {
    }

    internal TokensList(IEnumerable<object> values) : base(values)
    {
    }

    private TokensList(TokensList tokens) : base(tokens)
    {
    }

    public object Dequeue()
    {
      return this[dequeueIndex++];
    }

    public object DequeueObject(bool evaluate = true)
    {
      return Process(Dequeue(), evaluate);
    }

    public void Enqueue(object value)
    {
      Add(value);
    }

    public void MoveFirst()
    {
      dequeueIndex = 0;
    }

    public object Peek()
    {
      return this[dequeueIndex];
    }

    public bool TryDequeue(out object result)
    {
      if (dequeueIndex < base.Count)
      {
        result = Dequeue();
        return true;
      }
      result = null;
      return false;
    }

    public bool TryPeek(out object result)
    {
      if (dequeueIndex < base.Count)
      {
        result = Peek();
        return true;
      }
      result = null;
      return false;
    }

    internal TokensList Clone(int skip = 0)
    {
      var result = new TokensList(this);
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
      for (var i = dequeueIndex; i < base.Count; ++i)
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