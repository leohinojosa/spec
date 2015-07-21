using System.Linq;
using FluentAssertions;
using spec;

namespace SampleSpecs
{
  class Simple : Spec
  {
    public Simple()
    {
      describe("Array", () =>
      {
        describe("IndexOf", () =>
        {
          it("should be false when the value is not present", () =>
          {
            new[] {1, 5, 3}.Any(x => x == 0).Should().BeFalse();
            new[] {5, 9, 10}.Any(x => x == 8).Should().BeFalse();
          });
        });

        xdescribe("IndexOf - Disabled", () =>
        {
          context("disabled context", ()=>
          {
            it("should be false when the value is not present", () =>
            {
              new[] { 1, 5, 3 }.Any(x => x == 0).Should().BeFalse();
              new[] { 5, 9, 10 }.Any(x => x == 8).Should().BeFalse();
            });
          }
        );
          
        });
      });
    }
  }
}