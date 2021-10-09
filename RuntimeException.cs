using System;

namespace Run2
{
  internal sealed class RuntimeException : Exception
  {
    public RuntimeException(string message) : base(message)
    {
    }
  }
}