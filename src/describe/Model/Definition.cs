using System;
using System.Collections.Generic;

namespace spec.Model
{
  [Serializable]
  public class Definition
  {
    public Definition Parent { get; set; }
    public List<Each> BeforeEach { get; private set; }
    public List<Each> AfterEach { get; private set; }
    public List<Each> BeforeAll { get; private set; }
    public List<Each> AfterAll { get; private set; }
    public List<Definition> Childs { get; set; }

    public string Id { get; set; }
    public string Description { get; set; }
    public Action Fn { get; set; }
    public bool Enabled { get; set; }

    public ExecStatus ExecutionStatus { get; set; }
    public bool RanSuccesfully { get; set; }
    public string ExecutionResult { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime StartTime { get; set; }

    public Definition ()
    {
      Childs = new List<Definition>();
      BeforeEach = new List<Each>();
      AfterEach = new List<Each>();

      BeforeAll = new List<Each>();
      AfterAll = new List<Each>();
    }
    public void AddChild(Definition spec)
    {
      Childs.Add(spec);
    }

    public void AddBeforeEach(Each each)
    {
      this.BeforeEach.Add(each);
    }
    public void AddAfterEach(Each each)
    {
      this.AfterEach.Add(each);
    }
    public void AddBeforeAll(Each each)
    {
      this.BeforeAll.Add(each);
    }
    public void AddAfterAll(Each each)
    {
      this.AfterAll.Add(each);
    }

    public override string ToString()
    {
      return Description;
    }
  }
}