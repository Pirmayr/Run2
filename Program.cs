using System;

namespace Run2
{
  public static class Program
  {
    private static int Main()
    {
      try
      {
        // Helpers.Test();
        Run2.RunScript();
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