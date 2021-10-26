using System;
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
  public static class Helpers
  {
    public static void AddProperties(this object value, Properties properties)
    {
      if (properties != null)
      {
        if (!Globals.Properties.TryGetValue(value, out _))
        {
          Globals.Properties.AddOrUpdate(value, properties);
        }
      }
    }

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

    public static void Check([DoesNotReturnIf(false)] this bool condition, string message)
    {
      Check(condition, Globals.CurrentScriptPath, Globals.CurrentLineNumber, message);
    }

    public static void Check([DoesNotReturnIf(false)] this bool condition, string scriptNameOrPath, int lineNumber, string message)
    {
      var actualMessage = !string.IsNullOrEmpty(scriptNameOrPath) && 0 <= lineNumber ? $"Error in script '{scriptNameOrPath.GetScriptName()}' at line {lineNumber}: {message}" : $"Error in script: {message}";
      Checked(condition, actualMessage);
    }

    public static void Check([DoesNotReturnIf(false)] this bool condition, Token token, string message)
    {
      Check(condition, token.ScriptPath, token.LineNumber, message);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static void CopyFiles(string sourceDirectory, string pattern, string destinationDirectory, string destinationFilename, bool expand, string lineAction)
    {
      foreach (var sourcePath in Directory.GetFiles(sourceDirectory, pattern, SearchOption.AllDirectories))
      {
        var targetPath = destinationDirectory + "\\" + (string.IsNullOrEmpty(destinationFilename) ? Path.GetFileName(sourcePath) : destinationFilename);
        ExpandPragmas(sourcePath, targetPath, expand, lineAction);
      }
    }

    public static void Execute(string scriptPath, int lineNumber, string executablePath, string arguments, string workingDirectory, int processTimeout, int tryCount, int minimalExitCode, int maximalExitCode, out string output)
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
          var timedOut = process.StartProcess(processTimeout, out output);
          (!timedOut).Check($"The process {executablePath} has timed out");
          WriteLine($"Process '{executablePath}' terminated");
          Globals.Variables.SetLocal("exitcode", process.ExitCode);
          (minimalExitCode == process.ExitCode && process.ExitCode <= maximalExitCode).Check(scriptPath, lineNumber, $"The exit-code {process.ExitCode} lies not between the allowed range of {minimalExitCode} to {maximalExitCode}");
          return;
        }
        catch (Exception exception)
        {
          mostRecentException = exception;
          HandleException(exception, scriptPath, lineNumber);
        }
        Thread.Sleep(5000);
      }
      throw mostRecentException;
    }

    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
      foreach (var item in enumeration)
      {
        action(item);
      }
    }

    public static string GetInvalidParametersCountErrorMessage(string commandName, int actualCount, int expectedCountFrom, int expectedCountTo)
    {
      var expected = expectedCountFrom == expectedCountTo ? $"{expectedCountFrom}" : $"from {expectedCountFrom} to {expectedCountTo}";
      return $"Actual number of arguments for command '{commandName}' is {actualCount}, but the number expected is {expected}";
    }

    public static Properties GetProperties(this object value)
    {
      return Globals.Properties.GetOrCreateValue(value);
    }

    public static void HandleException(Exception exception, string scriptPath = null, int lineNumber = -1)
    {
      if (exception is not RuntimeException)
      {
        var actualScriptPath = scriptPath ?? Globals.CurrentScriptPath;
        var actualLineNumber = lineNumber < 0 ? Globals.CurrentLineNumber : lineNumber;
        WriteLine(!string.IsNullOrEmpty(actualScriptPath) && 0 <= actualLineNumber ? $"Exception in script '{actualScriptPath.GetScriptName()}' at line {actualLineNumber}:" : "Exception:");
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
      var bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
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

    public static bool IsStrongQuote(this object value, out string result)
    {
      if (value is string stringValue && stringValue.StartsWith(Scanner.TextDelimiter) && stringValue.EndsWith(Scanner.TextDelimiter))
      {
        result = stringValue.Substring(1, stringValue.Length - 2);
        return true;
      }
      result = null;
      return false;
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ReturnTypeCanBeEnumerable.Global")]
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

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static string LocateDirectory(string baseDirectory, string pattern)
    {
      return LocateDirectories(baseDirectory, pattern).FirstOrDefault();
    }

    public static string LocateFile(string baseDirectory, string pattern)
    {
      return LocateFiles(baseDirectory, pattern).FirstOrDefault();
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ReturnTypeCanBeEnumerable.Global")]
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

    public static void SetCurrentScriptPathAndLineNumber(this object value)
    {
      var properties = value.GetProperties();
      if (!string.IsNullOrEmpty(properties?.ScriptPath) && 0 <= properties.LineNumber)
      {
        Globals.CurrentScriptPath = properties.ScriptPath;
        Globals.CurrentLineNumber = properties.LineNumber;
      }
    }

    public static object ToBestType(this string currentToken)
    {
      object bestTypedObject;
      if (bool.TryParse(currentToken, out var boolResult))
      {
        bestTypedObject = boolResult;
      }
      else if (int.TryParse(currentToken, out var intResult))
      {
        bestTypedObject = intResult;
      }
      else if (BigInteger.TryParse(currentToken, out var bigIntegerResult))
      {
        bestTypedObject = bigIntegerResult;
      }
      else if (double.TryParse(currentToken, NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleResult))
      {
        bestTypedObject = doubleResult;
      }
      else
      {
        bestTypedObject = currentToken;
      }
      return bestTypedObject;
    }

    public static void Write(string message, int verbosity = 5)
    {
      Write(message, (int) Globals.Variables.Get("verbositylevel", 5), verbosity);
    }

    public static void WriteLine(string message, int verbosity = 5)
    {
      Write($"{message}\n", (int) Globals.Variables.Get("verbositylevel", 5), verbosity);
    }

    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
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
            path = LocateFile(Globals.ProgramDirectory, filename);
          }
          expandedContents += path != null && File.Exists(path) ? File.ReadAllText(path) : "";
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
        expandedContents = string.Join('\n', expandedContents.Split('\n').Select(line => Run2.ExecuteCommand(lineCommand, new Items(line)).ToString()));
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

    private static string GetScriptName(this string path)
    {
      return Path.GetFileNameWithoutExtension(path);
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

    private static bool StartProcess(this Process process, int processTimeout, out string output)
    {
      process.Start();
      output = "";
      var timedOut = false;
      var stopwatch = new Stopwatch();
      stopwatch.Start();
      while (true)
      {
        if (process.HasExited)
        {
          var remainder = process.StandardOutput.ReadToEnd();
          if (!string.IsNullOrEmpty(remainder))
          {
            if (0 < output.Length)
            {
              output += '\n';
            }
            output += remainder;
            Write(remainder);
          }
          break;
        }
        if (processTimeout < stopwatch.ElapsedMilliseconds)
        {
          timedOut = true;
          break;
        }
        var line = process.StandardOutput.ReadLine();
        if (line != null)
        {
          if (0 < output.Length)
          {
            output += '\n';
          }
          output += line;
          WriteLine(line);
        }
      }
      stopwatch.Stop();
      return timedOut;
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

    private static void Write(string message, int verbosityLevel, int verbosity)
    {
      if (verbosityLevel <= verbosity)
      {
        var writeCallback = Globals.Variables.Get("writecallback", null);
        if (writeCallback == null)
        {
          Console.Write(message);
        }
        else
        {
          Run2.ExecuteCommand(writeCallback.ToString(), new Items(message));
          Console.Write("");
        }
      }
    }

    private static void WriteLine(string message, int verbosityLevel, int verbosity)
    {
      Write($"{message}\n", verbosityLevel, verbosity);
    }
  }
}