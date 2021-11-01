using System;

namespace Run2
{
  public static class Program
  {
    private static int Main()
    {
      try
      {
        Run2.RunScript();
        return 0;
      }
      catch (Exception exception)
      {
        Globals.HandleException(exception);
      }
      Globals.WriteLine("Script terminated with failure");
      return 1;
    }
  }
}