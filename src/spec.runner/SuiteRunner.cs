using System.Collections.Generic;
using System.Linq;
using spec.runner.Engine;
using spec.runner.Model;

namespace spec.runner
{
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
        specs = testUnit.Specs.Select(x => new SpecSummary { Id = x.Id, ExecutionResult = x.ExecutionResult, RanSuccesfully = x.RanSuccesfully, ExecutionStatus = x.ExecutionStatus, Enabled = x.Enabled, Duration = x.EndTime - x.StartTime,  }).ToList()
      };

      return results;
    }

  }
}