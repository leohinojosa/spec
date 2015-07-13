using System;

namespace spec.runner.Model
{
  [Serializable]
  public class SpecSummary
  {
    public ExecStatus ExecutionStatus { get; set; }
    public bool RanSuccesfully { get; set; }
    public string ExecutionResult { get; set; }
    public string Id { get; set; }
    public bool Enabled { get; set; }
    public TimeSpan Duration { get; set; }
  }
}