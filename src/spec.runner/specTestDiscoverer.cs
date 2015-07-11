﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;


namespace spec.runner
{
  [FileExtension(".exe")]
  [FileExtension(".dll")]
  [DefaultExecutorUri(specTestExecutor.ExecutorUriString)]
  public class specTestDiscoverer : ITestDiscoverer
  {
    public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger,
      ITestCaseDiscoverySink discoverySink)
    {
     // System.Diagnostics.Debugger.Launch();
      Discover(sources, discoverySink);
    }

    public static IEnumerable<TestCase>  Discover(IEnumerable<string> sources, ITestCaseDiscoverySink discoverySink)
    {
      List<TestCase> result = new List<TestCase>();
      // System.Diagnostics.Debugger.Launch();
      var t = new SuiteDiscovery(sources).Discover();
      var tt = sources.First();

      foreach (var it in t.SelectMany(x => x.runnableLookupTable))
      {
        var testCase = new TestCase(String.Format("{0} @ {1}", it.Parent.Description, it.Description), specTestExecutor.ExecutorUri, tt);
        // testCase.SetPropertyValue(TestResultProperties.Outcome, "Passed");
        testCase.SetPropertyValue(TestResultProperties.ErrorMessage, "No error");
        result.Add(testCase);
        if (discoverySink != null)
        {
          discoverySink.SendTestCase(testCase);
        }

      }

      return result;
    }

  }

  [ExtensionUri(specTestExecutor.ExecutorUriString)]
  public class specTestExecutor : ITestExecutor
  {
    public const string ExecutorUriString = "executor://specTestExecutor";
    public static readonly Uri ExecutorUri = new Uri(specTestExecutor.ExecutorUriString);

    public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
    {
      System.Diagnostics.Debugger.Launch();
     
      Console.WriteLine("cake");
      /*var t = new SuiteDiscovery.Discover();

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
        total = testResults.Sum(x => x.total),
        passed = testResults.Sum(x => x.passed),
        failed = testResults.Sum(x => x.failed),
        pending = testResults.Sum(x => x.pending)
      };*/
    }

    public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
    {
      IEnumerable<TestCase> tests = specTestDiscoverer.Discover(sources, null);

      RunTests(tests, runContext, frameworkHandle);
    }

    public void Cancel()
    {
      
    }
  }
}