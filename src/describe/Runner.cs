using System;
using System.Collections.Generic;
using spec.Model;

namespace spec
{
  public class Runner
  {
    public void run(Definition suite)
    {
      try
      {
        suite.ExecutionStatus = ExecStatus.Running;
        SetupSpec(suite.BeforeAll);

        if (suite.Childs.Count > 0)
        {
          foreach (var child in suite.Childs)
          {
            if (child.GetType().IsAssignableFrom(typeof (Suite)))
            {
              run(child);
            }

            if (child.GetType().IsAssignableFrom(typeof (Specification)))
            {
              SetupSpec(suite.BeforeEach);

              try
              {
                child.StartTime = DateTime.Now;

                if (child.Enabled )
                {
                  child.ExecutionStatus = ExecStatus.Running;
                  child.Fn();
                  child.RanSuccesfully = true;
                }
                else
                {
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

              SetupSpec(suite.AfterEach);
            }
          }
        }

        SetupSpec(suite.AfterAll);
      }
      finally
      {
        suite.ExecutionStatus = ExecStatus.Completed;
      }
    }

    private void SetupSpec(List<Each> setUp)
    {
      if (setUp.Count > 0)
      {
        setUp.ForEach(x =>
        {
          x.ExecutionStatus = ExecStatus.Running;
          x.Fn();
          x.ExecutionStatus = ExecStatus.Completed;
        });
      }
    }
  }
}