using System;
using System.Collections.Generic;

namespace Run2
{
  public sealed class Variables
  {
    private readonly Stack<Scope> scopes = new();
    private Scope globalScope;

    public event EventHandler globalScopeCreated;

    public void EnterScope()
    {
      var newScope = new Scope();
      if (scopes.Count == 0)
      {
        globalScope = newScope;
        globalScopeCreated?.Invoke(this, EventArgs.Empty);
      }
      scopes.Push(newScope);
    }

    public object Get(string name)
    {
      TryGetValue(name, out var result).Check("Could not find variable '{name}'");
      return result;
    }

    public IEnumerable<string> GetKeys()
    {
      var handled = new HashSet<string>();
      foreach (var scope in scopes)
      {
        foreach (var key in scope.Keys)
        {
          if (!handled.Contains(key))
          {
            handled.Add(key);
            // yield return key;
          }
        }
      }
      return handled;
    }

    public void LeaveScope()
    {
      scopes.Pop();
    }

    public void SetGlobal(string name, object value)
    {
      globalScope[name] = value;
    }

    public void SetLocal(string name, object value)
    {
      scopes.Peek()[name] = value;
    }

    public bool TryGetValue(string name, out object value)
    {
      foreach (var scope in scopes)
      {
        if (scope.TryGetValue(name, out value))
        {
          return true;
        }
      }
      value = null;
      return false;
    }

    private sealed class Scope : Dictionary<string, object>
    {
    }
  }
}