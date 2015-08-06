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
        printr(x.CurrentSuite);
      });
    }

    private void printr(Definition def, int level = 2)
    {
      if (def.GetType().IsAssignableFrom(typeof (Specification)))
      {
        Console.ForegroundColor = ConsoleColor.White;
      }
      else if (def.GetType().IsAssignableFrom(typeof(Suite)))
      {
        Console.ForegroundColor = ConsoleColor.Gray;
      }

      if(def.Parent != null )
      {
        Console.WriteLine("{0}{1}", new String(' ', level), Truncate(def.Description, 70));
      }

      def.Children.ForEach(x =>
      {
        printr(x, level + 1);        
      });

      if (def.GetType().IsAssignableFrom(typeof(Specification)) && !def.GetType().IsAssignableFrom(typeof(Suite)))
      {
        Console.SetCursorPosition(1,Console.CursorTop - 1);
        var result = _execresult.FirstOrDefault(x => x.Id == def.Id);
        PrintSpecStatus(result.RanSuccesfully, result.Enabled);
        
      }

    }

    public void PrintSpecStatus(bool ranSuccesful, bool enabled)
    {
      var label = String.Empty;
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

    public string Truncate( string value, int maxChars)
    {
      return value.Length <= maxChars ? value : value.Substring(0, maxChars) + " ..";
    }
  }
}