using System;
using System.Collections.Generic;
using System.Linq;
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
      r.Execute(specRegistries);
      Console.ReadLine();
    }
  }
}
