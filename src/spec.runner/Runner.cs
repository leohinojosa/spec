using System;
using System.Security.Cryptography.X509Certificates;

namespace spec.runner
{
  public class Runner
  {
    private string whitespace = "";

    public void run(Definition suite)
    {
      try
      {
        whitespace = whitespace + " ";
        Console.WriteLine(whitespace + "*" + suite.Description);
        suite.ExecutionStatus = ExecStatus.Running;
        if (suite.BeforeAll.Count > 0)
        {
          whitespace = whitespace + " ";
          suite.BeforeAll.ForEach(x =>
          {
            Console.WriteLine(whitespace + x.Description);
            x.ExecutionStatus = ExecStatus.Running;
            x.Fn();
            x.ExecutionStatus = ExecStatus.Completed;
          });
        }

        if (suite.Childs.Count > 0)
        {
          whitespace = whitespace + " ";
          foreach (var child in suite.Childs)
          {
            if (child.GetType().IsAssignableFrom(typeof (Suite)))
            {
              run(child);
            }

            if (child.GetType().IsAssignableFrom(typeof (Specification)))
            {

              if (suite.BeforeEach.Count > 0)
              {
                whitespace = whitespace + " ";
                suite.BeforeEach.ForEach(x =>
                {
                  x.ExecutionStatus = ExecStatus.Running;
                  Console.WriteLine(whitespace + x.Description);
                  x.Fn();
                  x.ExecutionStatus = ExecStatus.Completed;
                });
              }

              try
              {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write(whitespace + child.Description);

                child.StartTime = DateTime.Now;

                if (child.Enabled )
                {
                  child.ExecutionStatus = ExecStatus.Running;
                  child.Fn();
                  child.RanSuccesfully = true;
                }
                else
                {
                  Console.ForegroundColor = ConsoleColor.DarkYellow;
                  Console.Write(" || Pending ");
                  child.RanSuccesfully = false;
                }

              }
              catch (Exception e)
              {
                child.ExecutionResult = e.Message;
                child.RanSuccesfully = false;
                child.Parent.RanSuccesfully = false;
              }
              finally
              {
                child.EndTime = DateTime.Now;
                child.ExecutionStatus = ExecStatus.Completed;
              }

              if (child.RanSuccesfully && child.Enabled)
              {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" \u2713 ");
              }
              else if (!child.RanSuccesfully && child.Enabled)
              {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(" \uFF58" + whitespace + child.ExecutionResult);
              }
              Console.ForegroundColor = ConsoleColor.Gray;
              Console.WriteLine();

              if (suite.AfterEach.Count > 0)
              {
                suite.AfterEach.ForEach(x =>
                {
                  x.ExecutionStatus = ExecStatus.Running;
                  Console.WriteLine(whitespace + x.Description);
                  x.Fn();
                  x.ExecutionStatus = ExecStatus.Completed;
                });

              }
              whitespace = whitespace.Substring(0, whitespace.Length - 1);
            }
          }
          whitespace = whitespace.Substring(0, whitespace.Length==0?whitespace.Length:whitespace.Length - 1);
        }

        if (suite.AfterAll.Count > 0)
        {
          suite.AfterAll.ForEach(x =>
          {
            Console.WriteLine(whitespace + x.Description);
            x.ExecutionStatus = ExecStatus.Running;
            x.Fn();
            x.ExecutionStatus = ExecStatus.Completed;
          });

        }

        whitespace = whitespace.Substring(0, whitespace.Length > 0 ? whitespace.Length - 1 : whitespace.Length);
      }
      catch (Exception ex)
      {
        Console.WriteLine("General Error" + ex.Message);
        throw;
      }
      finally
      {
        suite.ExecutionStatus = ExecStatus.Completed;
      }
    }
  }
}