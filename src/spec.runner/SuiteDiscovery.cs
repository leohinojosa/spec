using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace spec.runner
{
  /// <summary>
  /// Class that loads list of SuiteRegistry types
  /// which have every suite and every spec
  /// </summary>
  public class SuiteDiscovery
  {
    private readonly IEnumerable<string> _sources;

    public SuiteDiscovery(IEnumerable<string> sources)
    {
      _sources = sources;
    }

    public static TestUnit GetSpecs(IEnumerable<Type> specs)
    {
      var registryList = new List<SuiteRegistry>();
      var runableSpecs = new List<Specification>();

      foreach (var spec in specs)
      {
        var instance = Activator.CreateInstance(spec) as spec;
        registryList.Add(instance.Registry);
        runableSpecs.AddRange(instance.Registry.runnableLookupTable);
      }

      return new TestUnit
      {
        SuiteRegistry = registryList,
        Specs = runableSpecs
      };
    }

    public IEnumerable<SuiteRegistry>  Discover()
    {
      var specs = _sources.Select(Assembly.LoadFile)
        .SelectMany(x => x.GetTypes())
        .Where(t => t.IsSubclassOf(typeof (spec)));
      
      return GetSpecs(specs).SuiteRegistry;
    }
  }

  public class TestUnit
  {
    public List<SuiteRegistry> SuiteRegistry { get; set; }
    public List<Specification> Specs { get; set; }
  }
}