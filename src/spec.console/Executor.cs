using System.Collections.Generic;
using System.Linq;
using spec.core;
using spec.core.Model;

namespace spec.console
{
  public class Executor : DomainProxy
  {
    public List<DefinitionSource> Execute(IEnumerable<DefinitionSource> typesSources)
    {
      var availableTypes = SandboxedAssembly.GetTypes();
      var targetTypesToRun = TypeIndex.TargetTypesToRun(typesSources, availableTypes);

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