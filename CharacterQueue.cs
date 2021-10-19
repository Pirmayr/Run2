using System.Collections.Generic;

namespace Run2
{
  public class CharacterQueue : Queue<char>
  {
    public CharacterQueue(string text) : base(text)
    {
    }
  }
}