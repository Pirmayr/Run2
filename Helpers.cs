﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Run2
{
  [SuppressMessage("ReSharper", "MemberCanBeInternal")]
  public static class Helpers
  {
    public static void AppendIndented(this StringBuilder builder, dynamic value, int indent, bool newLineMode)
    {
      builder.Append(newLineMode ? $"{new string(' ', indent)}{value}" : builder.EndsWith(' ') ? $"{value}" : $" {value}");
    }

    public static void AppendNewLine(this StringBuilder builder, bool newLine)
    {
      builder.Append(newLine ? "\n" : " ");
    }

    public static object AreEqual(dynamic value1, dynamic value2)
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

    public static Type BaseTypeOfMember(this Type type, string memberName, BindingFlags bindingFlags)
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

    // ReSharper disable once UnusedMember.Global
    public static void CopyFiles(string sourceDirectory, string pattern, string destinationDirectory, string destinationFilename, bool expand, string lineAction)
    {
      foreach (var sourcePath in Directory.GetFiles(sourceDirectory, pattern, SearchOption.AllDirectories))
      {
        var targetPath = destinationDirectory + "\\" + (string.IsNullOrEmpty(destinationFilename) ? Path.GetFileName(sourcePath) : destinationFilename);
        ExpandPragmas(sourcePath, targetPath, expand, lineAction);
      }
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
          WriteLine($"Starting process '{executablePath}':");
          WriteLine($"  Working-directory '{workingDirectory}'");
          WriteLine($"  Arguments '{arguments}' ...");
          process.Start();
          output = process.StandardOutput.ReadToEnd().Trim();
          error = process.StandardError.ReadToEnd();
          process.WaitForExit(processTimeout).Check($"Process '{executablePath}' has timed out");
          WriteLine($"Process '{executablePath}' terminated");
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

    public static object GetBestTypedObject(this string currentToken)
    {
      object bestTypedObject;
      if (currentToken.IsAnyString(out var stringResult))
      {
        if (bool.TryParse(stringResult, out var boolResult))
        {
          bestTypedObject = boolResult;
        }
        else if (int.TryParse(stringResult, out var intResult))
        {
          bestTypedObject = intResult;
        }
        else if (BigInteger.TryParse(stringResult, out var bigIntegerResult))
        {
          bestTypedObject = bigIntegerResult;
        }
        else if (double.TryParse(stringResult, NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleResult))
        {
          bestTypedObject = doubleResult;
        }
        else
        {
          bestTypedObject = stringResult;
        }
      }
      else
      {
        bestTypedObject = currentToken;
      }
      return bestTypedObject;
    }

    public static string GetCommandNameFromPath(string path)
    {
      return Path.GetFileNameWithoutExtension(path);
    }

    public static string GetProgramDirectory()
    {
      return Path.GetDirectoryName(AppContext.BaseDirectory);
    }

    public static void HandleException(Exception exception, int lineNumber = -1)
    {
      if (exception is not RuntimeException)
      {
        WriteLine(0 < lineNumber ? $"Exception around line {lineNumber}:" : "Exception:");
        WriteLine(exception.InnerMostException().Message);
      }
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
            case FieldInfo:
              bindingFlags |= arguments.Length == 0 ? BindingFlags.GetField : BindingFlags.SetField;
              break;
            case PropertyInfo:
              bindingFlags |= arguments.Length == 0 ? BindingFlags.GetProperty : BindingFlags.SetProperty;
              break;
            case MethodInfo:
              bindingFlags |= BindingFlags.InvokeMethod;
              break;
          }
        }
      }
      var result = type.InvokeMember(memberName, bindingFlags, null, target, arguments);
      return result;
    }

    public static bool IsAnyString(this object value, out string stringResult)
    {
      switch (value)
      {
        case string stringValue:
          stringResult = stringValue;
          return true;
        default:
          stringResult = "";
          return false;
      }
    }

    public static bool IsBlock(ref string text)
    {
      if (text.StartsWith(Globals.BlockStart) && text.EndsWith(Globals.BlockEnd))
      {
        text = text.Substring(1, text.Length - 2);
        return true;
      }
      return false;
    }

    public static bool IsStrongQuote(this object value, out string result)
    {
      if (value is string stringValue && stringValue.StartsWith(Globals.StrongQuote) && stringValue.EndsWith(Globals.StrongQuote))
      {
        result = stringValue.Substring(1, stringValue.Length - 2);
        return true;
      }
      result = null;
      return false;
    }

    public static bool IsStruct(this Type type)
    {
      return type.IsValueType && !type.IsEnum;
    }

    public static bool IsWeakQuote(ref string text)
    {
      if (text.StartsWith(Globals.WeakQuote) && text.EndsWith(Globals.WeakQuote))
      {
        text = text.Substring(1, text.Length - 2);
        return true;
      }
      return false;
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once ReturnTypeCanBeEnumerable.Global
    public static string[] LocateDirectories(string baseDirectory, string pattern)
    {
      Directory.Exists(baseDirectory).Check($"Base-directory '{baseDirectory} does not exist");
      string searchDirectory;
      var filename = Path.GetFileName(pattern);
      var currentDirectory = baseDirectory;
      while (!string.IsNullOrEmpty(currentDirectory))
      {
        searchDirectory = GetDirectory(currentDirectory, pattern);
        if (Directory.Exists(searchDirectory))
        {
          var currentPath = Directory.GetDirectories(searchDirectory, filename, SearchOption.TopDirectoryOnly).OrderByDescending(static item => item).FirstOrDefault();
          if (!string.IsNullOrEmpty(currentPath) && Directory.Exists(currentPath))
          {
            return new[] { currentPath };
          }
        }
        currentDirectory = Path.GetDirectoryName(currentDirectory);
      }
      searchDirectory = GetDirectory(baseDirectory, pattern);
      return Directory.Exists(searchDirectory) ? Directory.GetDirectories(searchDirectory, filename, SearchOption.AllDirectories).OrderByDescending(static item => item).ToArray() : new[] { "" };
    }

    // ReSharper disable once UnusedMember.Global
    public static string LocateDirectory(string baseDirectory, string pattern)
    {
      return LocateDirectories(baseDirectory, pattern).FirstOrDefault();
    }

    // ReSharper disable once UnusedMember.Global
    public static string LocateFile(string baseDirectory, string pattern)
    {
      return LocateFiles(baseDirectory, pattern).FirstOrDefault();
    }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once ReturnTypeCanBeEnumerable.Global
    public static string[] LocateFiles(string baseDirectory, string pattern)
    {
      Directory.Exists(baseDirectory).Check($"Base-directory '{baseDirectory} does not exist");
      string searchDirectory;
      var filename = Path.GetFileName(pattern);
      var currentDirectory = baseDirectory;
      while (!string.IsNullOrEmpty(currentDirectory))
      {
        searchDirectory = GetDirectory(currentDirectory, pattern);
        if (Directory.Exists(searchDirectory))
        {
          var currentPath = Directory.GetFiles(searchDirectory, filename, SearchOption.TopDirectoryOnly).OrderByDescending(static item => item).FirstOrDefault();
          if (!string.IsNullOrEmpty(currentPath) && File.Exists(currentPath))
          {
            return new[] { currentPath };
          }
        }
        currentDirectory = Path.GetDirectoryName(currentDirectory);
      }
      searchDirectory = GetDirectory(baseDirectory, pattern);
      return Directory.Exists(searchDirectory) ? Directory.GetFiles(searchDirectory, filename, SearchOption.AllDirectories).OrderByDescending(static item => item).ToArray() : new[] { "" };
    }

    public static string RemoveStrongQuotes(this string value)
    {
      return value.IsStrongQuote(out var result) ? result : value;
    }

    public static void WriteLine(string message, int verbosity = 5)
    {
      var verbosityLevel = Run2.TryGetVariable("verbositylevel", out var value) ? (int) value : 5;
      if (verbosityLevel <= verbosity)
      {
        Console.WriteLine(message);
      }
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

    private static bool EndsWith(this StringBuilder builder, char character)
    {
      return builder.Length == 0 || builder[^1] == character;
    }

    private static void ExpandPragmas(string sourcePath, string targetPath, bool expandIncludes, string lineCommand)
    {
      var replacements = new Dictionary<string, string>();
      var expandedContents = "";
      foreach (var currentLine in File.ReadAllText(sourcePath).Split('\n'))
      {
        var currentCleanLine = currentLine.Replace("\r", "");
        if (expandIncludes && currentCleanLine.TryGetControlValue(Globals.IncludePragma, out var filename))
        {
          var path = LocateFile(Path.GetDirectoryName(sourcePath), filename);
          if (!File.Exists(path))
          {
            path = LocateFile(GetProgramDirectory(), filename);
          }
          expandedContents += File.ReadAllText(path);
        }
        else if (currentCleanLine.TryGetControlValue(Globals.TargetPragma, out var target))
        {
          targetPath = $"{Path.GetDirectoryName(targetPath)}\\{target}";
        }
        else if (currentCleanLine.TryGetControlValue(Globals.ReplacePragma, out var replacementInformation))
        {
          var replacementItems = replacementInformation.Split('|');
          (replacementItems.Length == 2).Check("Replacement-pragma needs two arguments separated by '|'");
          replacements[replacementItems[0]] = replacementItems[1];
        }
        else
        {
          expandedContents += $"{currentLine}\n";
        }
      }
      if (!string.IsNullOrEmpty(lineCommand))
      {
        var lines = new List<string>();
        foreach (var line in expandedContents.Split('\n'))
        {
          lines.Add(Run2.RunCommand(lineCommand, new Tokens(new[] { line })) as string);
        }
        expandedContents = string.Join('\n', lines);
      }
      foreach (var (substring, replacement) in replacements)
      {
        expandedContents = expandedContents.Replace(substring, replacement);
      }
      File.WriteAllText(targetPath, expandedContents);
    }

    private static string GetDirectory(string directory, string pattern)
    {
      var result = Path.GetDirectoryName($"{directory}\\{pattern}");
      return result;
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

    private static bool TryGetControlValue(this string line, string controlTag, out string controlValue)
    {
      if (line.StartsWith(controlTag, StringComparison.Ordinal))
      {
        controlValue = line.Substring(controlTag.Length + 1);
        controlValue = controlValue.Substring(0, controlValue.Length - 3);
        return true;
      }
      controlValue = "";
      return false;
    }
  }
}