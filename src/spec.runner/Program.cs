using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using spec.Model;
using spec.runner.Engine;
using spec.runner.Model;

namespace spec.runner
{
  /*
   * preparar un spec de prueba con nunit
   */

  public class Program
  {
    private static void Main(string[] args)
    {
      Console.WriteLine("Speck runner.\n Running Specs ...");
      string[] sources =
      {
        @"C:\Personal\proyectos\speck\src\SampleSpecs\bin\debug\SampleSpecs.dll",
      };

      var specRegistries = new Engine.SuiteDiscovery(sources).Discover();

      var r = new SuiteExecutor();
      summary = r.Execute(specRegistries);

      PrintSummary(specRegistries);

      Console.ReadLine();
    }

    private static TestSummary summary;
    private static string whitespace = ".";
    
    private static void PrintSummary(IEnumerable<Registry> specRegistries)
    {
      foreach (var spec in specRegistries.SelectMany(x => x.CurrentSuite.Children))
      {
        whitespace = String.Empty;
        Console.WriteLine("");
        PrintOut(spec);
      }
      Console.WriteLine("\n{0} Total {1} Passed {2} Failed {3} Pending\n", summary.total, summary.passed, summary.failed,
        summary.pending);
    }
    public static void PrintOut(Definition spec)
    {
      Console.ForegroundColor = ConsoleColor.Gray;
      if (spec.GetType() == typeof(Specification))
      {
        var specResult = summary.specs.SingleOrDefault(x => x.Id == spec.Id);
        PrintSpecStatus(specResult.RanSuccesfully, spec.Enabled);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine( "{it} " + spec.Description.Trim());
      }
      else
      {
        Console.WriteLine(whitespace + spec.Description.Trim());
      }

      if (spec.Children.Count > 0)
      {
        whitespace = whitespace + ".";
        foreach (var definition in spec.Children)
        {
          PrintOut(definition);
        }
        whitespace = whitespace.TrimEnd('.');
      }
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
      Console.Write(whitespace + label + " ");
      Console.ForegroundColor = ConsoleColor.Gray;
    }
  }
}
