using System;

namespace Run2
{
  internal static class Program
  {
    private static int Main()
    {
      try
      {
        Run2.Initialize();
        return 0;
      }
      catch (Exception exception)
      {
        Helpers.HandleException(exception);
      }
      Helpers.WriteLine("Script terminated with failure");
      return 1;
    }
  }
}