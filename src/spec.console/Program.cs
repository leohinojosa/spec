using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using spec.core.Model;

namespace spec.console
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      Console.WriteLine("Speck runner.\n Running Specs ...");
      string[] sources = args;
#if DEBUG
      sources = new[]
      {
        @"C:\Personal\proyectos\spec\src\SampleSpecs\bin\debug\SampleSpecs.dll"
      };
#endif
      var specList = new Dictionary<string, IEnumerable<DefinitionSource>>();

      foreach (var source in sources)
      {
        using (var sandbox = new Sandbox<Discover>(source))
        {
          var discoveredDefinitions = sandbox.Content.DiscoverSpecsFromCurrentAssembly();
          specList.Add(source, discoveredDefinitions.Select(x => x));
        }
      }

      var executionResult = new List<DefinitionSource>();
      Task.WaitAll(specList.ToList().Select(item => Task.Factory.StartNew(() =>
      {
        using (var sandbox = new Sandbox<Executor>(item.Key))
        {
          executionResult.AddRange(sandbox.Content.Execute(item.Value.ToList()));
        }
      })).ToArray());

      foreach (var spec in executionResult.GroupBy(x => x.ParentDescription, x => x))
      {
        Console.WriteLine(" {0}", spec.Key);
        foreach (var definitionSource in spec)
        {
          Console.Write(" ");
          PrintSpecStatus(definitionSource.RanSuccesfully, definitionSource.Enabled);
          Console.Write("{0}", definitionSource.Description);
          Console.WriteLine();
          if (!String.IsNullOrEmpty(definitionSource.ExecutionResult))
          {
            Console.WriteLine("  - {0}", definitionSource.ExecutionResult.Replace('\n', ' ').Replace('\t', ' '));
          }
        }
      }

      var summary = new
      {
        TotalTests = executionResult.Count(),
        TotalPassed = executionResult.Count(x => x.Enabled && x.RanSuccesfully),
        TotalFailed = executionResult.Count(x => x.Enabled && !x.RanSuccesfully),
        TotalPending = executionResult.Count(x => !x.Enabled)
      };
      Console.WriteLine("{0} Total, {1} Passed, {2} Failed, {3} Pending", summary.TotalTests, summary.TotalPassed,
        summary.TotalFailed, summary.TotalPending);
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
