using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using spec.console;
using spec.Model;

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
      //System.Diagnostics.Debugger.Launch();
      var specList = new List<dynamic>();
      foreach (var source in sources)
      {
        using (var sandbox = new Sandbox<Discover>(source))
        {
          List<DefinitionSource> discoveredDefinitions = new List<DefinitionSource>();
          try
          {
             discoveredDefinitions = sandbox.Content.DiscoverSpecsFromCurrentAssembly();
          }
          catch (Exception a)
          {
            Console.WriteLine(a.Message);
          }

          specList.AddRange( discoveredDefinitions.Select(x => new { source, It= x }));
         
        }
      }

      foreach (var its in specList)
      {
        var it = its.It;
        var testCase = new TestCase(it.ParentDescription + ".spec" + it.LineNumber, specTestExecutor.ExecutorUri, its.source);
        //var testCase = new TestCase(it.Id, specTestExecutor.ExecutorUri, its.source);
        testCase.CodeFilePath = it.CodeBase;
        testCase.LineNumber = it.LineNumber;
        testCase.DisplayName = it.Description;
        testCase.SetPropertyValue(TestResultProperties.ErrorMessage, "No error");
        testCase.Traits.Add("suite", it.ParentDescription);
        testCase.Traits.Add("source", it.FileName);
        testCase.Traits.Add("FullName", it.Id);

        result.Add(testCase);
        if (discoverySink != null)
        {
          discoverySink.SendTestCase(testCase);
        }
          
      }

      return result;
    }

  }
}