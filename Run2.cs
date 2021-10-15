using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace Run2
{
  internal static class Run2
  {
    private static int MostRecentLineNumber { get; set; }

    public static void EnterScope()
    {
      Globals.Variables.EnterScope();
    }

    public static object Evaluate(object value)
    {
      if (value.IsAnyString(out var stringValue))
      {
        if (value is string && Globals.Variables.TryGetValue(stringValue, out var result))
        {
          return result;
        }
        foreach (var key in Globals.Variables.GetKeys())
        {
          stringValue = stringValue.Replace("[" + key + "]", Globals.Variables.Get(key)?.ToString());
        }
        return stringValue;
      }
      if (value is SubCommands subCommands)
      {
        return RunSubCommands(subCommands);
      }
      return value;
    }

    public static object GetCommands()
    {
      return string.Join('\n', Globals.Commands.Keys);
    }

    public static string GetNameFromToken(this object token)
    {
      switch (token)
      {
        case string stringValue:
          return stringValue;
        case TokensList tokensValue:
          return tokensValue.PeekString();
        default:
          false.Check("Could not get command-name");
          return "";
      }
    }

    public static void GetParameterInformation(this object token, out string name, out bool isOptional, out object defaultValue)
    {
      name = "";
      isOptional = false;
      defaultValue = null;
      switch (token)
      {
        case string stringValue:
          name = stringValue;
          isOptional = false;
          defaultValue = null;
          return;
        case TokensList tokensValue:
          (tokensValue.Count is >= 1 and <= 2).Check("The declaration of optional parameters must contain the parameter-name and optionally the default-value");
          name = tokensValue.PeekString();
          isOptional = true;
          defaultValue = tokensValue.ElementAtOrDefault(1);
          return;
        default:
          false.Check("Could not get command-name");
          break;
      }
    }

    public static object GetVariable(string name)
    {
      return Globals.Variables.Get(name);
    }

    public static void Initialize()
    {
      CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
      CreateStandardObject(new ArrayList());
      CreateStandardObject(new Stack());
      CreateStandardObject(new Hashtable());
      CreateStandardObject(new Queue());
      CreateStandardObject(new BigInteger());
      Globals.Variables.globalScopeCreated += OnGlobalScopeCreated;
      Globals.Debug = CommandLineParser.OptionExists("debug");
      Globals.BaseDirectory = CommandLineParser.GetOptionString("baseDirectory", Helpers.GetProgramDirectory());
      Globals.ScriptPathSystem = Helpers.LocateFile(Helpers.GetProgramDirectory(), Globals.ScriptNameSystem);
      Globals.ScriptName = CommandLineParser.GetOptionString("scriptName", Globals.ScriptNameDefault);
      Globals.ScriptPath = Helpers.LocateFile(Globals.BaseDirectory, Globals.ScriptName);
      Globals.ScriptDirectory = Path.GetDirectoryName(Globals.ScriptPath);
      Globals.Arguments = new TokensList(CommandLineParser.GetOptionStrings("scriptArguments", new List<string>()));
      File.Exists(Globals.ScriptPath).Check($"Could not find script '{Globals.ScriptName}' (base-directory: '{Globals.BaseDirectory}')");
      BuildSystemCommands();
      BuildInvokeCommands();
      if (File.Exists(Globals.ScriptPathSystem))
      {
        LoadCommand(Globals.ScriptPathSystem);
      }
      LoadCommand(Globals.ScriptPath);
      RunCommand(Helpers.GetCommandNameFromPath(Globals.ScriptPath), Globals.Arguments);
      Helpers.WriteLine("Script terminated successfully");
    }

    public static void LeaveScope()
    {
      Globals.Variables.LeaveScope();
    }

    public static object RunCommand(string name, TokensList arguments)
    {
      try
      {
        Globals.Commands.ContainsKey(name).Check($"Command '{name}' not found");
        if (Globals.Debug)
        {
          Helpers.WriteLine($"Begin '{name}'");
        }
        var result = Globals.Commands[name].Run(arguments.Clone());
        if (Globals.Debug)
        {
          Helpers.WriteLine($"End '{name}'");
        }
        return result;
      }
      catch (Exception exception)
      {
        if (exception is RuntimeException)
        {
          throw;
        }
        Helpers.HandleException(exception, MostRecentLineNumber);
        throw new RuntimeException("Runtime error");
      }
    }

    public static object RunSubCommands(SubCommands subCommands)
    {
      object result = null;
      foreach (var subCommand in subCommands)
      {
        MostRecentLineNumber = subCommand.Arguments.LineNumber;
        result = RunCommand(subCommand.CommandName, subCommand.Arguments);
        if (Globals.DoBreak)
        {
          break;
        }
      }
      return result;
    }

    public static void SetGlobalVariable(string name, object value)
    {
      Globals.Variables.SetGlobal(name, value);
    }

    public static void SetLocalVariable(string name, object value)
    {
      Globals.Variables.SetLocal(name, value);
    }

    public static void SetVariable(string name, object value)
    {
      Globals.Variables.Set(name, value);
    }

    public static bool TryGetVariable(string name, out object value)
    {
      return Globals.Variables.TryGetValue(name, out value);
    }

    private static bool AcceptMember(MemberInfo member)
    {
      return member switch
      {
        MethodInfo methodInfo => !methodInfo.IsSpecialName,
        PropertyInfo propertyInfo => !propertyInfo.IsSpecialName,
        FieldInfo => true,
        _ => false
      };
    }

    private static bool AcceptType(Type type)
    {
      return type.IsPublic && (type.IsClass || type.IsStruct()) && !type.IsNested && !type.IsGenericType && Globals.AcceptedTypes.Contains(type.Name);
    }

    private static void AddMember(Type type, MemberInfo member, BindingFlags bindingFlags, bool isStatic, bool isLocalType)
    {
      var key = isStatic ? type.Name + "." + member.Name : member.Name;
      if (!Globals.Commands.TryGetValue(key, out var command))
      {
        command = new InvokeCommand(member.Name, isStatic ? type : null);
        Globals.Commands.Add(key, command);
      }
      if (!isLocalType)
      {
        ((InvokeCommand) command).FullNames.Add(isStatic ? $"{type.FullName}.{member.Name}" : $"{type.BaseTypeOfMember(member.Name, bindingFlags).FullName}.{member.Name}");
      }
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
                  AddMember(type, member, bindingFlags, pass == 0, isLocalType);
                }
              }
            }
            if (0 < type.GetConstructors().Length)
            {
              var key = type.Name + ".new";
              if (!Globals.Commands.ContainsKey(key))
              {
                Globals.Commands.Add(key, new InvokeCommand("_new", type));
              }
            }
          }
        }
      }
    }

    private static void BuildSystemCommands()
    {
      foreach (var method in typeof(SystemCommands).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static))
      {
        var attribute = (DocumentationAttribute) Attribute.GetCustomAttribute(method, typeof(DocumentationAttribute));
        Globals.Commands.Add(attribute?.CommandName ?? method.Name.ToLower(), new SystemCommand((CommandAction) Delegate.CreateDelegate(typeof(CommandAction), method)));
      }
    }

    private static void BuildUserCommands(string path, TokensList tokens)
    {
      var firstCommandName = Path.GetFileNameWithoutExtension(path);
      var definitions = new Dictionary<object, TokensList>();
      object commandName = firstCommandName;
      var subtokens = new TokensList();
      var lineNumber = 0 < tokens.LineNumber ? tokens.LineNumber : 0;
      subtokens.LineNumber = lineNumber;
      while (0 < tokens.Count)
      {
        var token = tokens.Dequeue();
        if (token.ToString()?.Equals(Globals.PseudoCommandNameCommand, StringComparison.OrdinalIgnoreCase) == true)
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
        if (name.IsStrongQuote(out var unquotedName))
        {
          Globals.Commands.Add(unquotedName, new UserCommand { Name = unquotedName, IsQuoted = true, ScriptPath = path });
        }
        else
        {
          Globals.Commands.Add(name.GetNameFromToken(), new UserCommand { Name = name.GetNameFromToken(), ScriptPath = path });
        }
      }
      foreach (var (key, definitionTokens) in definitions)
      {
        var userCommand = Globals.Commands[key.GetNameFromToken().RemoveStrongQuotes()] as UserCommand;
        (userCommand != null).Check("User-command must not be null");
        if (TryPeekDocumentation(definitionTokens, out var description))
        {
          userCommand.Description = description;
          definitionTokens.Dequeue();
        }
        if (TryPeekDocumentation(definitionTokens, out var returns))
        {
          userCommand.Returns = returns;
          definitionTokens.Dequeue();
        }
        if (TryPeekDocumentation(definitionTokens, out var remarks))
        {
          userCommand.Remarks = remarks;
          definitionTokens.Dequeue();
        }
        while (0 < definitionTokens.Count)
        {
          var peekedToken = definitionTokens.Peek();
          var peekedTokenString = peekedToken.GetNameFromToken();
          if (Globals.Commands.ContainsKey(peekedTokenString))
          {
            break;
          }
          userCommand.ParameterNames.Add(peekedToken);
          definitionTokens.Dequeue();
          if (TryPeekDocumentation(definitionTokens, out var parameterDescription))
          {
            userCommand.ParameterDescriptions.Add(peekedTokenString, parameterDescription);
            definitionTokens.Dequeue();
          }
        }
        userCommand.SubCommands = GetSubCommands(definitionTokens, ref lineNumber);
      }
    }

    // ReSharper disable once UnusedParameter.Local
    private static void CreateStandardObject(object instance)
    {
    }

    private static void EnqueueToken(string currentTokenString, int lineNumber, TokensList result)
    {
      if (Helpers.IsWeakQuote(ref currentTokenString))
      {
        result.Enqueue(new WeakQuote(currentTokenString));
      }
      else if (Helpers.IsBlock(ref currentTokenString))
      {
        var tokens = GetTokens(currentTokenString, lineNumber);
        tokens.LineNumber = lineNumber;
        result.Enqueue(tokens);
      }
      else
      {
        result.Enqueue(currentTokenString.GetBestTypedObject());
      }
    }

    private static SubCommands GetSubCommands(TokensList tokens, ref int lineNumber)
    {
      var result = new SubCommands();
      lineNumber = 0 < tokens.LineNumber ? tokens.LineNumber : lineNumber;
      while (0 < tokens.Count)
      {
        var subCommand = new SubCommand { CommandName = tokens.DequeueString(false) };
        result.Add(subCommand);
        while (0 < tokens.Count)
        {
          if (tokens.Peek() is string peekedTokenString && Globals.Commands.ContainsKey(peekedTokenString))
          {
            break;
          }
          var token = tokens.Dequeue();
          if (token is TokensList tokensValue)
          {
            subCommand.Arguments.Enqueue(GetSubCommands(tokensValue, ref lineNumber));
          }
          else
          {
            subCommand.Arguments.Enqueue(token);
          }
        }
        if (subCommand.Arguments.LineNumber < 0)
        {
          subCommand.Arguments.LineNumber = lineNumber;
        }
        if (result.LineNumber < 0)
        {
          result.LineNumber = lineNumber;
        }
      }
      if (lineNumber == -1)
      {
        Helpers.WriteLine("Unexpected line number -1");
      }
      return result;
    }

    private static TokensList GetTokens(string code, int lineNumber)
    {
      var result = new TokensList();
      var tokenString = "";
      var blockLevel = 0;
      var expectedBlockEnders = new Stack<char>();
      var expectedBlockLevels = new Stack<int>();
      var characters = new Queue<char>(code);
      while (0 < characters.Count)
      {
        var currentCharacter = characters.Dequeue();
        if (currentCharacter == '\n')
        {
          ++lineNumber;
        }
        if (char.IsWhiteSpace(currentCharacter) && blockLevel == 0)
        {
          if (tokenString == "")
          {
            continue;
          }
          EnqueueToken(tokenString, lineNumber, result);
          tokenString = "";
        }
        else
        {
          switch (currentCharacter)
          {
            case Globals.BlockStart:
            {
              tokenString += currentCharacter;
              ++blockLevel;
              expectedBlockLevels.Push(blockLevel);
              expectedBlockEnders.Push(Globals.BlockEnd);
              break;
            }
            case Globals.BlockEnd:
            {
              var expectedBlockEnder = expectedBlockEnders.Pop();
              (expectedBlockEnder == Globals.BlockEnd).Check($"Expected block-ender: {expectedBlockEnder}");
              var expectedBlockLevel = expectedBlockLevels.Pop();
              (expectedBlockLevel == blockLevel).Check($"Expected block-level: {expectedBlockLevel}");
              --blockLevel;
              tokenString += currentCharacter;
              break;
            }
            case Globals.StrongQuote:
            case Globals.WeakQuote:
            {
              if (0 < expectedBlockEnders.Count && expectedBlockEnders.Peek() == currentCharacter)
              {
                expectedBlockEnders.Pop();
                var expectedBlockLevel = expectedBlockLevels.Pop();
                (expectedBlockLevel == blockLevel).Check($"Expected block-level: {expectedBlockLevel}");
                --blockLevel;
                if (currentCharacter is Globals.StrongQuote or Globals.WeakQuote || 0 < blockLevel)
                {
                  tokenString += currentCharacter;
                }
              }
              else
              {
                if (currentCharacter is Globals.StrongQuote or Globals.WeakQuote || 0 < blockLevel)
                {
                  tokenString += currentCharacter;
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
                  tokenString += '\n';
                  break;
                default:
                  tokenString += nextCharacter;
                  break;
              }
              break;
            default:
              tokenString += currentCharacter;
              break;
          }
        }
      }
      if (tokenString != "")
      {
        EnqueueToken(tokenString, lineNumber, result);
      }
      if (0 < blockLevel)
      {
        false.Check($"Unexpected end of script; number of unclosed blocks: {blockLevel}");
      }
      return result;
    }

    private static void LoadCommand(string path)
    {
      File.Exists(path).Check($"Script '{path}' not found");
      var tokens = GetTokens(File.ReadAllText(path), 0);
      BuildUserCommands(path, tokens);
    }

    private static void OnGlobalScopeCreated(object sender, EventArgs e)
    {
      SetGlobalVariable("commands", Globals.Commands);
      SetGlobalVariable("variables", Globals.Variables);
      SetGlobalVariable("scriptpath", Globals.ScriptPath);
      SetGlobalVariable("scriptdirectory", Globals.ScriptDirectory);
      SetGlobalVariable("basedirectory", Globals.BaseDirectory);
      SetGlobalVariable("programdirectory", Helpers.GetProgramDirectory());
      SetGlobalVariable("scriptpathsystem", Globals.ScriptPathSystem);
      SetGlobalVariable("verbositylevel", 5);
    }

    private static bool TryPeekDocumentation(TokensList tokens, out string description)
    {
      if (tokens.TryPeek(out var descriptionCandidate) && descriptionCandidate is WeakQuote weaklyQuotedString)
      {
        description = weaklyQuotedString.Value;
        return true;
      }
      description = null;
      return false;
    }
  }
}