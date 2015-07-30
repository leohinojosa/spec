using FluentAssertions;
using spec.core;

namespace SampleSpecs
{
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
        }
          );
      });
    }
  }
}