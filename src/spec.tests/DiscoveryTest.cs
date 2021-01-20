using System.Linq;
using FluentAssertions;
using spec.console;
using Xunit;

namespace spec.tests
{
  public class DiscoveryTest
  {
    [Fact(Skip = "Integration test")]
    public void Discover_LoadSpecs()
    {
        string sampleDll = "";
#if DEBUG
            sampleDll = @"/Users/leo.hinojosa/code/leohinojosa/github/spec/src/SampleSpecs/bin/Debug/netstandard2.1/SampleSpecs.dll";
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