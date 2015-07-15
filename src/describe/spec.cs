using System;
using System.Diagnostics;
using System.IO;
using spec.Model;

namespace spec
{
  public abstract class Spec : ISpec
  {
    public  Registry Registry { get; set; }
    public SpecType Type { get; set; }

    protected Spec() 
    {
      Registry = new Registry();
    }

    public void afterAll(string description, Action operation)
    {
      var before = Registry.EachFactory(description, operation);
      Registry.currentSuite.AddAfterAll(before);
    }

    public void afterEach(Action operation)
    {
      beforeEach("after Each", operation);
    }

    public void afterEach(string description, Action operation)
    {
      var after = Registry.EachFactory(description, operation);
      Registry.currentSuite.AddAfterEach(after);
    }

    public void beforeAll(string description, Action operation)
    {
      var before = Registry.EachFactory(description, operation);
      Registry.currentSuite.AddBeforeAll(before);
    }

    public void beforeEach(Action operation)
    {
      beforeEach("before Each", operation);
    }

    public void beforeEach(string description, Action operation)
    {
      var before = Registry.EachFactory(description, operation);
      Registry.currentSuite.AddBeforeEach(before);
    }

    public void context(string name, Action operation)
    {
      addSuite("Context:" + name, operation, true, SpecType.describe);
    }

    public void describe(string name, Action<object> operation)
    {
      //this guys allows calling it in this fashion
      // ( _=>{   #content# })
      describe(name, () => operation(this));
    }

    public void describe(string name, Action operation)
    {
      addSuite(name, operation, true, SpecType.describe);
    }

    public void it(string name, Action operation)
    {
      addSpec(name, operation, true, SpecType.it);
    }

    public void xdescribe(string name, Action<object> operation)
    {
      //this guys allows calling it in this fashion
      // ( _=>{   #content# })
      xdescribe(name, () => operation(this));
    }

    public void xdescribe(string name, Action operation)
    {
      addSuite(name, operation, false, SpecType.xdescribe);
    }

    public void xit(string name, Action operation)
    {
      addSpec(name, operation, false, SpecType.xit);
    }

    private Suite addSuite(string name, Action specDefinition, bool enabled, SpecType specType)
    {
      var suite = Registry.SuiteFactory(name, specDefinition);
      suite.Enabled = enabled;
      Registry.AddSpecToSuites(suite, specDefinition);
      return suite;
    }

    private void addSpec(string name, Action operation, bool enabled, SpecType specType)
    {
      var stackFrame = new System.Diagnostics.StackTrace(true).GetFrame(2);
      var codeBase = stackFrame.GetFileName();
      var lineNumber = stackFrame.GetFileLineNumber();
      var columnNumber = stackFrame.GetFileColumnNumber();
     // var fileName = Path.GetFileNameWithoutExtension(codeBase);
      var fileName = Path.GetFileName(codeBase);
      var className = this.GetType().FullName;

      var spec = Registry.SpecFactory(name, operation, Registry.currentSuite, codeBase, lineNumber, columnNumber, fileName, className);
      spec.Enabled = enabled && spec.Parent.Enabled;
      Registry.currentSuite.AddChild(spec);
    }
  }
}