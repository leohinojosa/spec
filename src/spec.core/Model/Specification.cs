using System;

namespace spec.Model
{
  [Serializable]
  public class Specification : Definition
  {
    
    public string CodeBase { get; set; }
    public int LineNumber { get; set; }
    public int Column { get; set; }
    public string FileName { get; set; }
    public string ClassName { get; set; }

    public override string ToString()
    {
      return Description + " " + Parent.Description;
    }
  }
}