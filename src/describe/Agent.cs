using System;
using System.Collections.Generic;
using spec.Model;

namespace spec
{
  public class Agent
  {
    public void RunSuite(Definition suite)
    {
      try
      {
        suite.ExecutionStatus = ExecStatus.Running;
        SetupSpec(suite.BeforeAll);
        RunChildDefinitions(suite);
        SetupSpec(suite.AfterAll);
      }
      finally
      {
        suite.ExecutionStatus = ExecStatus.Completed;
      }
    }

    private void RunChildDefinitions(Definition definition)
    {
      if (definition.Children.Count > 0)
      {
        foreach (var child in definition.Children)
        {
          if (child.GetType().IsAssignableFrom(typeof (Suite)))
          {
            RunSuite(child);
          }

          if (child.GetType().IsAssignableFrom(typeof (Specification)))
          {
            RunSpec(definition, child);
          }
        }
      }
    }

    private void RunSpec(Definition suite, Definition child)
    {
      SetupSpec(suite.BeforeEach);

      SafeExecute(child);

      SetupSpec(suite.AfterEach);
    }

    private void SafeExecute(Definition child)
    {
      try
      {
        child.StartTime = DateTime.Now;
        if (child.Enabled)
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