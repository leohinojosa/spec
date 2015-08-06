using System.Collections.Generic;
using spec.core;
using spec.core.Model;

namespace spec.console.reporters
{
  public interface ITestReporter
  {
    void Execute(List<Registry> registries, List<DefinitionSource> executionResult);
  }
}