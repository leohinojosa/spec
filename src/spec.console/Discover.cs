using System;
using System.Collections.Generic;
using System.Linq;
using spec.console.Adapter;
using spec.core;
using spec.core.Model;

namespace spec.console
{
  public class Discover : DomainProxy
  {
    public List<DefinitionSource> DiscoverSpecsFromCurrentAssembly()
    {
      var discoveredTypes = TypeIndex.TargetTypesToRun(SandboxedAssembly.GetTypes());
      
      var result = new List<DefinitionSource>();
      foreach (var type in discoveredTypes)
      {
        var currentType = SandboxedAssembly.CreateInstance(type.FullName) as Spec;

        var lookupTable = currentType.Registry.ExecutableLookupTable
                          .Select(DefinitionSource.CreateSource);

        result.AddRange(lookupTable);
      }

      return result;
    }

  }
}