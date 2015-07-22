using System;
using System.Collections.Generic;
using System.Linq;
using spec.console.Model;
using spec.core;
using spec.core.Model;

namespace spec.runner
{
  public class Executor : DomainProxy
  {
    public List<DefinitionSource> Execute(IEnumerable<DefinitionSource> typesSources)
    {
      var targetTypesToRun = SandboxedAssembly.GetTypes()
        .Where(t => t.BaseType == typeof(Spec))
        .Where(t => typesSources.Select(x => x.ClassName).Distinct().Contains(t.FullName, StringComparer.OrdinalIgnoreCase))
        .Select(t => t)
        .ToArray();

      var agent = new Agent();
      var specSummary = new List<DefinitionSource>();
      
      foreach (var specTypes in targetTypesToRun)
      {
        var currentRunninType = SandboxedAssembly.CreateInstance(specTypes.FullName) as Spec;
        agent.RunSuite(currentRunninType.Registry.CurrentSuite);


        specSummary.AddRange(currentRunninType.Registry.ExecutableLookupTable.Select( DefinitionSource.CreateSource).ToList());
      }

      return specSummary;
    }

  }
}