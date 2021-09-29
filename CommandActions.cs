using System.Diagnostics.CodeAnalysis;

namespace Run2
{
  [SuppressMessage("ReSharper", "UnusedType.Global")]
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  internal static class CommandActions
  {
    [CommandAction(2, 2, "+", "adds two values", "value1", "first value", "value2", "second value")]
    public static object Add(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 + value2;
    }

    [CommandAction(2, 2, null, "returns the element of an array, a list, or a string at the specified index", "object", "array, list, or string", "index", "index of the element")]
    public static object At(Tokens arguments)
    {
      var array = arguments.DequeueDynamic();
      var index = arguments.DequeueDynamic();
      return array[index];
    }

    [CommandAction(2, 2, "/", "divides first number by second number", "a", "first number", "b", "second number")]
    public static object Divide(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 / value2;
    }

    [CommandAction(2, 2, "==", "tests two values for equality", "value1", "first value", "value2", "second value")]
    public static object Equal(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return Helpers.IsEqual(value1, value2);
    }

    [CommandAction(1, int.MaxValue, null, "evaluates an object", "object", "object to be evaluated")]
    public static object Evaluate(Tokens arguments)
    {
      var value = arguments.DequeueObject();
      var result = Run2.Evaluate(value);
      return result;
    }

    [CommandAction(5, 5, null, "performs a for-loop", "name", "name of the variable which holds the counter", "from", "start value of the counter", "to", "end value of the counter", "step", "increment for the counter", "code", "body of the for-loop")]
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

    [CommandAction(3, 3, null, "performs a foreach-loop", "name", "name of the variable which holds the current iteration-value", "values", "values which are to be iterated through", "code", "body of the foreach-loop")]
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

    [CommandAction(1, 1, null, "return the value of a variable", "name", "name of the variable")]
    public static object Get(Tokens arguments)
    {
      var variableName = arguments.DequeueString();
      return Run2.GetVariable(variableName);
    }

    [CommandAction(0, 0, null, "returns help-information (formatted as markdown)")]
    // ReSharper disable once UnusedParameter.Global
    public static object GetHelp(Tokens arguments)
    {
      return Run2.GetHelp();
    }

    [CommandAction(2, 2, null, "creates or sets a global variable", "name", "name of the variable", "value", "value which should be assigned to the variable")]
    public static object Global(Tokens arguments)
    {
      var variableName = arguments.DequeueString(false);
      var variableValue = arguments.DequeueBestType();
      Run2.SetGlobalVariable(variableName, variableValue);
      return variableValue;
    }

    [CommandAction(2, 2, ">", "tests if value1 is greater than value2", "value1", "first value", "value2", "second value")]
    public static object Greater(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 > value2;
    }

    [CommandAction(2, 2, ">=", "tests if value1 is greater than or equal to value2", "value1", "first value", "value2", "second value")]
    public static object GreaterOrEqual(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 >= value2;
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

    [CommandAction(2, int.MaxValue)]
    public static object InvokeInstanceMember(Tokens arguments)
    {
      var memberName = arguments.DequeueString();
      var target = arguments.DequeueObject();
      return Helpers.InvokeMember(memberName, target.GetType(), target, arguments.ToList(true).ToArray());
    }

    [CommandAction(2, 2, "<", "tests if value1 is less than value2", "value1", "first value", "value2", "second value")]
    public static object Less(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 < value2;
    }

    [CommandAction(2, 2, "<=", "tests if value1 is less than or equal to value2", "value1", "first value", "value2", "second value")]
    public static object LessOrEqual(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 <= value2;
    }

    [CommandAction(2, 2, null, "creates or sets a local variable", "name", "name of the variable", "value", "value which should be assigned to the variable")]
    public static object Local(Tokens arguments)
    {
      var variableName = arguments.DequeueString(false);
      var variableValue = arguments.DequeueBestType();
      Run2.SetLocalVariable(variableName, variableValue);
      return variableValue;
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

    [CommandAction(2, 2, "*", "multiplies two numbers", "a", "first number", "b", "second number")]
    public static object Multiply(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 * value2;
    }

    [CommandAction(2, 2, "!=", "tests two values for unequality", "value1", "first value", "value2", "second value")]
    public static object NotEqual(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return !Helpers.IsEqual(value1, value2);
    }

    [CommandAction(1, 1)]
    public static object Return(Tokens arguments)
    {
      return arguments.DequeueBestType();
    }

    [CommandAction(1, int.MaxValue)]
    public static object Run(Tokens arguments)
    {
      var path = arguments.DequeueString();
      Helpers.Execute(path, string.Join(' ', arguments.ToList(true)), "c:\\", 3600000, 5, 0, 0, out var result, out _);
      return result;
    }

    [CommandAction(2, 2)]
    public static object Set(Tokens arguments)
    {
      var variableName = arguments.DequeueString();
      var variableValue = arguments.DequeueBestType();
      Run2.SetVariable(variableName, variableValue);
      return variableValue;
    }

    [CommandAction(2, 2, "-", "subtracts second number from first number", "a", "first number", "b", "second number")]
    public static object Subtract(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 - value2;
    }

    [CommandAction(2, int.MaxValue)]
    public static object Switch(Tokens arguments)
    {
      while (1 < arguments.Count)
      {
        var condition = arguments.DequeueBool();
        var code = arguments.DequeueBestType(false);
        if (condition)
        {
          return Run2.Evaluate(code);
        }
      }
      if (0 < arguments.Count)
      {
        var code = arguments.DequeueBestType(false);
        return Run2.Evaluate(code);
      }
      return null;
    }
  }
}