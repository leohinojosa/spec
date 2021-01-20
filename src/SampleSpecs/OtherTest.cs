using FluentAssertions;
using spec.core;

namespace SampleSpecs
{
  public class OtherTest : Spec
  {
    public OtherTest()
    {
      describe("one describe", () =>
      {
        it("one it", () =>
        {
          true.Should().BeTrue();
        });
      });

      describe("two describe", () =>
      {
        it("second it", () =>
        {
          true.Should().BeTrue();
        });
      });
    }
  }
}