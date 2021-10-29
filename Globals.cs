using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Run2
{
  public static class Globals
  {
    public const string DefaultExtension = "run2";
    public const string DefaultUserScriptFilename = "build.run2";
    public const string IncludePragma = "<!--@include";
    public const int MaxCodeLineLength = 80;
    public const string PragmaCommand = "command";
    public const string PragmaImport = "loadscript";
    public const string ReplacePragma = "<!--@replace";
    public const string SystemScriptName = "system";
    public const string TargetPragma = "<!--@target";
    public const string TestCommand = "performtest";
    public const string VariableNameArguments = "arguments";

    public static int TryCatchFinallyLevel { get; set; }

    public static Items Arguments { get; set; }

    public static string BaseDirectory { get; set; }

    public static Dictionary<string, ICommand> Commands { get; } = new();

    public static int CurrentLineNumber { get; set; }

    public static string CurrentScriptPath { get; set; }

    public static bool Debug { get; set; }

    public static bool DoBreak { get; set; }

    public static Dictionary<string, List<string>> Imports { get; } = new();

    public static string ProgramDirectory { get; set; }

    public static ConditionalWeakTable<object, Properties> Properties { get; } = new();

    public static object UserScriptDirectory => Path.GetDirectoryName(UserScriptPath);

    public static string UserScriptFilename { get; set; }

    public static string UserScriptPath { get; set; }

    public static Variables Variables { get; } = new();
  }
}