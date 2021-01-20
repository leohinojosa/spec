using System.Collections.Generic;
using System.IO;
using System.Linq;
using spec.core;
using spec.core.Model;

namespace spec.console
{
    public class Executor : DomainProxy
    {
        public IEnumerable<DefinitionSource> Execute(IEnumerable<DefinitionSource> typesSources)
        {
            var availableTypes = SandboxedAssembly.GetTypes();
            var targetTypesToRun = TypeIndex.TargetTypesToRun(typesSources, availableTypes);

            var agent = new Agent();
            var specSummary = new List<DefinitionSource>();

            using (var writer = new StringWriter())
            {
                foreach (var specTypes in targetTypesToRun)
                {
                    var currentRunninType = SandboxedAssembly.CreateInstance(specTypes.FullName) as Spec;
                    agent.RunSuite(currentRunninType);
                    specSummary.AddRange(currentRunninType.Registry.ExecutableLookupTable.Select(DefinitionSource.CreateSource).ToList());
                }
            }
            return specSummary;
        }
    }
}