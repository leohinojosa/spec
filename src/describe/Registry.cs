using System;
using System.Collections.Generic;
using spec.Model;

namespace spec
{
  public class Registry
  {
    private Definition _currentSuite = new Definition() { Description = "Root", Enabled = true};
    private readonly List<Specification>  _executableLookupTable = new List<Specification>();
    
    public string Source { get; set; }
    public Definition CurrentSuite
    {
      get { return _currentSuite; }
    }

    public List<Specification> ExecutableLookupTable
    {
      get { return _executableLookupTable; }
    }

    public  void AddSpecToSuites(Definition suite, Action specDefinition)
    {
      var parentSuite = _currentSuite;
      _currentSuite = suite;
      parentSuite.AddChild(_currentSuite);
      SafeExecute(specDefinition);
      _currentSuite = parentSuite;
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
        Parent = _currentSuite,
        Fn = fn
      };
    }
    public Specification SpecFactory(string description, Action function, Definition suite, string  codeBase, int lineNumber, int column, string fileName, string className)
    {
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

      _executableLookupTable.Add(spec);
      return spec;
    }
    public Hook EachFactory(string description, Action fnAction)
    {
      return new Hook() {Description = description, Fn = fnAction};
    }
  }
}