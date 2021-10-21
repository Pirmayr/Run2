﻿using System;
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
  public static class Run2
  {
    private static HashSet<string> AcceptedTypes { get; } = new() { "Array", "ArrayList", "BigInteger", "Char", "CodeFormatter", "Console", "Convert", "DictionaryEntry", "Directory", "File", "Hashtable", "Helpers", "Int32", "Math", "Path", "Queue", "String", "Stack", "SubCommands", "Items", "Variables" };

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
        case SubCommands subCommands:
          return ExecuteSubCommands(subCommands);
      }
      return value;
    }

    public static object ExecuteCommand(string name, Items arguments)
    {
      if (name == "foreachdirectorypair")
      {
        int a = 0;
      }
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
        Helpers.HandleException(exception, name.GetProperties().LineNumber);
        throw new RuntimeException("Runtime error");
      }
      finally
      {
        arguments.DequeueIndex = oldDequeueIndex;
      }
    }

    public static object ExecuteSubCommands(SubCommands subCommands)
    {
      object result = null;
      foreach (SubCommand subCommand in subCommands)
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

    public static void RunScript()
    {
      CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
      CreateStandardObject(new ArrayList());
      CreateStandardObject(new BigInteger());
      CreateStandardObject(new Hashtable());
      CreateStandardObject(new Queue());
      CreateStandardObject(new Stack());
      Globals.Variables.globalScopeCreated += OnGlobalScopeCreated;
      Globals.Debug = CommandLineParser.OptionExists("debug");
      Globals.Arguments = new Items(CommandLineParser.GetOptionStrings("scriptArguments"));
      Globals.ProgramDirectory = Path.GetDirectoryName(AppContext.BaseDirectory);
      Globals.BaseDirectory = CommandLineParser.GetOptionString("baseDirectory", Globals.ProgramDirectory);
      Globals.SystemScriptPath = Helpers.LocateFile(Globals.ProgramDirectory, Globals.SystemScriptFilename);
      Globals.UserScriptFilename = CommandLineParser.GetOptionString("scriptName", Globals.DefaultUserScriptFilename);
      Globals.UserScriptPath = Helpers.LocateFile(Globals.BaseDirectory, Globals.UserScriptFilename);
      File.Exists(Globals.UserScriptPath).Check($"Could not find script '{Globals.UserScriptFilename}' (base-directory: '{Globals.BaseDirectory}')");
      BuildSystemCommands();
      BuildInvokeCommands();
      if (File.Exists(Globals.SystemScriptPath))
      {
        LoadScript(Globals.SystemScriptPath);
      }
      LoadScript(Globals.UserScriptPath);
      ExecuteCommand(Globals.UserScriptName, Globals.Arguments);
      Helpers.WriteLine("Script terminated successfully");
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
      return type.IsPublic && (type.IsClass || type.IsStruct()) && !type.IsNested && !type.IsGenericType && AcceptedTypes.Contains(type.Name);
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
        Globals.Commands.Add(attribute?.CommandName ?? method.Name.ToLower(), new SystemCommand((CommandAction) Delegate.CreateDelegate(typeof(CommandAction), method)));
      }
    }

    // ReSharper disable once UnusedParameter.Local
    private static void CreateStandardObject(object instance)
    {
    }

    private static bool IsStruct(this Type type)
    {
      return type.IsValueType && !type.IsEnum;
    }

    private static void LoadScript(string path)
    {
      File.Exists(path).Check($"Script '{path}' not found");
      Parser.Parse(Scanner.Scan(path));
    }

    private static void OnGlobalScopeCreated(object sender, EventArgs e)
    {
      Globals.Variables.SetGlobal("commands", Globals.Commands);
      Globals.Variables.SetGlobal("variables", Globals.Variables);
      Globals.Variables.SetGlobal("userscriptpath", Globals.UserScriptPath);
      var value = Globals.UserScriptDirectory;
      Globals.Variables.SetGlobal("userscriptdirectory", value);
      Globals.Variables.SetGlobal("basedirectory", Globals.BaseDirectory);
      Globals.Variables.SetGlobal("programdirectory", Globals.ProgramDirectory);
      Globals.Variables.SetGlobal("systemscriptpath", Globals.SystemScriptPath);
      Globals.Variables.SetGlobal("verbositylevel", 5);
    }
  }
}