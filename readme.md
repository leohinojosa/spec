# Spec #
Spec is a small testing framework that implements **bdd** style testing for .net. it is mainly inspired by [mochajs](http://mochajs.org/) and [jasmine](http://jasmine.github.io/).
The library takes a minimal low ceremony approach to define your tests and allows for using a *Arrange-Act-Assert* pattern in your unit testing;

[![Build status](https://ci.appveyor.com/api/projects/status/a65uirokpr22l75d?svg=true)](https://ci.appveyor.com/project/leohinojosa/spec)

## Set up ##
Install the nuget package in your project. Open the Package manager console and type:

```
PM> Package-Install spec.core
```

This will install the ```Spec``` base class that allows you to write your tests.

In order to be able to run the tests you need to install the vsix extension. You can downlad the Spec.TestAdapter [here](https://visualstudiogallery.msdn.microsoft.com/c2e17e64-b57f-4065-9b8b-20ea9e8623d7). After you instal the vsix file, your tests should be discovered by the Visual Studio Test Runner.

---
## Basics ##

The Spec TDD library works by creating any class that inherits from the `Spec` base type.

Then you define your specs in the class constructor.
You can have *describe* blocks, *context* blocks, and *it* to define specifications.

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

Also you can use hooks for **BeforeAll**, **BeforeEach**, **AfterAll** & **AfterEach**.


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

          it("scope variable should be set in each run", () =>
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
          beforeAll(() =>
          {
            testList = new List<int>();
            testList.Add(1);
          });

          beforeEach(() =>
          {
            testList.Add(2);
          });

          it(" [it] List should have 2 items", () =>
          {
            testList.Count.Should().Be(2);
          });

         afterEach(() =>
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
----

## Assertion Libraries ##
Spec does **not** include an assertion library, you can use whatever assertion library you feel more comfortable with,

* Standard Assert Library
* [Fluent Assertions](http://www.fluentassertions.com/)
* [Shouldy](http://docs.shouldly-lib.net/)
* [Expect better](https://github.com/benjamin-bader/ExpectBetter)
* [NFluent](http://n-fluent.net/)

```csharp
describe("Multiple Assertion Libraries", () =>
{
  context("There are many assertion libraries out there:", () =>
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
)};
```
*Note*: Pretty much anything that throws an exception should work.

---
## Support for async/await ##
Async/Await methods can be tested by making the it lamba async, and awaiting on the applicable method.
```csharp
    it("it should be async ", () => async
    {
        var sut = new Class();
        var result = await sut.asyncMethod();
        result.Should().BeTrue();
    });
```

In async/await, exceptions are a bit harder to catch because they run in a separate application thread. In order to catch them you must wrap them in a Func and then execute it, otherwise the test runner fails and exits.
```csharp
    it("An async exception has to be catched with a special idiom", () =>
    {
        var sut = new Class();
        Func<Task> a = async () => { await sut.asyncMethodException(); };
        a.ShouldThrow<Exception>();
    });
```
---
## Support for Dynamic Tests Generation ##
Spec supports creating **it** statements inside a List, this way you can add different test cases and have them run with fixture data.

```csharp
describe("Dynamic spec creation", () =>
{
  new List<int>(){0,2,4}.ForEach(i =>
  {
    it("it should be dynamic " + i.ToString(), () =>
    {
      System.Threading.Thread.Sleep(1000);
      (i%2).Should().Be(0);
    });
  });
});
```
---
## Base Spec Classes ##
Because the spec runner loads types that inherit the spec type, you can create your own base clases that implement specific behaviors that you want to abstract. You can even write regular hooks in the root class that will be executed as part of the spec.

## TDD Interface ##
Spec also supports a TDD interface by inheriting from the Test base class.The Tdd Interface maps *describe* to *suite*, and *test* to *it*.*Setup* maps to *beforeEach* and *tearDown* to *afterEach*.
```csharp
public class TddStyle : Test
{
  public TddStyle()
  {
    suite("To create a TDD Suite", () =>
    {
      string subject = string.Empty;
      setup(() =>
      {
        subject = "initialized";
      });
      test("test subject is initialized", ()=>
      {
        subject.Should().Be("initialized");
      });
    });
  }
}
```
---
## Visual Studio Extensions that Help spec look better ##
Because the spec framework uses ````Action<>```` to define the tests, the following extensions allow for an better IDE experience to outline *describe*, *context* and *it*, plus providing better visual support for indentation.

* [C# outline 2013](https://visualstudiogallery.msdn.microsoft.com/4d7e74d7-3d71-4ee5-9ac8-04b76e411ea8)
* [Indent Guides](https://visualstudiogallery.msdn.microsoft.com/e792686d-542b-474a-8c55-630980e72c30)
* [spec.testAdapter](https://visualstudiogallery.msdn.microsoft.com/c2e17e64-b57f-4065-9b8b-20ea9e8623d7)

![test_console](https://cloud.githubusercontent.com/assets/1530791/9564848/79636124-4e68-11e5-97ee-8f5743df6291.png)

