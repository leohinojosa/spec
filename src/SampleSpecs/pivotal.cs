using FluentAssertions;
using spec;
using spec.core;

namespace SampleSpecs
{
  public class pivotal : Spec
  {
    public pivotal()
    {
      describe("A suite", () =>
      {
        it("contains spec with an expectation", () =>
        {
          true.Should().Be(true);
        });
      });

      describe("A suite is just a function", () =>
      {
        bool a;

        it("and so is a spec", () =>
        {
          a = true;
          a.Should().Be(true);
        });
      });


      describe("The 'toBe' matcher compares with ===", () =>
      {

        it("and has a positive case", () =>
        {
          true.Should().Be(true);
        });

        it("and can have a negative case", () =>
        {
          false.Should().BeFalse();
        });
      });


      describe("A spec using a beforeach and aftereach", () =>
      {
        var foo = 0;
        beforeEach(() =>
        {
          foo += 1;
        });

        afterEach(() =>
        {
          foo = 0;
        });

        it("is just a function, so it can contain any code", () =>
        {
          foo.Should().Be(1);
        });

        it("can have more than one expectation", () =>
        {
          foo.Should().Be(1);
        });

      }
    );
    }
  }
}