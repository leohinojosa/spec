using FluentAssertions;

namespace SampleSpecs
{
  public class TestSubject2 : spec.spec
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

            it("should fails", () =>
            { 
              test = false;
              test.Should().BeTrue();
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