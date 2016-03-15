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
        string sampleDll = "";
#if DEBUG
            sampleDll = @"..\..\..\SampleSpecs\bin\debug\SampleSpecs.dll";
#else
            sampleDll = @"..\..\..\SampleSpecs\bin\Release\SampleSpecs.dll";
#endif

            using (var sandbox = new Sandbox<Discover>(sampleDll))
      {
        var discoveredDefinitions = sandbox.Content.DiscoverSpecsFromCurrentAssembly();
        discoveredDefinitions.Should().NotBeEmpty("Because its loading spec definitions");
      }
    }
  }
}