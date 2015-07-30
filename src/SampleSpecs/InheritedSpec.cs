using System;
using FluentAssertions;
using spec.core;

namespace SampleSpecs
{
  public class BaseSpec : Spec
  {
    protected bool t = false;
    public BaseSpec()
    {
      beforeAll(() =>
      {
        Console.Write("Initialize application ");
        t = true;
      });
    }
  }

  public class InheritedSpec : BaseSpec
  {
    public InheritedSpec()
    {
      describe("Inherited Suite", () =>
      {
        it("BeforeAll from Parent Type Suite should have been executed", () =>
        {
          t.Should().BeTrue();
        });
      });
    }
  }
}
