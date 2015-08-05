
#Spec#
Spec is a small testing framework that implements **bdd** style testing for .net. inspired by [mochajs](http://mochajs.org/) and [jasmine](http://jasmine.github.io/).
The library takes a minimal approach to define your tests and allows for using a *Arrange-Act-Assert* pattern in your unit testing;

The Spec TDD library works by creating any class that inherits from the `Spec` base type. Create a constructor and start describing the behavior and expectations for your specs.
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

The library includes an integrated Test Runner for Visual Studio in a visx package and a standalone Console Runner exe as well.

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

## Support for dynamic tests ##
Spec supports creating **it** inside a List<T>, this way you can add different test cases and have them run with fixture data.

```csharp
describe("Dynamic spec creation", () =>
{
  new List<int>(){0,2,4}.ForEach(i =>
  {
    it("it should be dynamic " + i.ToString(), () =>
    {
      Console.WriteLine(i);
      System.Threading.Thread.Sleep(1000);
      (i%2).Should().Be(0);
    });
  });
});
```

## Visual Studio Extensions that Help spec look better ##
Because the spec framework uses ````Action<>```` to define the tests, the following extensions allow for an better IDE experience to outline *describe*, *context* and *it*, plus providing better visual support for indentation.

* [C# outline 2013](https://visualstudiogallery.msdn.microsoft.com/4d7e74d7-3d71-4ee5-9ac8-04b76e411ea8)
* [Indent Guides](https://visualstudiogallery.msdn.microsoft.com/e792686d-542b-474a-8c55-630980e72c30)
* [spec.testAdapter](https://visualstudiogallery.msdn.microsoft.com/c2e17e64-b57f-4065-9b8b-20ea9e8623d7)

## Integrating spec to your project pipeline ##
Consider downloading the spec.testAdapter from the [Visual Studio Extensions](https://visualstudiogallery.msdn.microsoft.com/c2e17e64-b57f-4065-9b8b-20ea9e8623d7) site. Once you instal the vsix it should be available in the visual studio IDE and on the console.

To run it in console use the following parameters
```
"c:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" SampleSpecs.dll /UseVsixExtensions:true /logger:TRX
```

This will instruct the test runner to load external runners to execute the test files.
