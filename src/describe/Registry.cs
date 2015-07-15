using System;
using System.Collections.Generic;
using spec.Model;

namespace spec
{
  public class Registry
  {
    public  Definition currentSuite = new Definition() { Description = "Root" };
    public List<Specification>  executableLookupTable = new List<Specification>();
    public int totalSpecsDefined = 0;
    public string Source { get; set; }

    public  void AddSpecToSuites(Definition suite, Action specDefinition)
    {
      var parentSuite = currentSuite;
      currentSuite = suite;
      parentSuite.AddChild(currentSuite);
      SafeExecute(specDefinition);
      currentSuite = parentSuite;
    }

    private void SafeExecute(Action specDefinition)
    {
      try
      {
        specDefinition();
      }
      catch (Exception ex)
      {
        throw new Exception(String.Format("Cannot build specification\n {0}",ex.Message) );
      }
    }

    public Suite SuiteFactory(string description, Action fn)
    {
      return new Suite()
      {
        Description = description,
        Parent = currentSuite,
        Fn = fn
      };
    }
    public Specification SpecFactory(string description, Action function, Definition suite, string  codeBase, int lineNumber, int column, string fileName, string className)
    {
      totalSpecsDefined ++;
      var spec = new Specification()
      {
        Id = String.Format("spec://{0}/{1}:{2}", className, lineNumber, column),
        Description = description,
        Fn = function,
        Parent = suite,
        CodeBase = codeBase,
        LineNumber = lineNumber,
        Column = column,
        FileName = fileName,
        ClassName = className
      };

      executableLookupTable.Add(spec);
      return spec;
    }
    public Each EachFactory(string description, Action fnAction)
    {
      return new Each() {Description = description, Fn = fnAction};
    }
  }
}