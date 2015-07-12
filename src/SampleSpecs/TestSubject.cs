using FluentAssertions;
using spec;

namespace SampleSpecs
{
  public class TestSubject : Spec
  {
    public TestSubject()
    {
      describe("describe with fancy lambda expression", _ =>
      {
        it("fancy", () => { true.Should().BeTrue(); });
      });

      describe("describe 1", () =>
      {
        bool t = true;

        beforeEach("Set up 1", () =>
        {
          t = false;
        });

        beforeEach("Set up 2", () =>
        {
          t = false;
        });

        describe("describe 1 - nested", () =>
        {
          beforeEach("nested setup 1", () =>
          {
            t = false;
          });

          it("nested it 1  - child", () =>
          {
            t.Should().BeFalse();
          });

          it("nested it 2 - child", () =>
          {
            t.Should().BeFalse();
          });

          afterEach("nested destroy", () =>
          {
            t = false;
          });

        });


        it("it 3", () =>
        {
          t.Should().BeTrue();
        });

        xit("it 4 - disabled", () =>
        {
          t.Should().BeFalse();
        });

        afterEach("Final After each", () =>
        {
          t = false;
        });

      });

      describe("describe 2", () =>
      {
        var t = true;
        beforeEach("before each", () =>
        {

        });

        it("it 1", () =>
        {
          true.Should().BeFalse();
        });

        afterEach("afterEach", () =>
        {

        });
      });
    }
  }
}