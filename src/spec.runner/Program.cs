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
   * separar suite registry a ser una clase injectada a test subject main
   * { 
   *    debido a que, la forma de usar la clase es en el constructor, no puedo crear una instancia e inyectar el registry class
   *    entonces tuve que hacer que la clase base, pudiera generar una instancia para si mismo en el constructor
   * }
   * preparar un spec de prueba con nunit
   * hacer prueba com threads,
   * separar clase de runner
   * implementar test duration en el runner
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

      //vamos a recorrer
      foreach (var spec in specRegistries.SelectMany(x => x.CurrentSuite.Childs))
      {
        whitespace = String.Empty;
        Console.WriteLine("");
        PrintOut(spec);
      }
      Console.WriteLine("\n{0} Total {1} Passed {2} Failed {3} Pending\n", summary.total, summary.passed, summary.failed,
        summary.pending);

      Console.ReadLine();
    }

    private static TestSummary summary;
    private static string whitespace = ".";
    public static void PrintOut(Definition spec)
    {
      whitespace = whitespace + ".";
      if (spec.GetType() == typeof(Specification))
      {
        var specResult = summary.specs.SingleOrDefault(x => x.Id == spec.Id);
        print(specResult.RanSuccesfully, spec.Enabled);
        Console.WriteLine( spec.Description.Trim());
      }
      else
      {
        Console.WriteLine(whitespace + spec.Description.Trim());
      }

      if (spec.Childs.Count > 0)
      {
        whitespace = whitespace + ".";
        foreach (var definition in spec.Childs)
        {
          PrintOut(definition);
        }
        whitespace = whitespace.TrimEnd('.');
      }
      whitespace = whitespace.TrimEnd('.');
    }

    public static void print(bool ranSuccesful, bool enabled)
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
