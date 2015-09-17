using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using spec.core.Model;

namespace spec.core
{
  public class Agent
  {
    public void RunSuite(Definition suite)
    {
      try
      {
        suite.ExecutionStatus = ExecStatus.Running;
        SetupHooks(suite.BeforeAll);
        RunChildDefinitions(suite);
        SetupHooks(suite.AfterAll);
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
      SetupHooks(suite.BeforeEach);

      SafeExecute(child);

      SetupHooks(suite.AfterEach);
    }
    
    private static bool IsAsyncAppliedToDelegate(Delegate d)
    {
      return d.Method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;
    }

    private void execute(Definition child)
    {
      child.Fn();
    }

    private async void asyncExecute(Definition child)
    {
      await child.Fn1();
    }

    private void SafeExecute(Definition child)
    {
      try
      {
        child.StartTime = DateTime.Now;
        if (child.Enabled)
        {
          child.ExecutionStatus = ExecStatus.Running;
          if (IsAsyncAppliedToDelegate(child.Fn))
          {
            child.Fn1 =  async () =>{child.Fn();};
            asyncExecute(child);  
          }
          else
          {
            execute(child);
          }
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
        child.StackTrace = e.StackTrace;
      }
      finally
      {
        child.EndTime = DateTime.Now;
        child.ExecutionStatus = ExecStatus.Completed;
      }
    }

    private void SetupHooks(List<Hook> setUp)
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