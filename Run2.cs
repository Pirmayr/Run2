using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Reflection;

namespace Run2
{
  public static class Run2
  {
    private static readonly HashSet<string> loadedScripts = new();

    private static HashSet<string> AcceptedTypes { get; } = new() { "Array", "ArrayList", "BigInteger", "Char", "CodeFormatter", "Console", "Convert", "DictionaryEntry", "Directory", "File", "Hashtable", "Globals", "Int32", "Math", "Path", "Queue", "String", "Stack", "SubCommands", "Items", "Variables", "Thread", "Interaction", "DateTime", "StringSplitOptions", "Activator", "Type", "HttpClient", "StringContent", "Encoding", "HttpResponseMessage", "Uri", "Exception" };

    public static object Evaluate(object value)
    {
      switch (value)
      {
        case string stringValue:
          if (Globals.Variables.TryGetValue(stringValue, out var result))
          {
            return result;
          }
          Globals.Variables.Keys.ForEach(item => stringValue = stringValue.Replace("[" + item + "]", Globals.Variables.Get(item)?.ToString()));
          return stringValue;
        case Globals.SubCommands subCommands:
          return ExecuteSubCommands(subCommands);
      }
      return value;
    }

    public static object ExecuteCommand(string name, Items arguments)
    {
      var previousDequeueIndex = arguments.DequeueIndex;
      var previousScriptPath = Globals.CurrentScriptPath;
      var previousLineNumber = Globals.CurrentLineNumber;
      name.SetCurrentScriptPathAndLineNumber();
      try
      {
        Globals.Commands.ContainsKey(name).Check($"Command '{name}' not found");
        if (Globals.Debug)
        {
          Globals.WriteLine($"Begin '{name}'");
        }
        arguments.DequeueIndex = 0;
        var command = Globals.Commands[name];
        command.SetCurrentScriptPathAndLineNumber();
        var result = command.Run(arguments);
        if (Globals.Debug)
        {
          Globals.WriteLine($"End '{name}'");
        }
        return result;
      }
      catch (Exception exception)
      {
        if (0 < Globals.TryCatchFinallyLevel || exception is Globals.RuntimeException)
        {
          throw;
        }
        Globals.HandleException(exception, Globals.CurrentScriptPath, Globals.CurrentLineNumber);
        throw new Globals.RuntimeException("Runtime error");
      }
      finally
      {
        Globals.CurrentLineNumber = previousLineNumber;
        Globals.CurrentScriptPath = previousScriptPath;
        arguments.DequeueIndex = previousDequeueIndex;
      }
    }

    public static object ExecuteSubCommands(Globals.SubCommands subCommands)
    {
      object result = null;
      foreach (Globals.SubCommand subCommand in subCommands)
      {
        result = ExecuteCommand(subCommand.CommandName, subCommand.Arguments);
        if (Globals.DoBreak)
        {
          break;
        }
      }
      return result;
    }

    public static string GetNameFromItem(this object item)
    {
      switch (item)
      {
        case string stringValue:
          return stringValue;
        case Items itemsValue:
          return itemsValue.PeekString();
        default:
          false.Check("Could not get command-name");
          return "";
      }
    }

    public static void GetParameterInformation(this object item, out string name, out bool isOptional, out object defaultValue)
    {
      name = "";
      isOptional = false;
      defaultValue = null;
      name = GetNameFromItem(item);
      switch (item)
      {
        case string:
          isOptional = false;
          defaultValue = null;
          return;
        case Items itemsValue:
          (itemsValue.QueueCount is >= 1 and <= 2).Check("The declaration of optional parameters must contain the parameter-name and optionally the default-value");
          isOptional = true;
          defaultValue = itemsValue.ElementAtOrDefault(1);
          return;
        default:
          false.Check("Could not get command-name");
          break;
      }
    }

    public static void LoadScript(string scriptNameOrPath, Items arguments = null)
    {
      var scriptPath = scriptNameOrPath;
      if (!File.Exists(scriptPath))
      {
        var scriptFileName = $"{scriptNameOrPath}.{Globals.DefaultExtension}";
        scriptPath = Globals.LocateFile(Globals.ProgramDirectory, scriptFileName);
        if (string.IsNullOrEmpty(scriptPath))
        {
          scriptPath = Globals.LocateFile(Globals.BaseDirectory, scriptFileName);
        }
      }
      if (!loadedScripts.Contains(scriptPath))
      {
        loadedScripts.Add(scriptPath);
        File.Exists(scriptPath).Check($"Script '{scriptNameOrPath}' not found");
        Parser.Parse(Scanner.Scan(scriptPath));
        var commandName = Path.GetFileNameWithoutExtension(scriptPath);
        if (Globals.Commands.ContainsKey(commandName))
        {
          ExecuteCommand(commandName, arguments ?? new Items());
        }
      }
    }

