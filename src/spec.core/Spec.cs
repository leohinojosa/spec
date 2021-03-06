using System;
using System.IO;
using spec.core.Model;

namespace spec.core
{
  public abstract class Spec : ISpec
  {
    public Registry Registry { get; set; }
    public SpecType Type { get; set; }

    protected Spec()
    {
      Registry = new Registry();
    }

    public void afterAll(string description, Action operation)
    {
      var before = Registry.AllFactory(description, operation, Registry.CurrentSuite);
        before.Enabled = before.Parent?.Enabled ?? true;
            before.Kind = GlobalHookKind.AfterAll;
            Registry.CurrentSuite.AddAfterAll(before);
    }

    public void afterAll(Action operation)
    {
      afterAll("after all", operation);
    }

    public void afterEach(Action operation)
    {
      afterEach("after Each", operation);
    }

    public void afterEach(string description, Action operation)
    {
      var after = Registry.EachFactory(description, operation, Registry.CurrentSuite);
            after.Enabled = after.Parent?.Enabled ?? true;
            Registry.CurrentSuite.AddAfterEach(after);
    }

    public void beforeAll(string description, Action operation)
    {
      var before = Registry.AllFactory(description, operation, Registry.CurrentSuite);
            before.Kind = GlobalHookKind.BeforeAll;
            before.Enabled = before.Parent?.Enabled??true;
            Registry.CurrentSuite.AddBeforeAll(before);
    }

    public void beforeAll(Action operation)
    {
      beforeAll("before All", operation);
    }

    public void beforeEach(Action operation)
    {
      beforeEach("before Each", operation);
    }

    public void beforeEach(string description, Action operation)
    {
      var before = Registry.EachFactory(description, operation, Registry.CurrentSuite);
            before.Enabled = before.Parent?.Enabled??true;
            Registry.CurrentSuite.AddBeforeEach(before);
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

     /// <summary>
     /// It currently fails in the visual studio unit test client.
     /// </summary>
     /// <param name="name"></param>
    public void it(string name)
    {
        addSpec(name, null, false, SpecType.xit);
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
      if (Registry.CurrentSuite.ExecutionStatus == ExecStatus.Running)
      {
        throw new Exception("Cannot add Suite in Spec block, please push this block to a parent Suite");
      }

      var suite = Registry.SuiteFactory(name, specDefinition);
      suite.Enabled = enabled && suite.Parent.Enabled;
      Registry.AddSpecToSuites(suite, specDefinition);
      return suite;
    }

      private void addSpec(string name, Action operation, bool enabled, SpecType specType)
      {
          if (name == String.Empty)
          {
              throw new Exception("Please specify a name for the spec");
          }
          var index = 2;
          var stackFrame = new System.Diagnostics.StackTrace(true).GetFrame(index);
          var codeBase = stackFrame.GetFileName();
          var lineNumber = stackFrame.GetFileLineNumber();
          var columnNumber = stackFrame.GetFileColumnNumber();
          var fileName = Path.GetFileName(codeBase);
          var className = this.GetType().FullName;
          //var className = this.GetType().Name;

          var spec = Registry.SpecFactory(name, operation, Registry.CurrentSuite, codeBase, lineNumber, columnNumber,
              fileName, className);
          spec.Enabled = enabled && spec.Parent.Enabled;
          Registry.CurrentSuite.AddChild(spec);
      }
  }
}