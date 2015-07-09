using FluentAssertions;

namespace spec
{
  public class TestSubject : spec
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

  public class TestSubject2 : spec
  {
    public TestSubject2()
    {
      describe("#Create Button", () =>
      {
        context("Create Functionality", () =>
        {
          bool? test = null;
          beforeAll("setup for All it", () =>
          {
            test = null;
          });

          beforeEach("setup for each it", () =>
          {
            test = true;
          });

          it("display when clicking button", () =>
          {
            test.Should().BeTrue();
          });

          it("display when clicking button 2", () =>
          {
            test = false;
            test.Should().BeFalse();
          });

          describe("when button is disabled", () =>
          {
            beforeEach("nested setup for each it", () =>
            {
              test = true;
            });

            it("should not be visible", () =>
            {
              test.Should().BeTrue();
              test = false;
              test.Should().BeFalse();
            });

            afterEach("nested teardown for each it", () =>
            {

            });
          });

          xdescribe("disabled suite - when button is disabled", () =>
          {
            it("should not be visible", () =>
            {
              false.Should().BeFalse("is not visible");
            });
          });

          afterEach("teardown for each it", () =>
          {
            test = false;
          });

          afterAll("teardown for all it", () =>
          {
            test = null;
          });
        });
      });
    }
  }
}