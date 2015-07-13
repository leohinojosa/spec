﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        //@"C:\Personal\proyectos\speck\src\SampleSpecs\bin\debug\SampleSpecs.dll"
      };

   //   var t = new SuiteDiscovery(sources).Discover();
     
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
}
