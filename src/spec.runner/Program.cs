using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace spec.runner
{
  /*
   * separar suite registry a ser una clase injectada a test subject main
   * { 
   *    debido a que, la forma de usar la clase es en el constructor, no puedo crear una instancia e inyectar el registry class
   *    entonces tuve que hacer que la clase base, pudiera generar una instancia para si mismo en el constructor
   * }
   * preparar un spec de prueba con nunit
   * [0] implementar context exactamente como describe
   * implementar The TDD interface provides suite(), test(), suiteSetup(), suiteTeardown(), setup(), and teardown(): para TDD
   * implementar before and after each
   * implementar before and after all
   * implementar context
   * hacer prueba com threads,
   * separar clase de runner
   * implementar test duration en el runner
   *      quitar la referencia de los sample specs, estos los tenemos que leer como dll path
   */

  public class Program
  {
    private static void Main(string[] args)
    {
      //MainExec();

      string[] sources =
      {
        @"C:\Personal\proyectos\speck\src\SampleSpecs\bin\debug\SampleSpecs.dll",
        @"C:\Personal\proyectos\speck\src\SampleSpecs\bin\debug\SampleSpecs.dll"
      };

      SecondaryExec(sources);
      Console.ReadLine();
    }

    private static void SecondaryExec(IEnumerable<string> sources)
    {
      var discoveredTypes = sources.
        Select(Assembly.LoadFile)
        .SelectMany(x => x.GetTypes())
        .Where(t=>t.IsSubclassOf(typeof(spec)));


      foreach (var source in sources)
      {
        using (var sandbox = new Sandbox<Executor>(source))
        {
          sandbox.Content.Execute();  
        }
      }
    }

    public class Executor : DomainProxy
    {
      public void Execute()
      {
        var discoveredTypes = SandboxedAssembly.GetTypes()
                              .Where(t => t.IsSubclassOf(typeof(spec)));
        var runner = new TestRunner();
        runner.MainExec(discoveredTypes);
      }
    }
  }

  public class TestRunner
  {
    public TestSummary MainExec(IEnumerable<Type> specs)
    {
      var registryList = new List<SuiteRegistry>();
      var runableSpecs = new List<Specification>();

      foreach (var spec in specs)
      {
        var instance = Activator.CreateInstance(spec) as spec;
        registryList.Add(instance.Registry);
        runableSpecs.AddRange(instance.Registry.runnableLookupTable);
      }


      foreach (var suiteRegistry in registryList)
      {
        new Runner().run(suiteRegistry.currentDeclarationSuite);

        /*Task.Run(() =>
        {
          new Runner().run(suiteRegistry.currentDeclarationSuite);
        });*/
      }

      var results = new TestSummary
      {
        total = runableSpecs.Count(),
        passed = runableSpecs.Count(x => x.Enabled && x.RanSuccesfully),
        failed = runableSpecs.Count(x => x.Enabled && !x.RanSuccesfully),
        pending = runableSpecs.Count(x => !x.Enabled)
      };

      
      Console.WriteLine("\n{0} Total {1} Passed {2} Failed {3} Pending", results.total, results.passed, results.failed,
        results.pending);

      return results;
    }
  }

  public class TestSummary
  {
    public int total { get; set; }
    public int passed { get; set; }
    public int failed { get; set; }
    public int pending { get; set; }
  }
}
