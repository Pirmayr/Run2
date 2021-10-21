using System.Collections.Generic;

namespace Run2
{
  public class List : List<object>
  {
    public List(IEnumerable<object> values) : base(values)
    {
    }

    public List()
    {
    }
  }
}