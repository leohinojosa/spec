using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using spec.core.Model;

namespace spec.core
{
    public class Agent
    {
        public void RunSuite(Spec suite)
        {
            var mainSuiteExecutionPlan = new SortedList<string, List<Definition>>();
            foreach (var spec in suite.Registry.ExecutableLookupTable)
            {
                mainSuiteExecutionPlan.Add(spec.Id, new List<Definition>());
                List<Definition> executionPlan = mainSuiteExecutionPlan[spec.Id];
                executionPlan.Add(spec);
                AddBeforeEachs(spec, executionPlan);              
                AddAfterEach(spec, executionPlan);
            }

            foreach (var executionPlan in mainSuiteExecutionPlan)
            {
                foreach (var definition in executionPlan.Value)
                {
                    if(definition.GetType() == typeof(GlobalHook))
                    {
                        var globalHook = (GlobalHook) definition;
                        if (globalHook.Kind == GlobalHookKind.BeforeAll)
                        {
                            if (globalHook.ExecutionStatus != ExecStatus.NotRun)
                                continue;
                        }

                        if (globalHook.Kind == GlobalHookKind.AfterAll)
                        {
                            var max = mainSuiteExecutionPlan.SelectMany(x => x.Value, (x, y) => new
                            {
                                x.Key,
                                val=y,
                                idx=mainSuiteExecutionPlan.IndexOfKey(x.Key)
                            })
                            .Where(x=>x.val==definition)
                            .Max(x=>x.idx);
                            var index = mainSuiteExecutionPlan.IndexOfKey(executionPlan.Key);
                            if (index<max)
                            {
                                continue;
                            }
                        }

                    }
                    SafeExecute(definition);
                }
                
            }
        }

        private void AddAfterEach(Definition spec, List<Definition> specifications)
        {
            if (spec.AfterEach.Count > 0)
            {
                foreach (var hook in spec.AfterEach)
                {
                    specifications.Add(hook);
                }
            }

            if (spec.AfterAll.Count > 0)
            {
                foreach (var hook in spec.AfterAll.AsEnumerable())
                {
                    specifications.Add(hook);
                }
            }

            if (spec.Parent != null)
            {
                AddAfterEach(spec.Parent, specifications);
            }
        }

        private void AddBeforeEachs(Definition spec, List<Definition> specifications)
        {
            //insert current beforeachs
            if (spec.BeforeEach.Count > 0)
            {
                foreach (var hook in spec.BeforeEach.AsEnumerable().Reverse())
                {
                    specifications.Insert(0, hook);
                }
            }

            if (spec.BeforeAll.Count > 0)
            {
                foreach (var hook in spec.BeforeAll.AsEnumerable().Reverse())
                {
                    specifications.Insert(0, hook);
                }
            }

            if (spec.Parent != null)
            {
                AddBeforeEachs(spec.Parent, specifications);
            }
        }

        private static bool IsAsyncAppliedToDelegate(Delegate d)
        {
            return d.Method.GetCustomAttribute(typeof (AsyncStateMachineAttribute)) != null;
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
                        child.Fn1 = async () => { child.Fn(); };
                        asyncExecute(child);
                    }
                    else
                    {
                        execute(child);
                    }
                    child.RanSuccessfully = true;
                }
                else
                {
                    child.RanSuccessfully = false;
                }
            }
            catch (Exception e)
            {
                child.ExecutionResult = e.Message;
                child.RanSuccessfully = false;
                child.Parent.RanSuccessfully = false;
                child.StackTrace = e.StackTrace;
            }
            finally
            {
                child.EndTime = DateTime.Now;
                child.ExecutionStatus = ExecStatus.Completed;
            }
        }
    }
}