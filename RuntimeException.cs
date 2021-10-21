using System;

namespace Run2
{
  public sealed class RuntimeException : Exception
  {
    public RuntimeException(string message) : base(message)
    {
    }
  }
}