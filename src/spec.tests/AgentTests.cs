using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using spec.core;

namespace spec.tests
{
  [TestClass]
  public class AgentTests
  {
    spec_double _suite;
    [TestInitialize]
    public void Setup()
    {
      _suite = new spec_double();
    }

    [TestMethod]
    public void spec_shouldNotAllowAddingNestedSuites()
    {

    _suite = new spec_double();
      _suite.describe("new Describe", () =>
      {
        _suite.it("spec", () =>
        {
          _suite.describe("invalid", () => { });
        });
      });

      Agent agent = new Agent();
      agent.RunSuite(_suite.Registry.CurrentSuite);
      _suite.Registry.ExecutableLookupTable.Any(x => x.RanSuccesfully).Should().BeFalse("describes should not be in specs");
    }

    [TestMethod]
    public void agent_executesASpecAndRunsSuccesfully()
    {
      var i = 0;
      _suite.describe("new", () =>
      {
        i = 0;
        _suite.beforeAll(() =>
        {
          i++;
        });

        _suite.beforeEach(() =>
        {
          i++;
        });

        _suite.it("spec1", () =>
        {
          i++;
        });

        _suite.it("spec2", () =>
        {
          i++;
        });

        _suite.afterEach(() =>
        {
          i++;
        });

        _suite.afterAll(() =>
        {
          i++;
        });
      });

      var agent = new Agent();
      agent.RunSuite(_suite.Registry.CurrentSuite);
      i.Should().Be(8);
    }

    [TestMethod]
    public void agent_executesASpecAndRunsSuccesfullyWithHooks()
    {
      var i = 0;
      _suite.describe("new", () =>
      {
        _suite.beforeAll(() =>
        {
          i++;
        });

        _suite.beforeEach(() =>
        {
          i++;
        });
        
        _suite.beforeEach(() =>
        {
          i++;
        });

        _suite.it("spec1", () =>
        {
          //
        });

        _suite.afterEach(() =>
        {
          i--;
        });

        _suite.afterEach(() =>
        {
          i--;
        });

        _suite.afterAll(() =>
        {
          i--;
        });
      });

      var agent = new Agent();
      agent.RunSuite(_suite.Registry.CurrentSuite);
      i.Should().Be(0);
    }

    [TestMethod]
    public void agent_executesASpecAndRunsSuccesfullyWithHooksSiblingDescribes()
    {
      var i = 0;
      var action = new Action(() =>
      {
        _suite.beforeAll(() =>
        {
          i++;
        });

        _suite.beforeEach(() =>
        {
          i++;
        });

        _suite.it("spec1", () =>
        {
          //
        });

        _suite.afterEach(() =>
        {
          i--;
        });

        _suite.afterAll(() =>
        {
          i--;
        });
      });

      _suite.describe("new", action);
      _suite.describe("new1", action);

      var agent = new Agent();
      agent.RunSuite(_suite.Registry.CurrentSuite);
      i.Should().Be(0);
    }

    [TestMethod]
    public void agent_executesASpecAndRunsSuccesfullyWithHooksNestedDescribes()
    {
      var i = 0;
      var action = new Action(() =>
      {
        _suite.beforeAll(() =>
        {
          i++;
        });

        _suite.beforeEach(() =>
        {
          i++;
        });

        _suite.it("spec1", () =>
        {
          //
        });

        _suite.afterEach(() =>
        {
          i--;
        });

        _suite.afterAll(() =>
        {
          i--;
        });
      });

      _suite.describe("new", () =>
      {
        _suite.describe("Nested1", action);
        _suite.describe("Nested2", action);
      });

      var agent = new Agent();
      agent.RunSuite(_suite.Registry.CurrentSuite);
      i.Should().Be(0);
    }
  }
}