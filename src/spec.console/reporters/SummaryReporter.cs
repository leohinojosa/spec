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
        TotalPending = executionResult.Count(x => !x.Enabled),
        TotalSeconds = executionResult.Where(x => x.Enabled).Sum(x => (x.EndTime - x.StartTime).TotalSeconds)
      };

      if (summary.TotalPassed == summary.TotalTests)
      {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("\n All tests passed! You rock!");
        Console.ForegroundColor = ConsoleColor.Gray;
      }

      Console.WriteLine("");
      executionResult.Where(x=>x.Enabled && !x.RanSuccesfully).Select((exception, index)=>new {exception=exception, index = index+1}).ToList().ForEach(x =>
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(" {0}) {1}, {2}",x.index, x.exception.Description, ConsoleReporter.CleanUpForConsole(x.exception.ExecutionResult.ToLower()));
      });

      Console.ForegroundColor = ConsoleColor.Gray;
      Console.WriteLine("\n {0} Total, {1} Passed, {2} Failed, {3} Pending ({4} secs)", summary.TotalTests, summary.TotalPassed,
        summary.TotalFailed, summary.TotalPending, summary.TotalSeconds);
    }
  }
}