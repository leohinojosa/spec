using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace spec
{
  public class SuiteRegistry
  {
    public  Definition currentDeclarationSuite = new Definition() { Description = "Root" };
    public List<Specification>  runnableLookupTable = new List<Specification>();
    public int totalSpecsDefined = 0;
    public string Source { get; set; }

    public  void AddSpecToSuites(Definition suite, Action specDefinition)
    {
      var parentSuite = currentDeclarationSuite;
      currentDeclarationSuite = suite;
      parentSuite.AddChild(currentDeclarationSuite);
      SafeExecute(specDefinition);
      currentDeclarationSuite = parentSuite;
    }

    private void SafeExecute(Action specDefinition)
    {
      try
      {
        specDefinition();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        throw new Exception("Cannot build specification");
      }
    }

    public Suite SuiteFactory(string description, Action fn)
    {
      return new Suite()
      {
        Description = description,
        Parent = currentDeclarationSuite,
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

      runnableLookupTable.Add(spec);
      return spec;
    }
    public Each EachFactory(string description, Action fnAction)
    {
      return new Each() {Description = description, Fn = fnAction};
    }
  }

  public enum ExecStatus
  {
    NotRun,
    Running,
    Completed
  }

  [Serializable]
  public class Definition
  {
    public Definition Parent { get; set; }
    public List<Each> BeforeEach { get; private set; }
    public List<Each> AfterEach { get; private set; }
    public List<Each> BeforeAll { get; private set; }
    public List<Each> AfterAll { get; private set; }
    public List<Definition> Childs { get; set; }

    public string Id { get; set; }
    public string Description { get; set; }
    public Action Fn { get; set; }
    public bool Enabled { get; set; }

    public ExecStatus ExecutionStatus { get; set; }
    public bool RanSuccesfully { get; set; }
    public string ExecutionResult { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime StartTime { get; set; }

    public Definition ()
    {
      Childs = new List<Definition>();
      BeforeEach = new List<Each>();
      AfterEach = new List<Each>();

      BeforeAll = new List<Each>();
      AfterAll = new List<Each>();
    }
    public void AddChild(Definition spec)
    {
      Childs.Add(spec);
    }

    public void AddBeforeEach(Each each)
    {
      this.BeforeEach.Add(each);
    }
    public void AddAfterEach(Each each)
    {
      this.AfterEach.Add(each);
    }
    public void AddBeforeAll(Each each)
    {
      this.BeforeAll.Add(each);
    }
    public void AddAfterAll(Each each)
    {
      this.AfterAll.Add(each);
    }

    public override string ToString()
    {
      return Description;
    }
  }

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

  [Serializable]
  public class Suite : Definition
  {
   
  }

  [Serializable]
  public class Each : Definition
  {

  }

  public enum SpecType
  {
    describe,
    context,
    it,
    beforeEach,
    afterEach,
    beforeAll,
    afterAll,
    xdescribe,
    xit,
  }
}