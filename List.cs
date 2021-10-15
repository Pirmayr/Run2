using System.Collections.Generic;

namespace Run2
{
  public class List : List<object>
  {
    internal List(IEnumerable<object> values) : base(values)
    {
    }

    internal List()
    {
    }
  }
}