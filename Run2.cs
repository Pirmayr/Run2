using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Run2
{
  internal static class Run2
  {
    private static ConditionalWeakTable<object, Properties> Properties { get; } = new();

    public static void AddProperties(this object value, Properties properties)
    {
      if (properties != null)
      {
        if (!Properties.TryGetValue(value, out _))
        {
          Properties.AddOrUpdate(value, properties);
        }
      }
    }

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
          Helpers.WriteLine($"Starting process '{executablePath}':");
          Helpers.WriteLine($"  Working-directory '{workingDirectory}'");
          Helpers.WriteLine($"  Arguments '{arguments}' ...");
          process.Start();
          output = process.StandardOutput.ReadToEnd().Trim();
          error = process.StandardError.ReadToEnd();
          process.WaitForExit(processTimeout).Check($"Process '{executablePath}' has timed out");
          Helpers.WriteLine($"Process '{executablePath}' terminated");
          (minimalExitCode == process.ExitCode && process.ExitCode <= maximalExitCode).Check($"The exit-code {process.ExitCode} lies not between the allowed range of {minimalExitCode} to {maximalExitCode}");
          return;
        }
        catch (Exception exception)
        {
          mostRecentException = exception;
          Helpers.HandleException(exception);
        }
        Thread.Sleep(5000);
      }
      throw mostRecentException;
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
        case Items tokensValue:
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
        case Items tokensValue:
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

    public static Properties GetProperties(this object value)
    {
      return Properties.GetOrCreateValue(value);
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
      Globals.Arguments = new Items(CommandLineParser.GetOptionStrings("scriptArguments", new List<string>()));
      File.Exists(Globals.ScriptPath).Check($"Could not find script '{Globals.ScriptName}' (base-directory: '{Globals.BaseDirectory}')");
      BuildSystemCommands();
      BuildInvokeCommands();
      if (File.Exists(Globals.ScriptPathSystem))
      {
        LoadCommand(Globals.ScriptPathSystem);
      }
      LoadCommand(Globals.ScriptPath);
      RunCommand(GetCommandNameFromPath(Globals.ScriptPath), Globals.Arguments);
      Helpers.WriteLine("Script terminated successfully");
    }

    public static void LeaveScope()
    {
      Globals.Variables.LeaveScope();
    }

    public static object RunCommand(string name, Items arguments)
    {
      var oldDequeueIndex = arguments.DequeueIndex;
      try
      {
        Globals.Commands.ContainsKey(name).Check($"Command '{name}' not found");
        if (Globals.Debug)
        {
          Helpers.WriteLine($"Begin '{name}'");
        }
        arguments.DequeueIndex = 0;
        var result = Globals.Commands[name].Run(arguments);
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
        var properties = name.GetProperties();
        Helpers.HandleException(exception, name.GetProperties().LineNumber);
        throw new RuntimeException("Runtime error");
      }
      finally
      {
        arguments.DequeueIndex = oldDequeueIndex;
      }
    }

    public static object RunSubCommands(SubCommands subCommands)
    {
      object result = null;
      foreach (SubCommand subCommand in subCommands)
      {
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

    private static Type BaseTypeOfMember(this Type type, string memberName, BindingFlags bindingFlags)
    {
      var result = type;
      while (result != null && result.Name != "ValueType" && result.BaseType != null && result.Name != result.BaseType.Name && 0 < result.BaseType.GetMember(memberName, bindingFlags).Length)
      {
        var baseType = type.BaseType;
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

    // ReSharper disable once UnusedParameter.Local
    private static void CreateStandardObject(object instance)
    {
    }

    private static string GetCommandNameFromPath(string path)
    {
      return Path.GetFileNameWithoutExtension(path);
    }

    private static bool IsStruct(this Type type)
    {
      return type.IsValueType && !type.IsEnum;
    }

    private static void LoadCommand(string path)
    {
      File.Exists(path).Check($"Script '{path}' not found");
      Parser.Parse(Scanner.Scan(path));
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
  }
}