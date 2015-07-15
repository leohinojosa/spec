using System;
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
        if (suite.BeforeAll.Count > 0)
        {
          suite.BeforeAll.ForEach(x =>
          {
            x.ExecutionStatus = ExecStatus.Running;
            x.Fn();
            x.ExecutionStatus = ExecStatus.Completed;
          });
        }

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

              if (suite.BeforeEach.Count > 0)
              {
                suite.BeforeEach.ForEach(x =>
                {
                  x.ExecutionStatus = ExecStatus.Running;
                  x.Fn();
                  x.ExecutionStatus = ExecStatus.Completed;
                });
              }

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

              if (suite.AfterEach.Count > 0)
              {
                suite.AfterEach.ForEach(x =>
                {
                  x.ExecutionStatus = ExecStatus.Running;
                  x.Fn();
                  x.ExecutionStatus = ExecStatus.Completed;
                });

              }
            }
          }
        }

        if (suite.AfterAll.Count > 0)
        {
          suite.AfterAll.ForEach(x =>
          {
            x.ExecutionStatus = ExecStatus.Running;
            x.Fn();
            x.ExecutionStatus = ExecStatus.Completed;
          });

        }
      }
      finally
      {
        suite.ExecutionStatus = ExecStatus.Completed;
      }
    }
  }
}