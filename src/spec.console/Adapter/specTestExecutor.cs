using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using spec.core.Model;

namespace spec.console.Adapter
{
  [ExtensionUri(specTestExecutor.ExecutorUriString)]
  public class specTestExecutor : ITestExecutor
  {

    public const string ExecutorUriString = "executor://specTestExecutor";
    public static readonly Uri ExecutorUri = new Uri(specTestExecutor.ExecutorUriString);

    /// <summary>
    /// Entry point for run single
    /// </summary>
    /// <param name="tests"></param>
    /// <param name="runContext"></param>
    /// <param name="frameworkHandle"></param>
    public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
    {
      //System.Diagnostics.Debugger.Launch();
      var groups = tests.GroupBy(x => x.Source, x => x, (x, y) => new
      {
        Source = x,
        TestCases = y
      }).ToList();


      var results = new List<DefinitionSource>();
      foreach (var groupedItem in groups)
      {
        using (var sandbox = new Sandbox<Executor>(groupedItem.Source))
        {
          var targetTypes = groupedItem.TestCases
                          .Select(x => new DefinitionSource() { ClassName = new Uri(GetSpecID(x)).Host }).ToArray();
          var result = sandbox.Content.Execute(targetTypes);
          results.AddRange(result);
        }
      }

      var joinedList = from r in results
                       join tc in groups.SelectMany(x => x.TestCases) on r.Id equals GetSpecID(tc)
                       select new { TestResult = r, TestCase = tc };

      foreach (var resultItem in joinedList)
      {

        var testResult = new TestResult(resultItem.TestCase);
        if (resultItem.TestResult.Enabled)
        {
          testResult.DisplayName = resultItem.TestResult.Description;
          testResult.Outcome = resultItem.TestResult.RanSuccesfully ? TestOutcome.Passed : TestOutcome.Failed;
          testResult.Duration = resultItem.TestResult.EndTime - resultItem.TestResult.StartTime;
          testResult.ErrorStackTrace = resultItem.TestResult.StackTrace;
        }
        else
        {
          testResult.Outcome = TestOutcome.Skipped;
        }

        testResult.ErrorMessage = resultItem.TestResult.ExecutionResult; ;
        frameworkHandle.RecordResult(testResult);
      }

    }

    private string GetSpecID(TestCase testcase)
    {
      return testcase.Traits.Single(y => y.Name == "SpecId").Value;
    }

    /// <summary>
    /// Entry point for Run All
    /// </summary>
    /// <param name="sources"></param>
    /// <param name="runContext"></param>
    /// <param name="frameworkHandle"></param>
    public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
    {
      //System.Diagnostics.Debugger.Launch();
      IEnumerable<TestCase> tests = specTestDiscoverer.Discover(sources, null);
      RunTests(tests, runContext, frameworkHandle);
    }

    public void Cancel()
    {

    }
  }
}