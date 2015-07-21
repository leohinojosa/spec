using System.Linq;
using ExpectBetter;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            it("Should be false when the value is not present", () =>
            {
              new[] { 1, 5, 3 }.Any(x => x == 0).Should().BeFalse();
              new[] { 5, 9, 10 }.Any(x => x == 8).Should().BeFalse();
            });

            it("Assert", () =>
            {
              Assert.IsFalse(true, "this is the regular assert library - failing ");
            });

            it("Expect", () =>
            {
              Expect.The(true).ToBeFalse();
            });

          }
        );
          
        });
      });
    }
  }
}