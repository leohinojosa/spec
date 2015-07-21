using System;
using System.Reflection;

namespace spec.Model
{
  [Serializable]
  public class DefinitionSource
  {
    public string Id { get; set; }
    public string Description { get; set; }
    public bool Enabled { get; set; }

    public bool RanSuccesfully { get; set; }
    public string ExecutionResult { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime StartTime { get; set; }

    public string CodeBase { get; set; }
    public int LineNumber { get; set; }
    public int Column { get; set; }
    public string FileName { get; set; }
    public string ClassName { get; set; }
    public string ParentDescription { get; set; }

    public static DefinitionSource CreateSource(Specification Specification)
    {
      var result = new DefinitionSource();
      result.Id = Specification.Id;

      result.Id = Specification.Id;
      result.ParentDescription = Specification.Parent.Description;
      result.Description = Specification.Description;
      result.Enabled = Specification.Enabled;
      result.RanSuccesfully = Specification.RanSuccesfully;
      result.ExecutionResult = Specification.ExecutionResult;
      result.EndTime = Specification.EndTime;
      result.StartTime = Specification.StartTime;
      result.CodeBase = Specification.CodeBase;
      result.LineNumber = Specification.LineNumber;
      result.Column = Specification.Column;
      result.FileName = Specification.FileName;
      result.ClassName = Specification.ClassName;

      return result;
    }
  }
}