using System.Collections.Generic;
using System.Linq;
using spec.runner.Engine;
using spec.runner.Model;

namespace spec.runner
{
  public class Executor : DomainProxy
  {
    //asi podemos solo ejecutar las que nos pidieron y no todas las clases que existen
    public TestSummary Execute(IEnumerable<string> targetTypes)
    {
      var discoveredTypes = SandboxedAssembly.GetTypes()
        .Where(t => t.IsSubclassOf(typeof(Spec)))
        .Where(t => targetTypes.Contains(t.FullName.ToLower()))
        .Select(t => new TestSourceMap { Source = this.Source, Type = t });

      var runner = new SuiteRunner();
      return runner.RunSpecs(discoveredTypes);
    }

  }
}