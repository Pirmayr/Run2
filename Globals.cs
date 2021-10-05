using System.Collections.Generic;

namespace Run2
{
  internal static class Globals
  {
    public const string DefaultScriptName = "build.run2";
    public const int MaxCodeLineLength = 100;

    public static Tokens Arguments { get; set; }

    public static string BaseDirectory { get; set; }

    public static Dictionary<string, Command> Commands { get; } = new();

    public static bool Debug { get; set; }

    public static bool DoBreak { get; set; }

    public static string ScriptName { get; set; }

    public static string ScriptPath { get; set; }
  }
}