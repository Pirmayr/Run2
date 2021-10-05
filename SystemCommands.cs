using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Run2
{
  [SuppressMessage("ReSharper", "UnusedType.Global")]
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  internal static class SystemCommands
  {
    [Documentation(2, 2, "+", "adds two values", "value1", "first value", "value2", "second value")]
    public static object Add(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 + value2;
    }

    [Documentation(2, 2, null, "returns the element of an array, a list, or a string at the specified index", "object", "array, list, or string", "index", "index of the element")]
    public static object At(Tokens arguments)
    {
      var array = arguments.DequeueDynamic();
      var index = arguments.DequeueDynamic();
      return array[index];
    }

    [Documentation(1, 1, null, "breaks the innermost loop and returns a value", "value", "the value to be returned")]
    public static object Break(Tokens arguments)
    {
      var result = arguments.DequeueObject();
      Globals.DoBreak = true;
      return result;
    }

    [Documentation(2, 2, "/", "divides first number by second number", "a", "first number", "b", "second number")]
    public static object Divide(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 / value2;
    }

    [Documentation(2, 2, "==", "tests two values for equality", "value1", "first value", "value2", "second value")]
    public static object Equal(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return Helpers.IsEqual(value1, value2);
    }

    [Documentation(1, int.MaxValue, null, "evaluates an object", "object", "object to be evaluated")]
    public static object Evaluate(Tokens arguments)
    {
      var value = arguments.DequeueObject();
      var result = Run2.Evaluate(value);
      return result;
    }

    [Documentation(1, int.MaxValue, null, "evaluates an array or a list", "values", "values to be evaluated")]
    public static object EvaluateValues(Tokens arguments)
    {
      var result = new List<object>();
      var values = arguments.DequeueObject() as IEnumerable;
      (values != null).Check("Values must be of type 'IEnumerable'");
      foreach (var value in values)
      {
        result.Add(Run2.Evaluate(value));
      }
      return result;
    }

    [Documentation(5, 5, null, "performs a for-loop", "name", "name of the variable which holds the counter", "from", "start value of the counter", "to", "end value of the counter", "step", "increment for the counter", "code", "body of the for-loop")]
    public static object For(Tokens arguments)
    {
      object result = null;
      var variableName = arguments.DequeueString(false);
      var from = arguments.DequeueDynamic();
      var to = arguments.DequeueDynamic();
      var step = arguments.DequeueDynamic();
      var code = arguments.DequeueObject(false);
      for (var i = from; i <= to; i += step)
      {
        Run2.SetLocalVariable(variableName, i);
        result = Run2.Evaluate(code);
        if (Globals.DoBreak)
        {
          Globals.DoBreak = false;
          break;
        }
      }
      return result;
    }

    [Documentation(3, 3, null, "performs a foreach-loop", "name", "name of the variable which holds the current iteration-value", "values", "values which are to be iterated through", "code", "body of the foreach-loop")]
    public static object ForEach(Tokens arguments)
    {
      object result = null;
      var variableName = arguments.DequeueString(false);
      var values = arguments.DequeueDynamic();
      var code = arguments.DequeueObject(false);
      foreach (var value in values)
      {
        Run2.SetLocalVariable(variableName, value);
        result = Run2.Evaluate(code);
      }
      return result;
    }

    [Documentation(1, 1, null, "return the value of a variable; the variable can exist in any active scope", "name", "name of the variable")]
    public static object Get(Tokens arguments)
    {
      var variableName = arguments.DequeueString();
      return Run2.GetVariable(variableName);
    }

    [Documentation(0, 0, null, "returns the formatted script")]
    // ReSharper disable once UnusedParameter.Global
    public static object GetCode(Tokens arguments)
    {
      return Run2.ToCode();
    }

    [Documentation(0, 0, null, "returns the list of commands")]
    // ReSharper disable once UnusedParameter.Global
    public static object GetCommands(Tokens arguments)
    {
      return Run2.GetCommands();
    }

    [Documentation(0, 0, null, "returns help-information (formatted as markdown)")]
    // ReSharper disable once UnusedParameter.Global
    public static object GetHelp(Tokens arguments)
    {
      return Run2.GetHelp();
    }

    [Documentation(2, 2, null, "creates or sets a global variable", "name", "name of the variable", "value", "value to be assigned to the variable")]
    public static object Global(Tokens arguments)
    {
      var variableName = arguments.DequeueString(false);
      var variableValue = arguments.DequeueObject();
      Run2.SetGlobalVariable(variableName, variableValue);
      return variableValue;
    }

    [Documentation(2, 2, ">", "tests if value1 is greater than value2", "value1", "first value", "value2", "second value")]
    public static object Greater(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 > value2;
    }

    [Documentation(2, 2, ">=", "tests if value1 is greater than or equal to value2", "value1", "first value", "value2", "second value")]
    public static object GreaterOrEqual(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 >= value2;
    }

    [Documentation(2, 3, null, "performs the if-statement", "condition", "condition", "true-block", "command to be executed if the condition is 'true'", "false-block", "(optional) command to be executed if the condition is 'false'")]
    public static object If(Tokens arguments)
    {
      var condition = arguments.DequeueBool();
      var trueCase = arguments.DequeueObject(false);
      if (condition)
      {
        return Run2.Evaluate(trueCase);
      }
      return arguments.TryDequeue(out var falseCase) ? Run2.Evaluate(falseCase) : "";
    }

    [Documentation(2, int.MaxValue, null, "calls the 'Invoke'-method of the type of the specified object", "name", "name of the object-member", "object", "target of the invokation")]
    public static object InvokeInstanceMember(Tokens arguments)
    {
      var memberName = arguments.DequeueString();
      var target = arguments.DequeueObject();
      return Helpers.InvokeMember(memberName, target.GetType(), target, arguments.ToList(true).ToArray());
    }

    [Documentation(2, 2, "<", "tests if value1 is less than value2", "value1", "first value", "value2", "second value")]
    public static object Less(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 < value2;
    }

    [Documentation(2, 2, "<=", "tests if value1 is less than or equal to value2", "value1", "first value", "value2", "second value")]
    public static object LessOrEqual(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 <= value2;
    }

    [Documentation(2, 2, null, "creates or sets a local variable", "name", "name of the variable", "value", "value to be assigned to the variable")]
    public static object Local(Tokens arguments)
    {
      var variableName = arguments.DequeueString(false);
      var variableValue = arguments.DequeueObject();
      Run2.SetLocalVariable(variableName, variableValue);
      return variableValue;
    }

    [Documentation(2, 2, null, "executes a command with all elements of an array or listf; the variable 'item' holds the current element", "arrayOrList", "array or list", "command", "command")]
    public static object Map(Tokens arguments)
    {
      object result = null;
      var values = arguments.DequeueDynamic();
      var code = arguments.DequeueObject(false);
      foreach (var value in values)
      {
        Run2.SetLocalVariable("item", value);
        result = Run2.Evaluate(code);
      }
      return result;
    }

    [Documentation(2, 2, "%", "return the remainder when dividing the first number by the second number", "a", "first number", "b", "second number")]
    public static object Modulo(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 % value2;
    }

    [Documentation(2, 2, "*", "multiplies two numbers", "a", "first number", "b", "second number")]
    public static object Multiply(Tokens arguments)
    {
      var value1 = arguments.DequeueObject();
      var value2 = arguments.DequeueObject();
      if (BigInteger.TryParse(value1.ToString(), out var bigIntegerValue1) && BigInteger.TryParse(value2.ToString(), out var bigIntegerValue2))
      {
        return Helpers.GetBestTypedObject((bigIntegerValue1 * bigIntegerValue2).ToString());
      }
      return (dynamic) value1 * (dynamic) value2;
    }

    [Documentation(2, 2, "!=", "tests two values for unequality", "value1", "first value", "value2", "second value")]
    public static object NotEqual(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return !Helpers.IsEqual(value1, value2);
    }

    [Documentation(0, 0, null, "the value 'null'")]
    // ReSharper disable once UnusedParameter.Global
    public static object Null(Tokens arguments)
    {
      return null;
    }

    [Documentation(2, 3, null, "assigns a new value to the element of an array, a list, or a string at the specified index", "object", "array, list, or string", "index", "index of the element", "value", "value to be set")]
    public static object Put(Tokens arguments)
    {
      var array = arguments.DequeueDynamic();
      var index = arguments.DequeueDynamic();
      var value = arguments.DequeueDynamic();
      array[index] = value;
      return value;
    }

    [Documentation(1, 1, null, "returns a value", "value", "the value to be returned")]
    public static object Return(Tokens arguments)
    {
      return arguments.DequeueObject();
    }

    [Documentation(1, int.MaxValue, null, "runs an external program with the arguments given", "path", "path of the external program", "arguments", "the values ​​to be passed to the external program")]
    public static object Run(Tokens arguments)
    {
      var path = arguments.DequeueString();
      Helpers.Execute(path, string.Join(' ', arguments.ToList(true)), "c:\\", 3600000, 5, 0, 0, out var result, out _);
      return result;
    }

    [Documentation(2, 2, null, "assigns a new value to an existing variable; the variable can exist in any active scope", "value", "the value to be assigned to the variable")]
    public static object Set(Tokens arguments)
    {
      var variableName = arguments.DequeueString();
      var variableValue = arguments.DequeueObject();
      Run2.SetVariable(variableName, variableValue);
      return variableValue;
    }

    [Documentation(2, 2, "-", "subtracts second number from first number", "a", "first number", "b", "second number")]
    public static object Subtract(Tokens arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 - value2;
    }

    [Documentation(2, int.MaxValue, null, "calls the first command for which a condition holds true", "condition-command-pairs", "pairs consisting of condition and command; the first command whose condition is 'true' is executed")]
    public static object Switch(Tokens arguments)
    {
      while (1 < arguments.Count)
      {
        var condition = arguments.DequeueBool();
        var code = arguments.DequeueObject(false);
        if (condition)
        {
          return Run2.Evaluate(code);
        }
      }
      if (0 < arguments.Count)
      {
        var code = arguments.DequeueObject(false);
        return Run2.Evaluate(code);
      }
      return null;
    }

    [Documentation(2, 2, null, "performs a while-loop", "condition", "condition for continuing the loop", "code", "body of the while-loop")]
    public static object While(Tokens arguments)
    {
      object result = null;
      var condition = arguments.DequeueObject(false);
      var code = arguments.DequeueObject(false);
      var conditionValue = Run2.Evaluate(condition) is true;
      while (conditionValue)
      {
        result = Run2.Evaluate(code);
        conditionValue = Run2.Evaluate(condition) is true;
      }
      return result;
    }
  }
}