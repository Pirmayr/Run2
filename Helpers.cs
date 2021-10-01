using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Run2
{
  [SuppressMessage("ReSharper", "MemberCanBeInternal")]
  public static class Helpers
  {
    public static Type BaseTypeOfMember(Type type, string memberName, BindingFlags bindingFlags)
    {
      var result = type;
      while (result != null && result.Name != "ValueType" && result.BaseType != null && result.Name != result.BaseType.Name && 0 < result.BaseType.GetMember(memberName, bindingFlags).Length)
      {
        var baseType = type.BaseType;
        result = baseType;
      }
      return result;
    }

    public static void Check([DoesNotReturnIf(false)] this bool condition, string message)
    {
      Checked(condition, message);
    }

    public static void Execute(string executablePath, string arguments, string workingDirectory, int processTimeout, int tryCount, int minimalExitCode, int maximalExitCode, out string output, out string error)
    {
      var mostRecentException = new Exception("Execution failed");
      for (var i = 0; i < tryCount; ++i)
      {
        try
        {
          var process = new Process();
          process.StartInfo.FileName = executablePath;
          process.StartInfo.Arguments = arguments;
          process.StartInfo.WorkingDirectory = workingDirectory;
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.RedirectStandardOutput = true;
          process.StartInfo.RedirectStandardError = true;
          WriteLine($"Starting process {executablePath} ...");
          process.Start();
          output = process.StandardOutput.ReadToEnd().Trim();
          error = process.StandardError.ReadToEnd();
          process.WaitForExit(processTimeout).Check($"Process {executablePath} has timed out");
          WriteLine($"Process {executablePath} terminated");
          (minimalExitCode == process.ExitCode && process.ExitCode <= maximalExitCode).Check($"The exit-code {process.ExitCode} lies not between the allowed range of {minimalExitCode} to {maximalExitCode}");
          return;
        }
        catch (Exception exception)
        {
          mostRecentException = exception;
          HandleException(exception);
        }
        Thread.Sleep(5000);
      }
      throw mostRecentException;
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static string FindDirectory(string baseDirectory, string pattern)
    {
      Directory.Exists(baseDirectory).Check($"Base-directory '{baseDirectory} does not exist");
      var currentDirectory = baseDirectory;
      while (!string.IsNullOrEmpty(currentDirectory))
      {
        var currentPath = Directory.GetDirectories(currentDirectory, pattern, SearchOption.TopDirectoryOnly).OrderByDescending(static item => item).FirstOrDefault();
        if (!string.IsNullOrEmpty(currentPath) && Directory.Exists(currentPath))
        {
          return currentPath;
        }
        currentDirectory = Path.GetDirectoryName(currentDirectory);
      }
      return Directory.GetDirectories(baseDirectory, pattern, SearchOption.AllDirectories).OrderByDescending(static item => item).FirstOrDefault() ?? "";
    }

    public static string FindFile(string baseDirectory, string pattern)
    {
      Directory.Exists(baseDirectory).Check($"Base-directory '{baseDirectory} does not exist");
      var currentDirectory = baseDirectory;
      while (!string.IsNullOrEmpty(currentDirectory))
      {
        var currentPath = Directory.GetFiles(currentDirectory, pattern, SearchOption.TopDirectoryOnly).OrderByDescending(static item => item).FirstOrDefault();
        if (!string.IsNullOrEmpty(currentPath) && File.Exists(currentPath))
        {
          return currentPath;
        }
        currentDirectory = Path.GetDirectoryName(currentDirectory);
      }
      return Directory.GetFiles(baseDirectory, pattern, SearchOption.AllDirectories).OrderByDescending(static item => item).FirstOrDefault() ?? "";
    }

    public static object GetBestType(object value)
    {
      if (IsAnyString(value, out var stringResult))
      {
        if (bool.TryParse(stringResult, out var boolResult))
        {
          return boolResult;
        }
        if (int.TryParse(stringResult, out var intResult))
        {
          return intResult;
        }
        if (double.TryParse(stringResult, NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleResult))
        {
          return doubleResult;
        }
        return stringResult;
      }
      return value;
    }

    public static string GetCommandNameFromPath(string path)
    {
      return Path.GetFileNameWithoutExtension(path);
    }

    public static string GetProgramDirectory()
    {
      return Path.GetDirectoryName(AppContext.BaseDirectory);
    }

    public static void HandleException(Exception exception)
    {
      WriteLine(exception.InnerMostException().Message);
    }

    public static object Invoke(string memberName, object typeOrTarget, object[] arguments)
    {
      var isType = typeOrTarget is Type;
      var type = isType ? (Type) typeOrTarget : typeOrTarget.GetType();
      var target = isType ? null : typeOrTarget;
      return InvokeMember(memberName, type, target, arguments);
    }

    public static object InvokeMember(string memberName, Type type, object target, object[] arguments)
    {
      var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
      if (memberName == "_new")
      {
        bindingFlags |= BindingFlags.CreateInstance;
      }
      else
      {
        var member = type.GetMember(memberName, bindingFlags).FirstOrDefault();
        if (member != null)
        {
          switch (member)
          {
            case MethodInfo:
              bindingFlags |= BindingFlags.InvokeMethod;
              break;
            case PropertyInfo:
              bindingFlags |= arguments.Length == 0 ? BindingFlags.GetProperty : BindingFlags.SetProperty;
              break;
          }
        }
      }
      var result = type.InvokeMember(memberName, bindingFlags, null, target, arguments);
      return result;
    }

    public static bool IsAnyString(object value, out string stringResult)
    {
      switch (value)
      {
        case string stringValue:
          stringResult = stringValue;
          return true;
        case WeaklyQuotedString weaklyQuotedStringValue:
          stringResult = weaklyQuotedStringValue.Value;
          return true;
        default:
          stringResult = "";
          return false;
      }
    }

    public static object IsEqual(dynamic value1, dynamic value2)
    {
      if (value1 is not string && value2 is not string && value1 is IEnumerable enumerable1 && value2 is IEnumerable enumerable2)
      {
        var enumerator1 = enumerable1.GetEnumerator();
        var enumerator2 = enumerable2.GetEnumerator();
        var containsElement1 = enumerator1.MoveNext();
        var containsElement2 = enumerator2.MoveNext();
        while (containsElement1 && containsElement2)
        {
          dynamic current1 = enumerator1.Current;
          dynamic current2 = enumerator2.Current;
          if (current1 != current2)
          {
            return false;
          }
          containsElement1 = enumerator1.MoveNext();
          containsElement2 = enumerator2.MoveNext();
        }
        return !containsElement1 && !containsElement2;
      }
      return value1 == value2;
    }

    public static bool IsStruct(this Type type)
    {
      return type.IsValueType && /*!type.IsPrimitive &&*/ !type.IsEnum;
    }

    public static void WriteLine(string message)
    {
      Console.WriteLine(message);
    }

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
    private static bool Checked([DoesNotReturnIf(false)] this bool condition, string message)
    {
      if (!condition)
      {
        throw new Exception(message);
      }
      return true;
    }

    private static Exception InnerMostException(this Exception exception)
    {
      var result = exception;
      while (result.InnerException != null)
      {
        result = result.InnerException;
      }
      return result;
    }
  }
}