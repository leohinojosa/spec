using System;
using System.Collections.Generic;
using System.Linq;
using spec.core.Model;

namespace spec.core
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
        throw new Exception($"Cannot build specification\n {ex.Message}");
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
        Id = $"spec://{className}/{lineNumber}:{column}",
       // Id = suite.Parent.Description + ".spec" + string.Format("{0}x{1}", lineNumber, column),  /* it.Id*/
        Description = description,
        Fn = function,
        Parent = suite,
        CodeBase = codeBase,
        LineNumber = lineNumber,
        Column = column,
        FileName = fileName,
        ClassName = className
      };

        if(_executableLookupTable.Any(x=>x.Id == spec.Id))
        {
            var t = (from p in _executableLookupTable
                where p.Id.StartsWith(spec.Id)
                select p).Count();
            spec.Id = spec.Id + string.Format("({0})", t);
        }
    
      _executableLookupTable.Add(spec);
      return spec;
    }
    public Hook EachFactory(string description, Action fnAction, Definition suite)
    {
      return new Hook() {Description = description, Fn = fnAction, Enabled = true, Parent = suite};
    }
    public GlobalHook AllFactory(string description, Action fnAction, Definition suite)
    {
      return new GlobalHook() {Description = description, Fn = fnAction, Enabled = true, Parent = suite};
    }
  }
}