using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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
        if (item is SubCommands subCommandsValue)
        {
          result.Append('(');
          result.Append(subCommandsValue.ToCode());
          result.Append(')');
        }
        else
        {
          result.Append(item);
        }
      }
      return result.ToString().Replace("\n", "\\n");
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
      return Helpers.GetBestType(DequeueObject(evaluate));
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
        // result.Add(Helpers.GetBestType(Evaluate(token, evaluate)));
        result.Add(Evaluate(token, evaluate));
      }
      return result;
    }

    private static object Evaluate(object value, bool evaluate)
    {
      return evaluate ? Run2.Evaluate(value) : value;
    }

    private new object Dequeue()
    {
      return base.Dequeue();
    }
  }
}