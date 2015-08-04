using System;
using FluentAssertions;
using spec;
using spec.core;

namespace SampleSpecs
{
  public class LongTest : Spec
  {
    public LongTest()
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
          t.Should().BeTrue("Should be true, but previous describe changed scoped value");
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
          t = false;
        });

        it("it 1 ---- " , () => 
        {
          t.Should().BeFalse();
        });

        afterEach("afterEach", () =>
        {
          t = true;
        });
      });

      describe("dynamic spec creation", () =>
      {
        int specCount = 2;
        for (int i = 0; i < specCount; i++)
        {
          it("it should be dynamic" + i.ToString(), () =>
          {
            true.Should().Be(true);
          });
        }
      });
    }
  }
}