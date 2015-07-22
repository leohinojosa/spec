using System;
using System.Collections.Generic;

namespace spec.core.Model
{
  [Serializable]
  public class Definition
  {
    public Definition Parent { get; set; }
    public List<Hook> BeforeEach { get; private set; }
    public List<Hook> AfterEach { get; private set; }
    public List<Hook> BeforeAll { get; private set; }
    public List<Hook> AfterAll { get; private set; }
    public List<Definition> Children { get; set; }

    public string Id { get; set; }
    public string Description { get; set; }
    public Action Fn { get; set; }
    public bool Enabled { get; set; }

    public ExecStatus ExecutionStatus { get; set; }
    public bool RanSuccesfully { get; set; }
    public string ExecutionResult { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime StartTime { get; set; }
    public string StackTrace { get; set; }

    public Definition ()
    {
      Children = new List<Definition>();
      BeforeEach = new List<Hook>();
      AfterEach = new List<Hook>();

      BeforeAll = new List<Hook>();
      AfterAll = new List<Hook>();
    }
    public void AddChild(Definition spec)
    {
      Children.Add(spec);
    }

    public void AddBeforeEach(Hook hook)
    {
      this.BeforeEach.Add(hook);
    }
    public void AddAfterEach(Hook hook)
    {
      this.AfterEach.Add(hook);
    }
    public void AddBeforeAll(Hook hook)
    {
      this.BeforeAll.Add(hook);
    }
    public void AddAfterAll(Hook hook)
    {
      this.AfterAll.Add(hook);
    }

    public override string ToString()
    {
      return Description;
    }
  }
}