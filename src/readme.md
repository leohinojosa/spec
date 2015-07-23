
#Spec#
Spec is a small testing framework that implements **bdd** style testing for .net. inspired by [mochajs](http://mochajs.org/) and [jasmine](http://jasmine.github.io/).

The library takes a minimal approach to define your tests and aligns to the same structure that *mochajs* and *jasmine* use; Allowing you to have a unified perspective in how you write and structure your tests.

The Spec TDD library works by creating any class that inherits from the `Spec` base type. Create a constructor, start describing the behavior and expectations for your tests.
You can have *describe* blocks, *context* blocks, and *it* specifications.

```csharp
public class SimpleTest : Spec
{
  public SimpleTest()
  {
      describe("A describe suite", ()=>{
          var scopeVariable = false;
          it("can have multiple specs", () =>
          {
            Assert.IsTrue(true);
          });

          xit("or disabled specs", () =>
          {
            Assert.IsFalse(true);
          });
      });
  }
}
```

Also you can use hooks for **BeforeAll, BeforeEach, AfterAll **&** AfterEach**.


```csharp
public class SimpleTest : Spec
{
  public SimpleTest()
  {
      describe("A describe suite", ()=>{
          var scopeVariable = false;
          beforeEach(() =>
          {
            scopeVariable = true;
          });

          it("can have multiple specs", () =>
          {
            Assert.IsTrue(true);
          });

          afterEach(() =>
          {
            scopeVariable = false;
          });
      });
  }
}
```
BeforeAll and AfterAll hooks will be run once per **describe**, while beforeEach and AfterEach will run once per every **it**. Also you can have multiple hooks per spec, which will be appended and executed as discovered.


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
      });
    }
  }
}
```

The library includes an integrated Test Runner for Visual Studio and a standalone Console Runner as well.

##Assertion Libraries ##
Spec does **not** include an assertion library, you can use whatever assertion library you feel more comfortable with.

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
