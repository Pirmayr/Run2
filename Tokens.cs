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
      return Process(Dequeue(), evaluate);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public string ToCode(int indent, bool newLine)
    {
      var result = DoToCode(indent, false);
      return !newLine && Globals.MaxCodeLineLength < result.Length ? DoToCode(indent, true) : result;
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

    private string DoToCode(int indent, bool newLine)
    {
      var multilineSubCommandWritten = false;
      var result = new StringBuilder();
      foreach (var item in this)
      {
        switch (item)
        {
          case SubCommands subCommandsValue:
          {
            var subCommandsCode = subCommandsValue.ToCode(indent + 2, newLine);
            if (Globals.MaxCodeLineLength < subCommandsCode.Length || subCommandsCode.Contains('\n'))
            {
              result.AppendNewLine(newLine);
              result.AppendIndented("(", indent, newLine);
              result.AppendNewLine(newLine);
              result.Append($"{subCommandsCode}");
              result.AppendNewLine(newLine);
              result.AppendIndented(")", indent, newLine);
              multilineSubCommandWritten = true;
            }
            else
            {
              result.AppendNewLine(multilineSubCommandWritten && newLine);
              result.AppendIndented("( ", indent, multilineSubCommandWritten && newLine);
              result.Append($"{subCommandsCode.Trim(' ')}");
              result.Append(" )");
            }
            break;
          }
          case WeaklyQuotedString:
          {
            result.AppendNewLine(multilineSubCommandWritten && newLine);
            result.AppendIndented($"'{item.ToString()?.Replace("\n", "~n")}'", indent, multilineSubCommandWritten && newLine);
            multilineSubCommandWritten = false;
            break;
          }
          default:
            result.AppendNewLine(multilineSubCommandWritten && newLine);
            string itemString;
            switch (item)
            {
              case double doubleValue:
                itemString = doubleValue.ToString("0.0############################");
                break;
              default:
                itemString = item.ToString();
                break;
            }
            result.AppendIndented($"{itemString}", indent, multilineSubCommandWritten && newLine);
            multilineSubCommandWritten = false;
            break;
        }
      }
      return result.ToString();
    }
  }
}