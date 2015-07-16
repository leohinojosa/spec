using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using spec.runner.Engine;

namespace spec.runner.Adapter
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

      foreach (var suiteRegistry in t)
      {
        suiteRegistry.ExecutableLookupTable.ForEach(it =>
        {
          var testCase = new TestCase(it.Id, specTestExecutor.ExecutorUri, suiteRegistry.Source);
          testCase.CodeFilePath = it.CodeBase;
          testCase.LineNumber = it.LineNumber;
          //testCase.DisplayName = String.Format("{0} @ {1}", it.Parent.Description, it.Description);
          testCase.DisplayName = it.Description;
          testCase.SetPropertyValue(TestResultProperties.ErrorMessage, "No error");
          testCase.Traits.Add("suite", it.Parent.Description);
          testCase.Traits.Add("source", it.FileName);
          
          result.Add(testCase);
          if (discoverySink != null)
          {
            discoverySink.SendTestCase(testCase);
          }
        });
          
      }

      return result;
    }

  }
}