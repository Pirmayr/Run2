using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Numerics;

namespace Run2
{
  [SuppressMessage("ReSharper", "UnusedType.Global")]
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public static class SystemCommands
  {
    [Documentation(2, 2, "+", "adds two values", "value1", "first value", "value2", "second value")]
    public static object Add(Items arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 + value2;
    }

    [Documentation(2, int.MaxValue, null, "performs the 'and'-operation", "value1", "first value", "value2", "second value")]
    public static object And(Items arguments)
    {
      while (0 < arguments.QueueCount)
      {
        if (!arguments.DequeueBool())
        {
          return false;
        }
      }
      return true;
    }

    [Documentation(2, 2, null, "returns the element of an array, a list, or a string at the specified index", "object", "array, list, or string", "index", "index of the element")]
    public static object At(Items arguments)
    {
      var array = arguments.DequeueDynamic();
      var index = arguments.DequeueDynamic();
      return array[index];
    }

    [Documentation(1, 1, null, "breaks the innermost loop and returns a value", "value", "the value to be returned")]
    public static object Break(Items arguments)
    {
      var result = arguments.DequeueObject();
      Globals.DoBreak = true;
      return result;
    }

    [Documentation(2, 2, "/", "divides first number by second number", "a", "first number", "b", "second number")]
    public static object Divide(Items arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 / value2;
    }

    [Documentation(2, 2, "==", "tests two values for equality", "value1", "first value", "value2", "second value")]
    public static object Equal(Items arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return Helpers.AreEqual(value1, value2);
    }

    [Documentation(1, int.MaxValue, null, "evaluates an object", "object", "object to be evaluated")]
    public static object Evaluate(Items arguments)
    {
      var value = arguments.DequeueObject();
      var result = Run2.Evaluate(value);
      return result;
    }

    [Documentation(5, 5, null, "performs a for-loop", "name", "name of the variable which holds the counter", "from", "start value of the counter", "to", "end value of the counter", "step", "increment for the counter", "code", "body of the for-loop")]
    public static object For(Items arguments)
    {
      object result = null;
      var variableName = arguments.DequeueString(false);
      var from = arguments.DequeueDynamic();
      var to = arguments.DequeueDynamic();
      var step = arguments.DequeueDynamic();
      var code = arguments.DequeueObject(false);
      for (var i = from; i <= to; i += step)
      {
        Globals.Variables.SetLocal(variableName, (object) i);
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
    public static object ForEach(Items arguments)
    {
      object result = null;
      var variableName = arguments.DequeueString(false);
      var values = arguments.DequeueDynamic();
      var code = arguments.DequeueObject(false);
      foreach (var value in values)
      {
        Globals.Variables.SetLocal(variableName, (object) value);
        result = Run2.Evaluate(code);
      }
      return result;
    }

    [Documentation(1, 1, null, "return the value of a variable; the variable can exist in any active scope", "name", "name of the variable")]
    public static object Get(Items arguments)
    {
      var variableName = arguments.DequeueString();
      return Globals.Variables.Get(variableName);
    }

    [Documentation(1, 1, null, "returns the formatted script")]
    // ReSharper disable once UnusedParameter.Global
    public static object GetCode(Items arguments)
    {
      var filter = arguments.DequeueString();
      return CodeFormatter.GetCode(filter);
    }

    [Documentation(0, 0, null, "returns the list of commands")]
    // ReSharper disable once UnusedParameter.Global
    public static object GetCommands(Items arguments)
    {
      return string.Join('\n', Globals.Commands.Keys);
    }

    [Documentation(0, 0, null, "returns help-information (formatted as markdown)")]
    // ReSharper disable once UnusedParameter.Global
    public static object GetHelp(Items arguments)
    {
      return HelpGenerator.GetHelp();
    }

    [Documentation(2, int.MaxValue, null, "creates or sets a global variable", "name", "name of the variable", "value", "value to be assigned to the variable")]
    public static object Global(Items arguments)
    {
      var evaluate = false;
      if (arguments.Peek() is true)
      {
        evaluate = true;
        arguments.Dequeue();
      }
      (arguments.QueueCount % 2 == 0).Check("The number of arguments must be even");
      object result = null;
      while (0 < arguments.QueueCount)
      {
        var variableName = arguments.DequeueString(evaluate);
        result = arguments.DequeueObject();
        Globals.Variables.SetGlobal(variableName, result);
      }
      return result;
    }

    [Documentation(2, 2, ">", "tests if value1 is greater than value2", "value1", "first value", "value2", "second value")]
    public static object Greater(Items arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 > value2;
    }

    [Documentation(2, 2, ">=", "tests if value1 is greater than or equal to value2", "value1", "first value", "value2", "second value")]
    public static object GreaterOrEqual(Items arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 >= value2;
    }

    [Documentation(2, 3, null, "performs the if-statement", "condition", "condition", "true-block", "command to be executed if the condition is 'true'", "false-block", "(optional) command to be executed if the condition is 'false'")]
    public static object If(Items arguments)
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
    public static object InvokeInstanceMember(Items arguments)
    {
      var memberName = arguments.DequeueString();
      var target = arguments.DequeueObject();
      return Helpers.InvokeMember(memberName, target.GetType(), target, arguments.ToList(true).ToArray());
    }

    [Documentation(2, 2, "<", "tests if value1 is less than value2", "value1", "first value", "value2", "second value")]
    public static object Less(Items arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 < value2;
    }

    [Documentation(2, 2, "<=", "tests if value1 is less than or equal to value2", "value1", "first value", "value2", "second value")]
    public static object LessOrEqual(Items arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 <= value2;
    }

    [Documentation(2, int.MaxValue, null, "creates local variables", "name", "name of the variable", "value", "value to be assigned to the variable")]
    public static object Local(Items arguments)
    {
      var evaluate = false;
      if (arguments.Peek() is true)
      {
        evaluate = true;
        arguments.Dequeue();
      }
      (arguments.QueueCount % 2 == 0).Check("The number of arguments must be even");
      object result = null;
      while (0 < arguments.QueueCount)
      {
        var variableName = arguments.DequeueString(evaluate);
        result = arguments.DequeueObject();
        Globals.Variables.SetLocal(variableName, result);
      }
      return result;
    }

    [Documentation(2, 2, null, "executes a command with all elements of an array or list; the variable 'item' holds the current element", "arrayOrList", "array or list", "command", "command")]
    public static object Map(Items arguments)
    {
      object result = null;
      var values = arguments.DequeueDynamic();
      var code = arguments.DequeueObject(false);
      foreach (var value in values)
      {
        Globals.Variables.SetLocal("item", (object) value);
        result = Run2.Evaluate(code);
      }
      return result;
    }

    [Documentation(2, 2, "%", "return the remainder when dividing the first number by the second number", "a", "first number", "b", "second number")]
    public static object Modulo(Items arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 % value2;
    }

    [Documentation(2, 2, "*", "multiplies two numbers", "a", "first number", "b", "second number")]
    public static object Multiply(Items arguments)
    {
      var value1 = arguments.DequeueObject();
      var value2 = arguments.DequeueObject();
      if (BigInteger.TryParse(value1.ToString(), out var bigIntegerValue1) && BigInteger.TryParse(value2.ToString(), out var bigIntegerValue2))
      {
        return (bigIntegerValue1 * bigIntegerValue2).ToString().ToBestType();
      }
      return (dynamic) value1 * (dynamic) value2;
    }

    [Documentation(2, 2, "!=", "tests two values for unequality", "value1", "first value", "value2", "second value")]
    public static object NotEqual(Items arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return !Helpers.AreEqual(value1, value2);
    }

    [Documentation(0, 0, null, "the value 'null'")]
    // ReSharper disable once UnusedParameter.Global
    public static object Null(Items arguments)
    {
      return null;
    }

    [Documentation(2, int.MaxValue, null, "performs the 'or'-operation", "value1", "first value", "value2", "second value")]
    public static object Or(Items arguments)
    {
      while (0 < arguments.QueueCount)
      {
        if (arguments.DequeueBool())
        {
          return true;
        }
      }
      return false;
    }

    [Documentation(2, 3, null, "assigns a new value to the element of an array, a list, or a string at the specified index", "object", "array, list, or string", "index", "index of the element", "value", "value to be set")]
    public static object Put(Items arguments)
    {
      var array = arguments.DequeueDynamic();
      var index = arguments.DequeueDynamic();
      var value = arguments.DequeueDynamic();
      array[index] = value;
      return value;
    }

    [Documentation(1, 1, null, "returns a value", "value", "the value to be returned")]
    public static object Return(Items arguments)
    {
      return arguments.DequeueObject();
    }

    [Documentation(1, int.MaxValue, null, "runs an external program with the arguments given", "path", "path of the external program", "arguments", "the values ​​to be passed to the external program")]
    public static object Run(Items arguments)
    {
      var commandOrpathOrDirectory = arguments.DequeueString();
      if (Globals.Commands.ContainsKey(commandOrpathOrDirectory))
      {
        return Run2.ExecuteCommand(commandOrpathOrDirectory, arguments);
      }
      string workingDirectory;
      string executablePath;
      if (Directory.Exists(commandOrpathOrDirectory))
      {
        workingDirectory = commandOrpathOrDirectory;
        executablePath = arguments.DequeueString();
      }
      else
      {
        workingDirectory = Path.GetDirectoryName(commandOrpathOrDirectory);
        if (string.IsNullOrEmpty(workingDirectory) || !Directory.Exists(workingDirectory))
        {
          workingDirectory = Globals.ProgramDirectory;
        }
        executablePath = commandOrpathOrDirectory;
      }
      Helpers.Execute(executablePath, string.Join(' ', arguments.ToList(true)), workingDirectory, 3600000, 5, 0, 0, out var result, out _);
      return result;
    }

    [Documentation(2, int.MaxValue, null, "assigns a new value to an existing variable; the variable can exist in any active scope", "value", "the value to be assigned to the variable")]
    public static object Set(Items arguments)
    {
      var evaluate = false;
      if (arguments.Peek() is true)
      {
        evaluate = true;
        arguments.Dequeue();
      }
      (arguments.QueueCount % 2 == 0).Check("The number of arguments must be even");
      object result = null;
      while (0 < arguments.QueueCount)
      {
        var variableName = arguments.DequeueString(evaluate);
        result = arguments.DequeueObject();
        Globals.Variables.Set(variableName, result);
      }
      return result;
    }

    [Documentation(2, 2, "-", "subtracts second number from first number", "a", "first number", "b", "second number")]
    public static object Subtract(Items arguments)
    {
      var value1 = arguments.DequeueDynamic();
      var value2 = arguments.DequeueDynamic();
      return value1 - value2;
    }

    [Documentation(2, int.MaxValue, null, "calls the first command for which a condition holds true", "condition-command-pairs", "pairs consisting of condition and command; the first command whose condition is 'true' is executed")]
    public static object Switch(Items arguments)
    {
      while (1 < arguments.QueueCount)
      {
        var condition = arguments.DequeueBool();
        var code = arguments.DequeueObject(false);
        if (condition)
        {
          return Run2.Evaluate(code);
        }
      }
      if (0 < arguments.QueueCount)
      {
        var code = arguments.DequeueObject(false);
        return Run2.Evaluate(code);
      }
      return null;
    }

    [Documentation(1, 1, null, "throws an exception", "message", "message")]
    public static object Throw(Items arguments)
    {
      var message = arguments.DequeueString();
      throw new Exception(message);
    }

    [Documentation(2, 2, null, "performs a while-loop", "condition", "condition for continuing the loop", "code", "body of the while-loop")]
    public static object While(Items arguments)
    {
      object result = null;
      var condition = arguments.DequeueObject(false);
      var code = arguments.DequeueObject(false);
      var conditionValue = Run2.Evaluate(condition) is true;
      while (conditionValue)
      {
        result = Run2.Evaluate(code);
        if (Globals.DoBreak)
        {
          Globals.DoBreak = false;
          break;
        }
        conditionValue = Run2.Evaluate(condition) is true;
      }
      return result;
    }
  }
}