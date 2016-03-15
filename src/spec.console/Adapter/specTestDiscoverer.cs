using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using spec.core.Model;

namespace spec.console.Adapter
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

          discoveredDefinitions
            .Select(x => AddToSink(source, x, discoverySink))
            .ToList()
            .ForEach(x => result.Add(x));
        }
      }

      return result;
    }

    public static TestCase AddToSink(string source, DefinitionSource definitionSources, ITestCaseDiscoverySink discoverySink)
    {
      var it = definitionSources;
      var testCase = new TestCase(it.ParentDescription + " " + it.Description + ".spec" + it.LineNumber, specTestExecutor.ExecutorUri, source);
      testCase.CodeFilePath = it.CodeBase;
      testCase.LineNumber = it.LineNumber;
      testCase.DisplayName = it.Description;
      testCase.SetPropertyValue(TestResultProperties.ErrorMessage, "No error");
      testCase.Traits.Add("File", it.FileName);
      testCase.Traits.Add("SpecId", it.Id);

      if (discoverySink != null)
      {
        discoverySink.SendTestCase(testCase);
      }
      return testCase;
    }

  }
}