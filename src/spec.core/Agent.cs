using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using spec.core.Model;

namespace spec.core
{
    public class Agent
    {
        public void RunSuite(Spec suite)
        {
            var mainSuiteExecutionPlan = new SortedList<string, List<Definition>>();
           // var mainSuiteExecutionPlan = new Dictionary<string, List<Definition>>();
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
                    //Si es un before all, que no se ha executado continua,
                    //si es un beforeall que ya se executo, no lo corras
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
                            //If its not the Last AFter all of the spec, skip execution until it is.
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