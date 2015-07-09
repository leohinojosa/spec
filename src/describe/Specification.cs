using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace describe
{
  public abstract class Specification
  {
    public  SuiteRegistry Registry { get; set; }

    protected Specification() 
    {
      Registry = new SuiteRegistry();
    }
    public SpecType Type { get; set; }

    protected void describe(string name, Action<object> operation)
    {
      //this guys allows calling it in this fashion
      // ( _=>{   #content# })
      describe("before Each", () => operation(this));
    }

    protected void beforeEach(Action operation)
    {
      beforeEach("before Each", operation);
    }

    protected void beforeAll(string description, Action operation)
    {
      var before = Registry.EachFactory(description, operation);
      Registry.currentDeclarationSuite.AddBeforeAll(before);
    }

    protected void afterAll(string description, Action operation)
    {
      var before = Registry.EachFactory(description, operation);
      Registry.currentDeclarationSuite.AddAfterAll(before);
    }

    protected void beforeEach(string description, Action operation)
    {
      var before = Registry.EachFactory(description, operation);
      Registry.currentDeclarationSuite.AddBeforeEach(before);
    }

    protected void afterEach(Action operation)
    {
      beforeEach("after Each", operation);
    }

    protected void afterEach(string description, Action operation)
    {
      var after = Registry.EachFactory(description, operation);
      Registry.currentDeclarationSuite.AddAfterEach(after);
    }

    protected void context(string name, Action operation)
    {
      addSuite(name, operation, true, SpecType.describe);
    }

    protected void describe(string name, Action operation)
    {
      addSuite(name, operation, true, SpecType.describe);
    }

    protected void xdescribe(string name, Action operation)
    {
      addSuite(name, operation, false, SpecType.xdescribe);
    }

    protected void it(string name, Action operation)
    {
      addSpec(name, operation, true, SpecType.it);
    }

    protected void xit(string name, Action operation)
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
      var spec = Registry.SpecFactory(name, operation, Registry.currentDeclarationSuite);
      spec.Enabled = enabled && spec.Parent.Enabled;
      Registry.currentDeclarationSuite.AddChild(spec);
    }
  }
}