using System;
using System.Collections.Generic;

namespace Run2
{
  public static class CommandLineParser
  {
    private const string CommandPrefix = "-";
    private static readonly Dictionary<string, List<string>> options = new();
    private static bool commandLineParsed;

    public static string GetOptionString(string name, string defaultValue)
    {
      Parse();
      var key = GetKey(name);
      return options.TryGetValue(key, out var value) ? string.Join("|", value) : defaultValue;
    }

    public static IEnumerable<string> GetOptionStrings(string name, List<string> defaultValue = null)
    {
      Parse();
      var key = GetKey(name);
      return options.TryGetValue(key, out var value) ? value : defaultValue ?? new List<string>();
    }

    public static bool OptionExists(string name)
    {
      Parse();
      var key = GetKey(name);
      return options.ContainsKey(key);
    }

    private static string GetKey(string name)
    {
      return CommandPrefix + name.ToLower();
    }

    private static void Parse()
    {
      if (commandLineParsed)
      {
        return;
      }
      commandLineParsed = true;
      var commandLineArguments = Environment.GetCommandLineArgs();
      var currentOption = string.Empty;
      foreach (var currentItem in commandLineArguments)
      {
        if (currentItem.StartsWith(CommandPrefix, StringComparison.Ordinal))
        {
          currentOption = currentItem.ToLower();
          if (!options.ContainsKey(currentOption))
          {
            options.Add(currentOption, new List<string>());
          }
        }
        else
        {
          if (options.ContainsKey(currentOption))
          {
            options[currentOption].Add(currentItem);
          }
        }
      }
    }
  }
}