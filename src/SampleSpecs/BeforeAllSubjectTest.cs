using System.Collections.Generic;
using FluentAssertions;
using spec;
using spec.core;

namespace SampleSpecs
{
  public class BeforeAllSubjectTest : Spec
  {
    public BeforeAllSubjectTest()
    {
      describe("#Test BeforeAll being called only once",()=>
      {
        context("The beforeAll suite should consider being called only once per suite", () =>
        {
          List<int> testList = null;
          beforeAll("[before all] - List should be initialized", () =>
          {
            testList = new List<int>();
            testList.Add(1);
          });

          beforeEach("[before each]- Add Item to list", () =>
          {
            testList.Add(2);
          });

          beforeEach("[before each]- Add Item to list", () =>
          {
            testList.Add(2);
          });

          it(" [it] List should have 2 items", () =>
          {
            testList.Count.Should().Be(3);
          });

          it(" [it] List should have 3 items", () =>
          {
            testList.Count.Should().Be(5);
          });
            
        });
      }
        );
    }
  }
}