using System.Collections.Generic;

namespace Run2
{
  internal static class Globals
  {
    public const char BlockEnd = ')';
    public const char BlockStart = '(';
    public const string IncludePragma = "<!--@include";
    public const int MaxCodeLineLength = 100;
    public const string PragmaCommand = "command";
    public const string ReplacePragma = "<!--@replace";
    public const string ScriptNameDefault = "build.run2";
    public const string ScriptNameSystem = "system.run2";
    public const char StrongQuote = '"';
    public const string TargetPragma = "<!--@target";
    public const string TestCommand = "performtest";
    public const string VariableNameArguments = "arguments";
    public const char WeakQuote = '\'';

    public static HashSet<string> AcceptedTypes { get; } = new() { "Array", "ArrayList", "BigInteger", "Char", "CodeFormatter", "Console", "Convert", "DictionaryEntry", "Directory", "File", "Hashtable", "Helpers", "Int32", "Math", "Path", "Queue", "String", "Stack", "SubCommands", "Items", "Variables" };

    public static Items Arguments { get; set; }

    public static string BaseDirectory { get; set; }

    public static Dictionary<string, ICommand> Commands { get; } = new();

    public static bool Debug { get; set; }

    public static bool DoBreak { get; set; }

    public static object ScriptDirectory { get; set; }

    public static string ScriptName { get; set; }

    public static string ScriptPath { get; set; }

    public static string ScriptPathSystem { get; set; }

    public static Variables Variables { get; } = new();
  }
}