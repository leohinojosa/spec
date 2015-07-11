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

    public static TestUnit GetSpecs(IEnumerable<TestSourceMap> specs)
    {
      var registryList = new List<SuiteRegistry>();
      var runableSpecs = new List<Specification>();

      foreach (var spec in specs)
      {
        var instance = Activator.CreateInstance(spec.Type) as spec;
        instance.Registry.Source = spec.Source;
        registryList.Add(instance.Registry);
        runableSpecs.AddRange(instance.Registry.runnableLookupTable);
      }

      return new TestUnit
      {
        SuiteRegistry = registryList,
        Specs = runableSpecs
      };
    }

    public IEnumerable<SuiteRegistry> Discover()
    {
      //var specs = _sources.SelectMany( a => new TestSourceMap{Source = a, Types=Assembly.LoadFile(a).GetTypes()})
      var specs = _sources.SelectMany(a =>
        {
          var source = a;
          var types = Assembly.LoadFile(a).GetTypes();
          return types.Select(t => new TestSourceMap { Source = source, Type = t });
        })
        .Where(t => t.Type.IsSubclassOf(typeof(spec)));

      return GetSpecs(specs).SuiteRegistry;
    }
  }

  public class TestSourceMap
  {
    public string Source { get; set; }
    public Type Type { get; set; }
  }

  public class TestUnit
  {
    public List<SuiteRegistry> SuiteRegistry { get; set; }
    public List<Specification> Specs { get; set; }
  }
}