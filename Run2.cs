using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Run2
{
  internal static class Run2
  {
    private const char BlockEnd = ')';
    private const char BlockStart = '(';
    private const string CommandToken = "command";
    private const char StrongQuote = '"';
    private const char WeakQuote = '\'';
    private static readonly HashSet<string> acceptedTypes = new() { "Console", "Math", "Array", "File", "Directory", "Path", "String", "Helpers", "Variables", "Tokens" };
    private static readonly Dictionary<string, Command> commands = new();
    private static readonly Variables variables = new();

    public static void EnterScope()
    {
      variables.EnterScope();
    }

    public static object Evaluate(object value)
    {
      switch (value)
      {
        case string stringValue:
        {
          if (variables.TryGetValue(stringValue, out var result))
          {
            return result;
          }
          foreach (var key in variables.GetKeys())
          {
            stringValue = stringValue.Replace("[" + key + "]", variables.Get(key).ToString());
          }
          if (stringValue.StartsWith(BlockStart) && stringValue.EndsWith(BlockEnd))
          {
            return RunCommand(stringValue.Substring(1, stringValue.Length - 2));
          }
          return stringValue;
        }
        case Tokens tokensValue:
        {
          var tokensValueClone = tokensValue.Clone();
          return RunCommand(tokensValueClone.DequeueString(), tokensValueClone);
        }
      }
      return value;
    }

    public static void Initialize()
    {
      variables.globalScopeCreated += OnGlobalScopeCreated;
      Globals.Debug = CommandLineParser.OptionExists("debug");
      Globals.BaseDirectory = CommandLineParser.GetOptionString("baseDirectory", Helpers.GetProgramDirectory());
      Globals.ScriptName = CommandLineParser.GetOptionString("scriptName", Globals.DefaultScriptName);
      Globals.Arguments = new Tokens(CommandLineParser.GetOptionStrings("scriptArguments", new List<string>()));
      Globals.ScriptPath = Helpers.FindFile(Globals.BaseDirectory, Globals.ScriptName);
      File.Exists(Globals.ScriptPath).Check($"Could not find script '{Globals.ScriptName}' (base-directory: '{Globals.BaseDirectory}')");
      BuildSystemCommands();
      BuildInvokeCommands();
      LoadCommand(Globals.ScriptPath);
      RunCommand(Helpers.GetCommandNameFromPath(Globals.ScriptPath), Globals.Arguments);
      Helpers.WriteLine("Script terminated successfully");
    }

    private static void OnGlobalScopeCreated(object sender, EventArgs e)
    {
      SetGlobalVariable("commands", commands);
      SetGlobalVariable("variables", variables);
    }

    public static void LeaveScope()
    {
      variables.LeaveScope();
    }

    public static object RunCommand(string name, Tokens arguments)
    {
      commands.ContainsKey(name).Check($"Command '{name}' not found");
      if (Globals.Debug)
      {
        Helpers.WriteLine($"Begin '{name}'");
      }
      var result = commands[name].Run(arguments.Clone());
      if (Globals.Debug)
      {
        Helpers.WriteLine($"End '{name}'");
      }
      return result;
    }

    public static void SetGlobalVariable(string name, object value)
    {
      variables.SetGlobal(name, value);
    }

    public static void SetLocalVariable(string name, object value)
    {
      variables.SetLocal(name, value);
    }

    private static bool AcceptMember(MemberInfo member)
    {
      return member switch
      {
        MethodInfo methodInfo => !methodInfo.IsSpecialName,
        PropertyInfo propertyInfo => !propertyInfo.IsSpecialName,
        _ => false
      };
    }

    private static bool AcceptType(Type type)
    {
      return type.IsPublic && type.IsClass && !type.IsNested && !type.IsGenericType && acceptedTypes.Contains(type.Name);
    }

    private static void BuildInvokeCommands()
    {
      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        foreach (var type in assembly.GetTypes())
        {
          if (AcceptType(type))
          {
            Debug.WriteLine(type.Name);
            for (var pass = 0; pass < 2; ++pass)
            {
              var bindingFlags = BindingFlags.Public;
              bindingFlags |= pass == 0 ? BindingFlags.Static : BindingFlags.Instance;
              foreach (var member in type.GetMembers(bindingFlags))
              {
                if (AcceptMember(member))
                {
                  if (pass == 0)
                  {
                    var key = type.Name + "." + member.Name;
                    if (!commands.ContainsKey(key))
                    {
                      commands.Add(key, new InvokeCommand(member.Name, type));
                    }
                  }
                  else
                  {
                    var key = member.Name;
                    if (!commands.ContainsKey(key))
                    {
                      commands.Add(member.Name, new InvokeCommand(member.Name, null));
                    }
                  }
                }
              }
            }
            if (0 < type.GetConstructors().Length)
            {
              var key = type.Name + ".new";
              if (!commands.ContainsKey(key))
              {
                commands.Add(key, new InvokeCommand("_new", type));
              }
            }
          }
        }
      }
    }

    private static void BuildSystemCommands()
    {
      foreach (var method in typeof(CommandActions).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static))
      {
        var attribute = (CommandActionAttribute) Attribute.GetCustomAttribute(method, typeof(CommandActionAttribute));
        commands.Add(attribute?.CommandName ?? method.Name.ToLower(), new SystemCommand((CommandAction) Delegate.CreateDelegate(typeof(CommandAction), method)));
      }
    }

    private static void BuildUserCommands(Dictionary<string, Tokens> definitions)
    {
      foreach (var name in definitions.Keys)
      {
        commands.Add(name, new UserCommand());
      }
      foreach (var (key, tokens) in definitions)
      {
        var userCommand = commands[key] as UserCommand;
        (userCommand != null).Check("Assertion");
        while (0 < tokens.Count)
        {
          var token = tokens.Peek();
          if (commands.ContainsKey(token.ToString() ?? string.Empty))
          {
            break;
          }
          userCommand.ParameterNames.Enqueue(token);
          tokens.Dequeue();
        }
        while (0 < tokens.Count)
        {
          var subCommand = new SubCommand { CommandName = tokens.Dequeue().ToString() };
          userCommand.SubCommands.Add(subCommand);
          while (0 < tokens.Count && !commands.ContainsKey(tokens.PeekString()))
          {
            subCommand.Arguments.Enqueue(tokens.Dequeue());
          }
        }
      }
    }

    private static void GetCommandDefinitions(string firstCommandName, Tokens tokens, out Dictionary<string, Tokens> definitions)
    {
      definitions = new Dictionary<string, Tokens>();
      var commandName = firstCommandName;
      var subtokens = new Tokens();
      while (0 < tokens.Count)
      {
        var token = tokens.Dequeue();
        if (token.ToString()?.Equals(CommandToken, StringComparison.OrdinalIgnoreCase) == true)
        {
          if (0 < subtokens.Count)
          {
            (commandName != null).Check("Unexpected null while searching for command-definitions");
            definitions.Add(commandName, subtokens.Clone());
            subtokens.Clear();
          }
          (0 < tokens.Count).Check("Unexpected end of script while searching for command-definitions");
          commandName = tokens.Dequeue().ToString();
        }
        else
        {
          subtokens.Enqueue(token);
        }
      }
      if (0 < subtokens.Count)
      {
        (commandName != null).Check("Unexpected null while searching for command-definitions");
        definitions.Add(commandName, subtokens.Clone());
      }
    }

    private static Tokens GetTokens(string code, bool keepQuotes)
    {
      var result = new Tokens();
      var currentToken = "";
      var terminate = false;
      var blockLevel = 0;
      var expectedBlockEnders = new Stack<char>();
      var expectedBlockLevels = new Stack<int>();
      var characters = new Queue<char>(code);
      while (!terminate && 0 < characters.Count)
      {
        var currentCharacter = characters.Dequeue();
        if (char.IsWhiteSpace(currentCharacter) && blockLevel == 0)
        {
          if (currentToken == "")
          {
            continue;
          }
          result.Enqueue(IsBlock(ref currentToken) ? GetTokens(currentToken, keepQuotes) : currentToken);
          currentToken = "";
        }
        else
        {
          switch (currentCharacter)
          {
            case BlockStart:
            {
              currentToken += currentCharacter;
              ++blockLevel;
              expectedBlockLevels.Push(blockLevel);
              expectedBlockEnders.Push(BlockEnd);
              break;
            }
            case BlockEnd:
            {
              var expectedBlockEnder = expectedBlockEnders.Pop();
              (expectedBlockEnder == BlockEnd).Check($"Expected block-ender: {expectedBlockEnder}");
              var expectedBlockLevel = expectedBlockLevels.Pop();
              (expectedBlockLevel == blockLevel).Check($"Expected block-level: {expectedBlockLevel}");
              --blockLevel;
              currentToken += currentCharacter;
              break;
            }
            case StrongQuote:
            case WeakQuote:
            {
              if (0 < expectedBlockEnders.Count && expectedBlockEnders.Peek() == currentCharacter)
              {
                expectedBlockEnders.Pop();
                var expectedBlockLevel = expectedBlockLevels.Pop();
                (expectedBlockLevel == blockLevel).Check($"Expected block-level: {expectedBlockLevel}");
                --blockLevel;
                if (keepQuotes || currentCharacter == StrongQuote || 0 < blockLevel)
                {
                  currentToken += currentCharacter;
                }
              }
              else
              {
                if (keepQuotes || currentCharacter == StrongQuote || 0 < blockLevel)
                {
                  currentToken += currentCharacter;
                }
                ++blockLevel;
                expectedBlockLevels.Push(blockLevel);
                expectedBlockEnders.Push(currentCharacter);
              }
              break;
            }
            case '~':
              var nextCharacter = characters.Dequeue();
              switch (nextCharacter)
              {
                case 'n':
                  currentToken += '\n';
                  break;
                default:
                  currentToken += nextCharacter;
                  break;
              }
              break;
            /*
            case '!':
              if (blockLevel == 0)
              {
                terminate = true;
              }
              else
              {
                currentToken += currentCharacter;
              }
              break;
            */
            default:
              currentToken += currentCharacter;
              break;
          }
        }
      }
      if (currentToken != "")
      {
        result.Enqueue(IsBlock(ref currentToken) ? GetTokens(currentToken, keepQuotes) : currentToken);
      }
      return result;
    }

    private static bool IsBlock(ref string text)
    {
      if (text.StartsWith(BlockStart) && text.EndsWith(BlockEnd))
      {
        text = text.Substring(1, text.Length - 2);
        return true;
      }
      return false;
    }

    private static void LoadCommand(string path)
    {
      File.Exists(path).Check($"Script '{path}' not found");
      var tokens = GetTokens(File.ReadAllText(path), false);
      GetCommandDefinitions(Path.GetFileNameWithoutExtension(path), tokens, out var definitions);
      BuildUserCommands(definitions);
    }

    private static object RunCommand(string line)
    {
      var tokens = GetTokens(line, false);
      var commandName = tokens.Dequeue().ToString();
      return RunCommand(commandName, tokens);
    }
  }
}