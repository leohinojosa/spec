using System;
using System.Linq;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace spec.tests
{
  public class SpecTests
  {
    private spec_double _suite;

    public SpecTests()
    {
      _suite = new spec_double();
    }

    [Fact]
    public void Registry_InitialState()
    {
      _suite.Registry.Should().NotBe(null, "because all suites should have a Registry class");
      _suite.Registry.CurrentSuite.Description.Should().Be("Root");
      _suite.Registry.CurrentSuite.Children.Should().BeEmpty();
      _suite.Registry.ExecutableLookupTable.Should().BeEmpty();
    }

    [Fact]
    public void describe_registerSuite()
    {
      _suite.describe("new Describe", () => { });
      _suite.Registry.CurrentSuite.Children.Should().NotBeEmpty();
    }

    [Fact]
    public void describe_registerContext()
    {
      _suite.context("new Context", () => { });
      _suite.Registry.CurrentSuite.Children.Should().NotBeEmpty();
    }

    [Fact]
    public void describe_registerChildSuite()
    {
      _suite.describe("new Describe", () => { _suite.describe("new child", () => { }); });

      _suite.Registry.CurrentSuite.Children.First().Children.Should().NotBeEmpty();
    }

    [Fact]
    public void hooks_beforeAndAfterHooksShouldBeRegistered()
    {
      _suite.Registry.CurrentSuite.BeforeAll.Should().BeEmpty();
      _suite.Registry.CurrentSuite.BeforeEach.Should().BeEmpty();
      _suite.Registry.CurrentSuite.AfterEach.Should().BeEmpty();
      _suite.Registry.CurrentSuite.AfterAll.Should().BeEmpty();

      _suite.beforeAll(() => { });
      _suite.beforeEach(() => { });
      _suite.afterEach(() => { });
      _suite.afterAll(() => { });

      _suite.Registry.CurrentSuite.BeforeAll.Should().NotBeEmpty();
      _suite.Registry.CurrentSuite.BeforeEach.Should().NotBeEmpty();
      _suite.Registry.CurrentSuite.AfterEach.Should().NotBeEmpty();
      _suite.Registry.CurrentSuite.AfterAll.Should().NotBeEmpty();
    }

    [Fact]
    public void it_specsShouldBeRegistered()
    {
      _suite.describe("new Describe", () => { _suite.it("spec", () => { }); });

      _suite.Registry.CurrentSuite.Children.First().Children.First().Should().NotBeNull();
      _suite.Registry.CurrentSuite.Children.First().Children.First().Description.Should().Be("spec");
    }

    [Fact]
    public void it_specsShouldHaveADescription()
    {
      Action action = () => { _suite.describe("new Describe", () => { _suite.it("", () => { }); });};
      action.ShouldThrow<Exception>();
    }
  }
}