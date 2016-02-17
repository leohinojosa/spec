using FluentAssertions;
using spec;
using spec.core;

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


      describe("A spec using a beforeach and aftereach", () =>
      {
        var foo = 0;
        beforeEach(() =>
        {
          foo += 1;
        });

        afterEach(() =>
        {
          foo = 0;
        });

        it("is just a function, so it can contain any code", () =>
        {
          foo.Should().Be(1);
        });

        it("can have more than one expectation", () =>
        {
          foo.Should().Be(1);
        });

      });

        describe("A spec using a beforeach and aftereach", () =>
        {
            var foo = 0;
            beforeAll(() => { foo = 1; });

            afterAll(() => { foo = 0; });

            it("sets the initial value of foo before specs run", ()=> {
                foo.Should().Be(1);
                foo += 1;
            });

            it("does not reset foo between specs", ()=> {
                foo.Should().Be(2);
            });
        });



        describe("A spec", () =>{
            int? foo = null;

            beforeEach(()=> {
                foo = 0;
                foo += 1;
            });

            afterEach(()=> {
                foo = 0;
            });

            it("is just a function, so it can contain any code", () => {
                foo.Should().Be(1);
            });

            it("can have more than one expectation", ()=> {
                foo.Should().Be(1);
                true.Should().BeTrue();
            });

            describe("nested inside a second describe", ()=> {
                int? bar = null;

                beforeEach(()=> {
                    bar = 1;
                });

                it("can reference both scopes as needed", ()=> {
                    foo.Should().Be(bar);
                });
            });
        });

        xdescribe("A spec", () => {
            int? foo = null;

            beforeEach(() => {
                foo = 0;
                foo += 1;
            });

            it("is just a function, so it can contain any code", () => {
                foo.Should().Be(1);
            });
        });
        
        describe("Pending specs", () =>
        {
            xit("can be declared 'xit'", () =>{
                true.Should().Be(false);
            });

            it("can be declared with 'it' but without a function");
        });
    }
  }
}