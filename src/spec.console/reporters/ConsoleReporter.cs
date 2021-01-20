using System;
using System.Collections.Generic;
using System.Linq;
using spec.core;
using spec.core.Model;

namespace spec.console.reporters
{
  public class ConsoleReporter : ITestReporter
  {
    private List<DefinitionSource> _execresult;

    public void Execute(List<Registry> registries, List<DefinitionSource> executionResult)
    {
      _execresult = executionResult;
      registries.ForEach(x =>
      {
        Console.WriteLine("");
        PrintDefinition(x.CurrentSuite);
      });
    }

    private void PrintDefinition(Definition def, int level = 2)
    {
      Console.ForegroundColor = IsDefinitionASpec(def) ? ConsoleColor.White : ConsoleColor.Gray;

      if (!IsRootDefinition(def))
      {
        Console.WriteLine("{0}{1}", new String(' ', level), Truncate(def.Description, 70));
      }

      def.Children.ForEach(x => { PrintDefinition(x, level + 1); });

      if (IsDefinitionASpec(def))
      {
        Console.SetCursorPosition(1, Console.CursorTop - 1);
        var result = _execresult.FirstOrDefault(x => x.Id == def.Id);
        PrintSpecStatus(result.RanSuccessfully, result.Enabled);
        if (result.Enabled && !result.RanSuccessfully)
        {
          Console.WriteLine("{0}{1}", new String(' ', level), CleanUpForConsole(Truncate(result.ExecutionResult.Trim(), 120)));
        }
      }
    }

    private bool IsRootDefinition(Definition def)
    {
      return def.Parent == null;
    }

    private bool IsDefinitionASpec(Definition definition)
    {
      return definition.GetType().IsAssignableFrom(typeof (Specification)) && !definition.GetType().IsAssignableFrom(typeof (Suite));
    }

    public void PrintSpecStatus(bool ranSuccesful, bool enabled)
    {
      string label;
      if (enabled)
      {
        if (ranSuccesful)
        {
          Console.ForegroundColor = ConsoleColor.Green;
          label = "\u221A";
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.Red;
          label = "x";
        }
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Yellow;
        label = "\u25A0";
      }
      Console.WriteLine(label);
      Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static string CleanUpForConsole(string value)
    {
      return value.Replace('\n', ' ').Replace('\t', ' ');
    }
    private string Truncate(string value, int maxChars)
    {
      return value.Length <= maxChars ? value : value.Substring(0, maxChars) + " ..";
    }
  }
}