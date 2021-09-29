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
    private const string TestCommand = "performtest";
    private const char WeakQuote = '\'';
    private static readonly HashSet<string> acceptedTypes = new() { "Console", "Math", "Array", "File", "Directory", "Path", "String", "Helpers", "Variables", "Tokens", "SubCommands", "Hashtable", "DictionaryEntry" };
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
      var commandReferences = GetCommandReferences(TestCommand);
      var result = new StringBuilder();
      var insertLine = false;
      result.Append("# Predefined Run2-Commands");
      foreach (var (name, command) in commands.Where(static item => !item.Value.HideHelp).OrderBy(static item => item.Key))
      {
        if (insertLine)
        {
          result.Append("\n\n---");
        }
        insertLine = true;
        result.Append($"\n\n##### {name}");
        if (!string.IsNullOrEmpty(command.GetDescription()))
        {
          result.Append($"\n\n{command.GetDescription()}");
        }
        var parameterNames = command.GetParameterNames();
        if (0 < parameterNames.Count)
        {
          result.Append("\n");
          foreach (var parameterName in parameterNames)
          {
            var parameterDescription = command.GetParameterDescription(parameterName);
            result.Append($"\n* {parameterName}" + (string.IsNullOrEmpty(parameterDescription) ? "" : $": {parameterDescription}"));
          }
        }
        if (commandReferences.TryGetValue(name, out var references))
        {
          result.Append("\n\nExamples:\n");
          foreach (var reference in references)
          {
            (reference.Arguments.Count == 2).Check("Expected test with exactly two parts");
            var argumentsClone = reference.Arguments.Clone();
            var part1 = argumentsClone.Dequeue();
            var part2 = argumentsClone.Dequeue();
            var code1 = part1 is SubCommands subCommandsValue ? subCommandsValue.ToCode() : "";
            var code2 = new Tokens(new[] { part2 }).ToCode();
            result.Append($"\n* &nbsp;{code1} -> {code2}");
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
      return type.IsPublic && (type.IsClass || type.IsStruct()) && !type.IsNested && !type.IsGenericType && acceptedTypes.Contains(type.Name);
    }

    private static void BuildInvokeCommands()
    {
      var localFullName = Assembly.GetExecutingAssembly().FullName;
      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        foreach (var type in assembly.GetTypes())
        {
          if (AcceptType(type))
          {
            var isLocalType = type.Assembly.FullName == localFullName;
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
                      commands.Add(key, new InvokeCommand(member.Name, type, isLocalType ? "" : $"{type.FullName}.{member.Name}"));
                    }
                  }
                  else
                  {
                    var key = member.Name;
                    if (!commands.ContainsKey(key))
                    {
                      commands.Add(member.Name, new InvokeCommand(member.Name, null, isLocalType ? "" : $"{type.FullName}.{member.Name}"));
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
                commands.Add(key, new InvokeCommand("_new", type, ""));
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
      if (IsWeaklyQuoted(ref currentToken))
      {
        result.Enqueue(new WeaklyQuotedString(currentToken));
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

    private static void GetCommandNames(SubCommand subCommand, ref HashSet<string> commandNames)
    {
      commandNames.Add(subCommand.CommandName);
      foreach (var token in subCommand.Arguments)
      {
        if (token is SubCommands subCommandsValue)
        {
          foreach (var subCommandValue in subCommandsValue)
          {
            GetCommandNames(subCommandValue, ref commandNames);
          }
        }
      }
    }

    private static Dictionary<string, List<SubCommand>> GetCommandReferences(string filterCommandName)
    {
      var filteredSubCommands = new List<SubCommand>();
      foreach (var command in commands.Values)
      {
        if (command is UserCommand userCommand)
        {
          foreach (var subCommand in userCommand.SubCommands)
          {
            if (subCommand.CommandName == filterCommandName)
            {
              filteredSubCommands.Add(subCommand);
            }
          }
        }
      }
      var result = new Dictionary<string, List<SubCommand>>();
      foreach (var filteredSubCommand in filteredSubCommands)
      {
        var commandNames = new HashSet<string>();
        GetCommandNames(filteredSubCommand, ref commandNames);
        foreach (var commandName in commandNames)
        {
          if (commandName != filterCommandName)
          {
            if (!result.TryGetValue(commandName, out var references))
            {
              references = new List<SubCommand>();
              result.Add(commandName, references);
            }
            references.Add(filteredSubCommand);
          }
        }
      }
      return result;
    }

    private static SubCommands GetSubCommands(Tokens tokens)
    {
      var result = new SubCommands();
      while (0 < tokens.Count)
      {
        var subCommand = new SubCommand { CommandName = tokens.DequeueString(false) };
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

    private static bool IsWeaklyQuoted(ref string text)
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
      if (tokens.TryPeek(out var descriptionCandidate) && descriptionCandidate is WeaklyQuotedString weaklyQuotedString)
      {
        description = weaklyQuotedString.Value;
        return true;
      }
      description = null;
      return false;
    }
  }
}