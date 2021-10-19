using System.Collections.Generic;

namespace Run2
{
  public sealed class CharacterQueue : Queue<char>
  {
    public CharacterQueue(string text) : base(text)
    {
    }
  }
}