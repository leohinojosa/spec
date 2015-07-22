
#Spec#
Spec is a small testing framework that implements **bdd** style testing for .net. inspired by mochajs and jasmine.

The library takes a minimal approach to define your tests and aligns to the same structure that *mochajs* and *jasmine* use; Allowing you to have a unified perspective in how you write and structure your tests.

The Spec TDD library works by creating any class that inherits from the Spec base type. Then create in the class constructor start describing the behavior and expectations for your tests.
You can have *describe* blocks, *context* blocks, and *it* specifications.

Also you can use hooks for **BeforeAll, BeforeEach, AfterAll **&** AfterEach**.

The library includes an integrated Test Runner for Visual Studio and a standalone Console Runner as well.

```csharp
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
```

##Assertion Libraries ##
Spec does not include an assertion library, you can use whatever assertion library you feel more comfortable with,

* Assert Library
* Fluent Assertions
* Shouldy
* Expect better
* NFluent

```csharp
describe("Multiple Assertion Libraries", () =>
{
  context("There are many assertion libraries out there:", ()=>
  {
    it("Fluent Assertions", () =>
    {
      new[] { 1, 5, 3 }.Any(x => x == 1).Should().BeFalse("Because it should fail");
    });

    it("Assert", () =>
    {
      Assert.IsFalse(true, "this is the regular assert library - failing ");
    });

    it("Expect", () =>
    {
      Expect.The(true).ToBeFalse();
    });

    it("Shouldy", () =>
    {
      true.ShouldBe(false);
    });

    it("NFluent", () =>
    {
      Check.That(true).IsFalse();
    });

  }
);
```

## Visual Studio Extensions that Help spec look better ##
Because the spec framework uses Action<> to define the tests, the following extensions allow for an easier IDE experience to outline describes and its, plus providing better visual support for indentation.

* C# outline 2013
* Indent Guides
