using System;
using System.Collections.Generic;
using System.Linq;
using spec.runner.Model;

namespace spec.runner.Engine
{
  public class SuiteExecutor
  {
    public void Execute(IEnumerable<Registry> sources)
    {

      var groups = sources.GroupBy(x => x.Source, x => x, (x, y) => new
      {
        Source = x,
        TestCases = y.SelectMany(a=>a.runnableLookupTable)
      }).ToList();

      var results = new List<TestSummary>();
      foreach (var groupedItem in groups)
      {
       
        using (var sandbox = new Sandbox<Executor>(groupedItem.Source))
        {
          var targetTypes = groupedItem.TestCases.Select(x => x.ClassName.ToLower()).Distinct().ToArray();
          var result = sandbox.Content.Execute(targetTypes);
          results.Add(result);
        }
      }

      var summary = new TestSummary
      {
        total = results.Sum(x => x.total),
        passed = results.Sum(x => x.passed),
        failed = results.Sum(x => x.failed),
        pending = results.Sum(x => x.pending)
      };

      Console.WriteLine("\n{0} Total {1} Passed {2} Failed {3} Pending\n", summary.total, summary.passed, summary.failed,
        summary.pending);


    }
  }
}