using System;
using System.Collections.Generic;
using System.Linq;
using spec.Model;
using spec.runner;

namespace spec.console
{
  public class Discover : DomainProxy
  {
    public List<DefinitionSource> DiscoverSpecsFromCurrentAssembly()
    {
      var discoveredTypes = SandboxedAssembly.GetTypes()
        .Where(t => t.BaseType == typeof(spec.Spec))
        .Select(t => t)
        .ToArray();

      var result = new List<DefinitionSource>();
      foreach (var type in discoveredTypes)
      {
        var currentType = SandboxedAssembly.CreateInstance(type.FullName) as spec.Spec;

        var lookupTable = currentType.Registry.ExecutableLookupTable
                          .Select(DefinitionSource.CreateSource);

        result.AddRange(lookupTable);
      }

      return result;
    }

  }
}