using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using FluentAssertions;
using spec;
using spec.core;

namespace SampleSpecs
{
  public class AfterEachSubjectTest : Spec
  {
    public AfterEachSubjectTest()
    {
      describe("#Test AfterEach being called only once",()=>
      {
        List<int> testList = null;
        context("The beforeEach suite should consider being called for every spec", () =>
        {
          beforeAll("[before all] - List should be initialized", () =>
          {
            testList = new List<int>();
            testList.Add(1);
          });

          beforeEach("[before each]- Add Item to list", () =>
          {
            testList.Add(2);
          });

          it(" [it] List should have 2 items", () =>
          {
            testList.Count.Should().Be(2);
          });

         afterEach("[after each] - Remove item from the list", () =>
         {
           testList = null;
         });            
        });

        it(" [it] should be null", () =>
        {
          testList.Should().BeNull();
        });
      }
        );
    }
  }
}