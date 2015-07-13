using System;
using System.Collections.Generic;

namespace spec.runner.Model
{
  [Serializable]
  public class TestSummary 
  {
    public int total { get; set; }
    public int passed { get; set; }
    public int failed { get; set; }
    public int pending { get; set; }
    public List<SpecSummary> specs { get; set; }
  }
}