    public static void RunScript()
    {
      try
      {
        LoadType("Microsoft.VisualBasic.Interaction, Microsoft.VisualBasic.Core");
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CreateStandardObject(new ArrayList());
        CreateStandardObject(new BigInteger());
        CreateStandardObject(new Hashtable());
        CreateStandardObject(new Queue());
        CreateStandardObject(new Stack());
        CreateStandardObject(new HttpClient());
        CreateStandardObject(new Uri("http://google.com"));
        Globals.Variables.globalScopeCreated += OnGlobalScopeCreated;
        Globals.ScriptArguments = new Items(CommandLineParser.GetOptionStrings("scriptArguments"));
        Globals.Debug = CommandLineParser.OptionExists("debug");
        Globals.ProgramDirectory = Path.GetDirectoryName(AppContext.BaseDirectory);
        Globals.BaseDirectory = CommandLineParser.GetOptionString("baseDirectory", Globals.ProgramDirectory);
        Globals.UserScriptFilename = CommandLineParser.GetOptionString("scriptName", Globals.DefaultUserScriptFilename);
        Globals.UserScriptPath = Globals.LocateFile(Globals.BaseDirectory, Globals.UserScriptFilename);
        File.Exists(Globals.UserScriptPath).Check($"Could not find script '{Globals.UserScriptFilename}' (base-directory: '{Globals.BaseDirectory}')");
        Globals.Variables.EnterScope();
        BuildSystemCommands();
        BuildInvokeCommands();
        LoadScript(Globals.SystemScriptName);
        LoadScript(Globals.UserScriptPath, Globals.ScriptArguments);
        Globals.WriteLine("Script terminated successfully");
      }
      finally
      {
        if (0 < Globals.Variables.ScopesCount)
        {
          Globals.Variables.LeaveScope();
        }
      }
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
      return type.IsPublic && (type.IsClass || type.IsEnum || type.IsStruct()) && !type.IsNested && !type.IsGenericType && AcceptedTypes.Contains(type.Name);
    }

    private static Type BaseTypeOfMember(this Type type, string memberName, BindingFlags bindingFlags)
    {
      Type previousResult = null;
      var result = type;
      while (previousResult != result && result != null && result.Name != "ValueType" && result.Name != "Enum" && result.BaseType?.Name != "Object" && result.BaseType != null && result.Name != result.BaseType.Name && 0 < result.BaseType.GetMember(memberName, bindingFlags).Length)
      {
        var baseType = type.BaseType;
        previousResult = result;
        result = baseType;
      }
      return result;
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
            for (var pass = 0; pass < 2; ++pass)
            {
              var bindingFlags = BindingFlags.Public;
              bindingFlags |= pass == 0 ? BindingFlags.Static : BindingFlags.Instance;
              foreach (var member in type.GetMembers(bindingFlags))
              {
                if (AcceptMember(member))
                {
                  var isStatic = pass == 0;
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
        Globals.Commands.Add(attribute?.CommandName ?? method.Name.ToLower(), new SystemCommand((Globals.CommandAction) Delegate.CreateDelegate(typeof(Globals.CommandAction), method)));
      }
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private static void CreateStandardObject(object instance)
    {
    }

    private static bool IsStruct(this Type type)
    {
      return type.IsValueType && !type.IsEnum;
    }

    private static void LoadType(string fullQualifiedTypeName)
    {
      (Type.GetType(fullQualifiedTypeName) != null).Check($"Could not load type {fullQualifiedTypeName}");
    }

    private static void OnGlobalScopeCreated(object sender, EventArgs e)
    {
      Globals.Variables.SetGlobal("commands", Globals.Commands);
      Globals.Variables.SetGlobal("variables", Globals.Variables);
      Globals.Variables.SetGlobal("userscriptpath", Globals.UserScriptPath);
      Globals.Variables.SetGlobal("userscriptdirectory", Globals.UserScriptDirectory);
      Globals.Variables.SetGlobal("basedirectory", Globals.BaseDirectory);
      Globals.Variables.SetGlobal("programdirectory", Globals.ProgramDirectory);
      Globals.Variables.SetGlobal("verbositylevel", 5);
    }
  }
}