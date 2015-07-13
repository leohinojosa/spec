using FluentAssertions;
using spec;

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

    }
  }
}