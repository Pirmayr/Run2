using System;
using System.Collections;
using System.Collections.Generic;

namespace Run2
{
  public sealed class Variables
  {
    private readonly Stack<Scope> scopes = new();
    private Scope globalScope;

    public event EventHandler globalScopeCreated;

    public IEnumerable<string> Keys
    {
      get
      {
        var handled = new HashSet<string>();
        foreach (var scope in scopes)
        {
          foreach (var key in scope.Keys)
          {
            if (!handled.Contains(key))
            {
              handled.Add(key);
            }
          }
        }
        foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
        {
          handled.Add(environmentVariable.Key.ToString());
        }
        return handled;
      }
    }

    public int ScopesCount => scopes.Count;

    public void EnterScope()
    {
      var newScope = new Scope();
      scopes.Push(newScope);
      if (scopes.Count == 1)
      {
        globalScope = newScope;
        globalScopeCreated?.Invoke(this, EventArgs.Empty);
      }
    }

    public object Get(string name)
    {
      TryGetValue(name, out var result).Check("Could not find variable '{name}'");
      return result;
    }

    public object Get(string name, object defaultValue)
    {
      return TryGetValue(name, out var result) ? result : defaultValue;
    }

    public void LeaveScope()
    {
      scopes.Pop();
    }

    public void Set(string name, object value)
    {
      foreach (var scope in scopes)
      {
        if (scope.ContainsKey(name))
        {
          scope[name] = value;
          if (scope == globalScope && value is string stringValue)
          {
            Environment.SetEnvironmentVariable(name, stringValue);
          }
          return;
        }
      }
    }

    public void SetGlobal(string name, object value)
    {
      (0 < scopes.Count && globalScope != null).Check("Cannot set variable because there is no valid scope");
      globalScope[name] = value;
      if (value is string stringValue)
      {
        Environment.SetEnvironmentVariable(name, stringValue);
      }
    }

    public void SetLocal(string name, object value)
    {
      (0 < scopes.Count).Check("Cannot set variable because there is no valid scope");
      scopes.Peek()[name] = value;
    }

    public bool TryGetValue(string name, out object value)
    {
      if (0 < scopes.Count)
      {
        foreach (var scope in scopes)
        {
          if (scope.TryGetValue(name, out value))
          {
            return true;
          }
        }
        value = Environment.GetEnvironmentVariable(name);
        return value != null;
      }
      value = "";
      return false;
    }

    private sealed class Scope : Dictionary<string, object>
    {
    }
  }
}