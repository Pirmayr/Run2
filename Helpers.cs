using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Run2
{
  internal static class Helpers
  {
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

    public static string GetCommandNameFromPath(string path)
    {
      return Path.GetFileNameWithoutExtension(path);
    }

    public static string GetProgramDirectory()
    {
      return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }

    public static void HandleException(Exception exception)
    {
      WriteLine(exception.Message);
    }

    public static object Invoke(string memberName, object typeOrTarget, object[] arguments)
    {
      var isType = typeOrTarget is Type;
      var type = isType ? (Type) typeOrTarget : typeOrTarget.GetType();
      var target = isType ? null : typeOrTarget;
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
      return type.InvokeMember(memberName, bindingFlags, null, target, arguments);
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
  }
}