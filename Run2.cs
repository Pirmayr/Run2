using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Run2
{
  internal static class Run2
  {
    private const char BlockEnd = ')';
    private const char BlockStart = '(';
    private const string CommandToken = "command";
    private const char StrongQuote = '"';
    private const char WeakQuote = '\'';
    private static readonly HashSet<string> acceptedTypes = new() { "Console", "Math", "Array", "File", "Directory", "Path", "String", "Helpers", "Variables", "Tokens", "SubCommands" };
    private static readonly Dictionary<string, Command> commands = new();
    private static readonly Variables variables = new();

    public static void EnterScope()
    {
      variables.EnterScope();
    }

    public static object Evaluate(object value)
    {
      if (Helpers.IsAnyString(value, out var stringValue))
      {
        if (value is string && variables.TryGetValue(stringValue, out var result))
        {
          return result;
        }
        foreach (var key in variables.GetKeys())
        {
          stringValue = stringValue.Replace("[" + key + "]", variables.Get(key).ToString());
        }
        return stringValue;
      }
      if (value is SubCommands subCommands)
      {
        return RunSubCommands(subCommands);
      }
      return value;
    }

    public static string GetHelp()
    {
      var result = new StringBuilder();
      result.Append("<style>th {background: red;}</style>");
      foreach (var (name, command) in commands.OrderBy(static item => item.Key))
      {
        if (command is SystemCommand or UserCommand)
        {
          if (0 < result.Length)
          {
            result.Append('\n');
          }
          result.Append("#### " + name);
          result.Append('\n');
          if (!string.IsNullOrEmpty(command.CommandDescription))
          {
            result.Append(command.CommandDescription);
          }
          if (command is UserCommand userCommand && 0 < userCommand.ParameterNames.Count)
          {
            result.Append('\n');
            result.Append("|<span style='display:inline-block;width:10em;'>Name</span>|<span style='display: inline-block; width:20em'>Name</span>|");
            result.Append('\n');
            result.Append("|-|-|");
            foreach (var token in userCommand.ParameterNames)
            {
              result.Append('\n');
              var parameterName = Helpers.ParameterName(token);
              result.Append("|" + parameterName + "|");
              result.Append(userCommand.ParameterDescriptions.TryGetValue(parameterName ?? string.Empty, out var parameterDescription) ? parameterDescription : parameterName);
              result.Append('|');
            }
          }
        }
      }
      return result.ToString();
    }

    public static object GetVariable(string name)
    {
      return variables.Get(name);
    }

    public static void Initialize()
    {
      CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
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

    public static void LeaveScope()
    {
      variables.LeaveScope();
    }

    public static object RunSubCommands(SubCommands subCommands)
    {
      object result = null;
      foreach (var subCommand in subCommands)
      {
        result = RunCommand(subCommand.CommandName, subCommand.Arguments);
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

    public static void SetVariable(string name, object value)
    {
      variables.Set(name, value);
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

    private static void BuildUserCommands(string path, Tokens tokens)
    {
      var firstCommandName = Path.GetFileNameWithoutExtension(path);
      var definitions = new Dictionary<string, Tokens>();
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
        definitions.Add(commandName, subtokens);
      }
      foreach (var name in definitions.Keys)
      {
        commands.Add(name, new UserCommand { Name = name });
      }
      foreach (var (definitionName, definitionTokens) in definitions)
      {
        var userCommand = commands[definitionName] as UserCommand;
        (userCommand != null).Check("User-command must not be null");
        if (TryPeekDescription(definitionTokens, out var description))
        {
          userCommand.CommandDescription = description;
          definitionTokens.Dequeue();
        }
        while (0 < definitionTokens.Count)
        {
          var peekedToken = definitionTokens.Peek();
          if (peekedToken is string peekedTokenString && commands.ContainsKey(peekedTokenString))
          {
            break;
          }
          userCommand.ParameterNames.Enqueue(peekedToken);
          definitionTokens.Dequeue();
          if (TryPeekDescription(definitionTokens, out var parameterDescription))
          {
            userCommand.ParameterDescriptions.Add(Helpers.ParameterName(peekedToken), parameterDescription);
            definitionTokens.Dequeue();
          }
        }
        userCommand.SubCommands = GetSubCommands(definitionTokens);
      }
    }

    private static void EnqueueToken(string currentToken, Tokens result)
    {
      if (IsWeakQuoted(ref currentToken))
      {
        result.Enqueue(new WeakQuotedString { Value = currentToken });
      }
      else if (IsBlock(ref currentToken))
      {
        result.Enqueue(GetTokens(currentToken));
      }
      else
      {
        result.Enqueue(currentToken);
      }
    }

    private static SubCommands GetSubCommands(Tokens tokens)
    {
      var result = new SubCommands();
      while (0 < tokens.Count)
      {
        var subCommand = new SubCommand { CommandName = tokens.Dequeue().ToString() };
        result.Add(subCommand);
        while (0 < tokens.Count)
        {
          if (tokens.Peek() is string peekedTokenString && commands.ContainsKey(peekedTokenString))
          {
            break;
          }
          var token = tokens.Dequeue();
          if (token is Tokens tokensValue)
          {
            subCommand.Arguments.Enqueue(GetSubCommands(tokensValue));
          }
          else
          {
            subCommand.Arguments.Enqueue(token);
          }
        }
      }
      return result;
    }

    private static Tokens GetTokens(string code)
    {
      var result = new Tokens();
      var currentToken = "";
      var blockLevel = 0;
      var expectedBlockEnders = new Stack<char>();
      var expectedBlockLevels = new Stack<int>();
      var characters = new Queue<char>(code);
      while (0 < characters.Count)
      {
        var currentCharacter = characters.Dequeue();
        if (char.IsWhiteSpace(currentCharacter) && blockLevel == 0)
        {
          if (currentToken == "")
          {
            continue;
          }
          EnqueueToken(currentToken, result);
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
                if (currentCharacter is StrongQuote or WeakQuote || 0 < blockLevel)
                {
                  currentToken += currentCharacter;
                }
              }
              else
              {
                if (currentCharacter is StrongQuote or WeakQuote || 0 < blockLevel)
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
            default:
              currentToken += currentCharacter;
              break;
          }
        }
      }
      if (currentToken != "")
      {
        EnqueueToken(currentToken, result);
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

    private static bool IsWeakQuoted(ref string text)
    {
      if (text.StartsWith(WeakQuote) && text.EndsWith(WeakQuote))
      {
        text = text.Substring(1, text.Length - 2);
        return true;
      }
      return false;
    }

    private static void LoadCommand(string path)
    {
      File.Exists(path).Check($"Script '{path}' not found");
      var tokens = GetTokens(File.ReadAllText(path));
      BuildUserCommands(path, tokens);
    }

    private static void OnGlobalScopeCreated(object sender, EventArgs e)
    {
      SetGlobalVariable("commands", commands);
      SetGlobalVariable("variables", variables);
      SetGlobalVariable("scriptpath", Globals.ScriptPath);
      SetGlobalVariable("basedirectory", Globals.BaseDirectory);
    }

    private static object RunCommand(string name, Tokens arguments)
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

    private static bool TryPeekDescription(Tokens tokens, out string description)
    {
      if (tokens.TryPeek(out var descriptionCandidate) && descriptionCandidate is WeakQuotedString weakQuotedString)
      {
        description = weakQuotedString.Value;
        return true;
      }
      description = null;
      return false;
    }
  }
}