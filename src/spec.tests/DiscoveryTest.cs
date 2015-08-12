using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using spec.console;

namespace spec.tests
{
  [TestClass]
  public class DiscoveryTest
  {
    [TestMethod]
    public void Discover_LoadSpecs()
    {
      var sampleDll = @"..\..\..\SampleSpecs\bin\debug\SampleSpecs.dll";
      using (var sandbox = new Sandbox<Discover>(sampleDll))
      {
        var discoveredDefinitions = sandbox.Content.DiscoverSpecsFromCurrentAssembly();
        discoveredDefinitions.Should().NotBeEmpty("Because its loading spec definitions");
      }
    }
  }
}