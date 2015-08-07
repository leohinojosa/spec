using System;
using System.Collections.Generic;
using System.Linq;
using spec.core;
using spec.core.Model;

namespace spec.console.reporters
{
  public class SummaryReporter : ITestReporter
  {
    public void Execute(List<Registry> registries, List<DefinitionSource> executionResult)
    {

      var summary = new
      {
        TotalTests = executionResult.Count(),
        TotalPassed = executionResult.Count(x => x.Enabled && x.RanSuccesfully),
        TotalFailed = executionResult.Count(x => x.Enabled && !x.RanSuccesfully),
        TotalPending = executionResult.Count(x => !x.Enabled)
      };

      if (summary.TotalPassed == summary.TotalTests)
      {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("\n All tests passed! You rock!");
        Console.ForegroundColor = ConsoleColor.Gray;
      }

      Console.WriteLine("\n {0} Total, {1} Passed, {2} Failed, {3} Pending", summary.TotalTests, summary.TotalPassed,
        summary.TotalFailed, summary.TotalPending);
    }
  }
}