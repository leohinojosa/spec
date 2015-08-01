using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using spec.core;

namespace spec.tests
{
  public class spec_double : Spec
  {
  }

  [TestClass]
  public class SpecTests
  {
    spec_double suite ;
    [TestInitialize]
    public void setup()
    {
      suite = new spec_double();
    }

    [TestMethod]
    public void Registry_InitialState()
    {
      suite.Registry.Should().NotBe(null, "because all suites should have a Registry class");
      suite.Registry.CurrentSuite.Description.Should().Be("Root");
      suite.Registry.CurrentSuite.Children.Should().BeEmpty();
      suite.Registry.ExecutableLookupTable.Should().BeEmpty();
    }

    [TestMethod]
    public void describe_registerSuite()
    {
      suite.describe("new Describe", () => { });
      suite.Registry.CurrentSuite.Children.Should().NotBeEmpty();
    }

    [TestMethod]
    public void describe_registerContext()
    {
      suite.context("new Context", () => { });
      suite.Registry.CurrentSuite.Children.Should().NotBeEmpty();
    }


    [TestMethod]
    public void describe_registerChildSuite()
    {
      suite.describe("new Describe", () =>
      {
        suite.describe("new child", ()=>{});
      });
      
      suite.Registry.CurrentSuite.Children.First().Children.Should().NotBeEmpty();
    }

    [TestMethod]
    public void hooks_beforeAndAfterHooksShouldBeRegistered()
    {
      suite.Registry.CurrentSuite.BeforeAll.Should().BeEmpty();
      suite.Registry.CurrentSuite.BeforeEach.Should().BeEmpty();
      suite.Registry.CurrentSuite.AfterEach.Should().BeEmpty();
      suite.Registry.CurrentSuite.AfterAll.Should().BeEmpty();

      suite.beforeAll(() => { });
      suite.beforeEach(() => { });
      suite.afterEach(() => { });
      suite.afterAll(() => { });

      suite.Registry.CurrentSuite.BeforeAll.Should().NotBeEmpty();
      suite.Registry.CurrentSuite.BeforeEach.Should().NotBeEmpty();
      suite.Registry.CurrentSuite.AfterEach.Should().NotBeEmpty();
      suite.Registry.CurrentSuite.AfterAll.Should().NotBeEmpty();
    }

    [TestMethod]
    public void it_specsShouldBeRegistered()
    {
      suite.describe("new Describe", () =>
      {
        suite.it("spec", () => { });
      });

      suite.Registry.CurrentSuite.Children.First().Children.First().Should().NotBeNull();
      suite.Registry.CurrentSuite.Children.First().Children.First().Description.Should().Be("spec");
    }

    public class double2 : Spec
    {
      public double2()
      {
        describe("new Describe", () =>
        {
          it("spec", () =>
          {
            describe("invalid", () => { });
          });
        });
      }
    }

    [TestMethod]
    public void it_specsShouldNotHaveDescribes()
    {
      var doubles = new double2();

      Agent agent = new Agent();


      agent.RunSuite(doubles.Registry.CurrentSuite);

      doubles.Registry.ExecutableLookupTable.Any(x => x.RanSuccesfully).Should().BeFalse("describes should not be in specs");
    }
  }
}