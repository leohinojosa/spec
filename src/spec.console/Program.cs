using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using spec.Model;
using spec.runner;

namespace spec.console
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Speck runner.\n Running Specs ...");
      string[] sources =
      {
        @"C:\Tesla\Everest\PE_Main\Everest\Everest.Api.Tests\bin\Debug\Everest.Api.Tests.dll",
        @"C:\Personal\proyectos\speck\src\SampleSpecs\bin\debug\SampleSpecs.dll",
      };

      var specList = new Dictionary<string, IEnumerable<DefinitionSource>>();
      foreach (var source in sources)
      {
        using (var sandbox = new Sandbox<Discover>(source))
        {
          var discoveredDefinitions = sandbox.Content.DiscoverSpecsFromCurrentAssembly();

          specList.Add(source, discoveredDefinitions.Select(x => x));
        }
      }

      List<Task> tasks = new List<Task>();
      var executionResult = new List<DefinitionSource>();
      foreach (var specItem in specList.ToList())
      {
        var item = specItem;
        tasks.Add(
          Task.Factory.StartNew(() =>
            {
              using (var sandbox = new Sandbox<Executor>(item.Key))
              {
                executionResult.AddRange(sandbox.Content.Execute(item.Value.ToList()));
              }
            })
          );
      }
      Task.WaitAll(tasks.ToArray());

      foreach (var spec in executionResult.GroupBy(x => x.ParentDescription, x => x))
      {
        Console.WriteLine(" {0}",spec.Key);
        foreach (var definitionSource in spec)
        {
          Console.Write(" ");
          PrintSpecStatus(definitionSource.RanSuccesfully, definitionSource.Enabled);
          Console.Write("{0}",  definitionSource.Description);
          Console.WriteLine();
          if (!String.IsNullOrEmpty(definitionSource.ExecutionResult) )
          {
            Console.WriteLine("  - {0}",definitionSource.ExecutionResult.Replace('\n',' ').Replace('\t',' '));
          }
        }
      }

      var summary = new
      {
        TotalTests = executionResult.Count(),
        TotalPassed = executionResult.Count(x=>x.Enabled && x.RanSuccesfully),
        TotalFailed = executionResult.Count(x=>x.Enabled && !x.RanSuccesfully),
        TotalPending = executionResult.Count(x=>!x.Enabled)
      };
      Console.WriteLine("{0} Total, {1} Passed, {2} Failed, {3} Pending", summary.TotalTests, summary.TotalPassed, summary.TotalFailed, summary.TotalPending);
      Console.ReadLine();
      
    }

    public static void PrintSpecStatus(bool ranSuccesful, bool enabled)
    {
      var label = String.Empty;
      if (enabled)
      {
        if (ranSuccesful)
        {
          Console.ForegroundColor = ConsoleColor.Green;
          label = "\u221A";
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.Red;
          label = "x";
        }
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Yellow;
        label = "\u25A0";
      }
      Console.Write(" " + label + " ");
      Console.ForegroundColor = ConsoleColor.Gray;
    }
  }
}
