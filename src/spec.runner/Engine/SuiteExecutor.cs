using System;
using System.Collections.Generic;
using System.Linq;
using spec.runner.Model;

namespace spec.runner.Engine
{
  public class SuiteExecutor
  {
    public TestSummary Execute(IEnumerable<Registry> sources)
    {

      var groups = sources.GroupBy(x => x.Source, x => x, (x, y) => new
      {
        Source = x,
        TestCases = y.SelectMany(a => a.ExecutableLookupTable)
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

      return new TestSummary
      {
        total = results.Sum(x => x.total),
        passed = results.Sum(x => x.passed),
        failed = results.Sum(x => x.failed),
        pending = results.Sum(x => x.pending),
        specs = results.SelectMany(x => x.specs).Select(x => new SpecSummary { Id = x.Id, ExecutionResult = x.ExecutionResult, RanSuccesfully = x.RanSuccesfully}).ToList()
      };
    }
  }
}