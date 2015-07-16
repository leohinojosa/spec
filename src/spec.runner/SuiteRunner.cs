using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using spec.Model;
using spec.runner.Engine;
using spec.runner.Model;

namespace spec.runner
{
  public class SuiteRunner
  {
    //Maybe we can split discovery and execution into two separate clases ?
    public TestSummary RunSpecs(IEnumerable<TestSourceMap> specs)
    {
      //we call twice the get specs method, 
      // the first time in the SuiteDiscovery, to get a list of all existing types
      var testUnit = SuiteDiscovery.GetSpecs(specs);
      
      List<Task> tasks = new List<Task>();
      foreach (var suiteRegistry in testUnit.SuiteRegistry.Select(x => x.CurrentSuite))
      {
        Task t = Task.Factory.StartNew(()=>new Agent().RunSuite(suiteRegistry));
        tasks.Add(t);
        //new Agent().RunSuite(suiteRegistry);
      }

      Task.WaitAll(tasks.ToArray());
      
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