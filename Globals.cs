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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Run2
{
  public static class Globals
  {
    public const string DefaultExtension = "run2";
    public const string DefaultUserScriptFilename = "build.run2";
    public const int MaxCodeLineLength = 80;
    public const string PragmaCommand = "command";
    public const string PragmaImport = "loadscript";
    public const string SystemScriptName = "system";
    public const string TestCommand = "performtest";
    public const string VariableNameArguments = "arguments";
    private const string IncludePragma = "<!--@include";
    private const string ReplacePragma = "<!--@replace";
    private const string TargetPragma = "<!--@target";
    public delegate object CommandAction(Items arguments);

    public enum TokenKind
    {
      Element,
      LeftParenthesis,
      RightParenthesis,
      Quote,
      Text,
      PragmaCommand,
      PragmaReadScript,
      CommandName,
      EOF
    }

    public static string BaseDirectory { get; set; }

    public static Dictionary<string, ICommand> Commands { get; } = new();

    public static int CurrentLineNumber { get; set; }

    public static string CurrentScriptPath { get; set; }

    public static bool Debug { get; set; }

    public static bool DoBreak { get; set; }

    public static Dictionary<string, List<string>> Imports { get; } = new();

    public static string ProgramDirectory { get; set; }

    public static Items ScriptArguments { get; set; }

    public static int TryCatchFinallyLevel { get; set; }

    public static object UserScriptDirectory => Path.GetDirectoryName(UserScriptPath);

    public static string UserScriptFilename { get; set; }

    public static string UserScriptPath { get; set; }

    public static Variables Variables { get; } = new();

    private static ConditionalWeakTable<object, Properties> Properties { get; } = new();

    public static void AddProperties(this object value, Properties properties)
    {
      if (properties != null)
      {
        if (!Properties.TryGetValue(value, out _))
        {
          Properties.AddOrUpdate(value, properties);
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
      if (!condition)
      {
        throw new Exception(message);
      }
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

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static bool EndsWith(this StringBuilder builder, char character)
    {
      return builder.Length == 0 || builder[^1] == character;
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
          Variables.SetLocal("exitcode", process.ExitCode);
          (minimalExitCode <= process.ExitCode && process.ExitCode <= maximalExitCode).Check($"The exit-code {process.ExitCode} lies not between the allowed range of {minimalExitCode} to {maximalExitCode}");
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

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static string GetDirectory(string directory, string pattern)
    {
      var result = Path.GetDirectoryName($"{directory}\\{pattern}");
      return result;
    }

    public static string GetInvalidParametersCountErrorMessage(string commandName, int actualCount, int expectedCountFrom, int expectedCountTo)
    {
      var expected = expectedCountFrom == expectedCountTo ? $"{expectedCountFrom}" : $"from {expectedCountFrom} to {expectedCountTo}";
      return $"Actual number of arguments for command '{commandName}' is {actualCount}, but the number expected is {expected}";
    }

    public static Properties GetProperties(this object value)
    {
      return Properties.GetOrCreateValue(value);
    }

    public static void HandleException(Exception exception, string scriptPath = null, int lineNumber = -1)
    {
      if (TryCatchFinallyLevel == 0 && exception is not RuntimeException)
      {
        var actualScriptPath = scriptPath ?? CurrentScriptPath;
        var actualLineNumber = lineNumber < 0 ? CurrentLineNumber : lineNumber;
        var message = exception.InnerMostException().Message;
        WriteLine(!string.IsNullOrEmpty(actualScriptPath) && 0 <= actualLineNumber ? $"Error in script '{actualScriptPath.GetScriptName()}' at line {actualLineNumber}: {message}" : $"Error: {message}");
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
        CurrentScriptPath = properties.ScriptPath;
        CurrentLineNumber = properties.LineNumber;
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

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static void Write(string message, int verbosity = 5)
    {
      Write(message, (int) Variables.Get("verbositylevel", 5), verbosity);
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static void Write(string message, int verbosityLevel, int verbosity)
    {
      if (verbosityLevel <= verbosity)
      {
        var writeCallback = Variables.Get("writecallback", null);
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

    public static void WriteLine(string message, int verbosity = 5)
    {
      Write($"{message}\n", (int) Variables.Get("verbositylevel", 5), verbosity);
    }

    private static void ExpandPragmas(string sourcePath, string targetPath, bool expandIncludes, string lineCommand)
    {
      var replacements = new Dictionary<string, string>();
      var expandedContents = "";
      foreach (var currentLine in File.ReadAllText(sourcePath).Split('\n'))
      {
        var currentCleanLine = currentLine.Replace("\r", "");
        if (expandIncludes && currentCleanLine.TryGetControlValue(IncludePragma, out var filename))
        {
          var path = LocateFile(Path.GetDirectoryName(sourcePath), filename);
          if (!File.Exists(path))
          {
            path = LocateFile(ProgramDirectory, filename);
          }
          expandedContents += path != null && File.Exists(path) ? File.ReadAllText(path) : "";
        }
        else if (currentCleanLine.TryGetControlValue(TargetPragma, out var target))
        {
          targetPath = $"{Path.GetDirectoryName(targetPath)}\\{target}";
        }
        else if (currentCleanLine.TryGetControlValue(ReplacePragma, out var replacementInformation))
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

    public sealed class CharacterQueue : Queue<char>
    {
      public CharacterQueue(string text) : base(text)
      {
      }
    }

    public class List : List<object>
    {
      public List(IEnumerable<object> values) : base(values)
      {
      }

      public List()
      {
      }
    }

    public sealed class RuntimeException : Exception
    {
      public RuntimeException(string message) : base(message)
      {
      }
    }

    public sealed class SubCommand
    {
      public Items Arguments { get; } = new();

      public string CommandName { get; set; }
    }

    public sealed class SubCommands : List
    {
    }

    public sealed class Tokens : Queue<Token>
    {
      public new Token Dequeue()
      {
        var result = base.Dequeue();
        result.SetCurrentScriptPathAndLineNumber();
        return result;
      }
    }
  }
}