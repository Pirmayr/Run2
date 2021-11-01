using System.Collections.Generic;

namespace Run2
{
  public sealed class Items : Globals.List
  {
    public int DequeueIndex { get; set; }

    public int QueueCount => Count - DequeueIndex;

    public Items()
    {
    }

    public Items(object value)
    {
      Add(value);
    }

    public Items(IEnumerable<object> values) : base(values)
    {
    }

    public object Dequeue()
    {
      return this[DequeueIndex++];
    }

    public bool DequeueBool(bool evaluate = true)
    {
      var result = DequeueObject(evaluate);
      if (result is string stringValue)
      {
        return bool.Parse(stringValue);
      }
      return (bool) result;
    }

    public dynamic DequeueDynamic(bool evaluate = true)
    {
      return DequeueObject(evaluate);
    }

    public object DequeueObject(bool evaluate = true)
    {
      return Process(Dequeue(), evaluate);
    }

    public string DequeueString(bool evaluate = true)
    {
      return DequeueObject(evaluate).ToString();
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

    public string PeekString()
    {
      return Peek().ToString();
    }

    public Globals.List ToList(bool evaluate)
    {
      var result = new Globals.List();
      for (var i = DequeueIndex; i < Count; ++i)
      {
        result.Add(Process(this[i], evaluate));
      }
      return result;
    }

    public bool TryDequeue(out object result)
    {
      if (DequeueIndex < Count)
      {
        result = Dequeue();
        return true;
      }
      result = null;
      return false;
    }

    private static object Process(object value, bool evaluate)
    {
      return evaluate ? Run2.Evaluate(value) : value;
    }
  }
}