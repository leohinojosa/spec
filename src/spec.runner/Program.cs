using System;
using System.Collections.Generic;
using System.Linq;

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
      Console.WriteLine("Speck runner.\n Running Specs ...");
      string[] sources =
      {
        @"C:\Personal\proyectos\speck\src\SampleSpecs\bin\debug\SampleSpecs.dll",
        //@"C:\Personal\proyectos\speck\src\SampleSpecs\bin\debug\SampleSpecs.dll"
      };

      var t = new SuiteDiscovery(sources).Discover();
     
      SpecExecutor(sources);
      Console.ReadLine();
    }

    private static void SpecExecutor(IEnumerable<string> sources)
    {
      List<TestSummary> testResults;
      testResults = new List<TestSummary>();
      foreach (var source in sources)
      {
        TestSummary result;
        using (var sandbox = new Sandbox<Executor>(source))
        {
          result = sandbox.Content.Execute();
          testResults.Add(result);
        }
      }

      var results = new TestSummary
      {
        total = testResults.Sum(x=>x.total),
        passed = testResults.Sum(x => x.passed),
        failed = testResults.Sum(x => x.failed),
        pending = testResults.Sum(x => x.pending)
      };

      Console.WriteLine("\n{0} Total {1} Passed {2} Failed {3} Pending\n", results.total, results.passed, results.failed,
        results.pending);


    }

  }

  public class Executor : DomainProxy
  {
    public TestSummary Execute()
    {
      var discoveredTypes = SandboxedAssembly.GetTypes()
                            .Where(t => t.IsSubclassOf(typeof(Spec)))
                            .Select(t=> new TestSourceMap{Source = this.Source, Type = t });

      var runner = new SuiteRunner();
      return runner.RunSpecs(discoveredTypes);
    }

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

  public class SuiteRunner
  {
    //Maybe we can split discovery and execution into two separate clases ?
    public TestSummary RunSpecs(IEnumerable<TestSourceMap> specs)
    {
      var testUnit = SuiteDiscovery.GetSpecs(specs);

     
     //foreach (var suiteRegistry in testUnit.SuiteRegistry.SelectMany(x=>x.currentDeclarationSuite))
       foreach (var suiteRegistry in testUnit.SuiteRegistry.Select(x => x.currentDeclarationSuite))
      {
        new Runner().run(suiteRegistry);

        /*Task.Run(() =>
        {
          new Runner().run(suiteRegistry.currentDeclarationSuite);
        });*/
      }
      
      var results = new TestSummary
      {
        total = testUnit.Specs.Count(),
        passed = testUnit.Specs.Count(x => x.Enabled && x.RanSuccesfully),
        failed = testUnit.Specs.Count(x => x.Enabled && !x.RanSuccesfully),
        pending = testUnit.Specs.Count(x => !x.Enabled),
        specs = testUnit.Specs.Select(x => new SpecSummary { Id = x.Id, ExecutionResult = x.ExecutionResult, RanSuccesfully = x.RanSuccesfully, ExecutionStatus = x.ExecutionStatus, Enabled = x.Enabled }).ToList()
      };

      return results;
    }

  }

  [Serializable]
  public class SpecSummary
  {
    public void Add(string id)
    {
      throw new NotImplementedException();
    }

    public ExecStatus ExecutionStatus { get; set; }
    public bool RanSuccesfully { get; set; }
    public string ExecutionResult { get; set; }
    public string Id { get; set; }
    public bool Enabled { get; set; }
  }

  [Serializable]
  public class TestSummary 
  {
    public int total { get; set; }
    public int passed { get; set; }
    public int failed { get; set; }
    public int pending { get; set; }
    public List<SpecSummary> specs { get; set; }

    // public List<Specification> specs { get; set; }
  }
}
