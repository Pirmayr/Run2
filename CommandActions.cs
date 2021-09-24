using System.Diagnostics.CodeAnalysis;

namespace Run2
{
  [SuppressMessage("ReSharper", "UnusedType.Global")]
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  internal static class CommandActions
  {
    [CommandAction(2, 2, "+")]
    public static object Add(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 + value2;
    }

    [CommandAction(2, 2)]
    public static object At(Tokens arguments)
    {
      var array = arguments.DequeueDynamic();
      var index = arguments.DequeueDynamic();
      return array[index];
    }

    [CommandAction(2, 2, "/")]
    public static object Divide(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 / value2;
    }

    [CommandAction(2, 2, "==")]
    public static object Equal(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 == value2;
    }

    [CommandAction(2, 2, "<")]
    public static object Less(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 < value2;
    }

    [CommandAction(2, 2, "<=")]
    public static object LessOrEqual(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 <= value2;
    }

    [CommandAction(2, 2, ">")]
    public static object Greater(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 > value2;
    }

    [CommandAction(2, 2, ">=")]
    public static object GreaterOrEqual(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 >= value2;
    }

    [CommandAction(2, 2, "!=")]
    public static object NotEqual(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 != value2;
    }

    public static object Evaluate(Tokens arguments)
    {
      var tokens = arguments.DequeueObject() as Tokens;
      var result = Run2.Evaluate(tokens);
      return result;
    }

    [CommandAction(5, 5)]
    public static object For(Tokens arguments)
    {
      object result = null;
      var variableName = arguments.DequeueString();
      var from = arguments.DequeueDynamic();
      var to = arguments.DequeueDynamic();
      var step = arguments.DequeueDynamic();
      var code = arguments.DequeueBestType(false);
      for (var i = from; i <= to; i += step)
      {
        Run2.SetLocalVariable(variableName, i);
        result = Run2.Evaluate(code);
      }
      return result;
    }

    [CommandAction(3, 3)]
    public static object ForEach(Tokens arguments)
    {
      object result = null;
      var variableName = arguments.DequeueString();
      var values = arguments.DequeueDynamic();
      var code = arguments.DequeueBestType(false);
      foreach (var value in values)
      {
        Run2.SetLocalVariable(variableName, value);
        result = Run2.Evaluate(code);
      }
      return result;
    }

    [CommandAction(2, 3)]
    public static object If(Tokens arguments)
    {
      var condition = arguments.DequeueBool();
      var trueCase = arguments.DequeueBestType(false);
      if (condition)
      {
        return Run2.Evaluate(trueCase);
      }
      return arguments.TryDequeue(out var falseCase) ? Run2.Evaluate(falseCase) : "";
    }

    [CommandAction(2, 2)]
    public static object Map(Tokens arguments)
    {
      object result = null;
      var values = arguments.DequeueDynamic();
      var code = arguments.DequeueBestType(false);
      foreach (var value in values)
      {
        Run2.SetLocalVariable("item", value);
        result = Run2.Evaluate(code);
      }
      return result;
    }

    [CommandAction(2, 2, "*")]
    public static object Multiply(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 * value2;
    }

    [CommandAction(1, 1)]
    public static object Return(Tokens arguments)
    {
      return arguments.DequeueBestType();
    }

    [CommandAction(1, 999)]
    public static object Run(Tokens arguments)
    {
      var path = arguments.DequeueString();
      Helpers.Execute(path, string.Join(' ', arguments.ToList(true)), "c:\\", 3600000, 5, 0, 0, out var result, out _);
      return result;
    }

    [CommandAction(2, 2)]
    public static object Set(Tokens arguments)
    {
      var variableName = arguments.DequeueString(false);
      var variableValue = arguments.DequeueBestType();
      Run2.SetGlobalVariable(variableName, variableValue);
      return variableValue;
    }

    [CommandAction(2, 2)]
    public static object SetLocal(Tokens arguments)
    {
      var variableName = arguments.DequeueString(false);
      var variableValue = arguments.DequeueBestType();
      Run2.SetLocalVariable(variableName, variableValue);
      return variableValue;
    }

    [CommandAction(2, 2, "-")]
    public static object Subtract(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 - value2;
    }
  }
}