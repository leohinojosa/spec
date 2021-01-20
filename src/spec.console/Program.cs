using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using spec.console.reporters;
using spec.core;
using spec.core.Model;

namespace spec.console
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      Console.WriteLine(" Spec runner.\n Running Specs ...");
      var sources = args;

      var specList = new Dictionary<string, IEnumerable<DefinitionSource>>();

      foreach (var source in sources)
      {
        using var sandbox = new Sandbox<Discover>(source);
        var discoveredDefinitions = sandbox.Content.DiscoverSpecsFromCurrentAssembly();
        specList.Add(source, discoveredDefinitions.Select(x => x));
      }

      var executionResult = new List<DefinitionSource>();
      Task.WaitAll(specList.ToList().Select(item => Task.Factory.StartNew(() =>
      {
        using var sandbox = new Sandbox<Executor>(item.Key);
        executionResult.AddRange(sandbox.Content.Execute(item.Value.ToList()));
      })).ToArray());


      var registries = GetRegistriesFromSources(sources);

      var reporters = new List<ITestReporter>() {new ConsoleReporter(), new SummaryReporter()};
      reporters.ForEach(r => { r.Execute(registries, executionResult); });
    }

    private static List<Registry> GetRegistriesFromSources(string[] sources)
    {
      var result = new List<Registry>();
      foreach (var source in sources)
      {
        var assembly = Assembly.LoadFile(source);
        var typeIndex = TypeIndex.TargetTypesToRun(assembly.GetTypes());
        typeIndex.ToList().ForEach(x =>
        {
          var t = CreateInstance(x);
          result.Add(t.Registry);
        });
      }
      return result;
    }

    private static Spec CreateInstance(Type x)
    {
      return Activator.CreateInstance(x, new object[] {}) as Spec;
    }
  }
}
