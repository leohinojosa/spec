using Microsoft.VisualStudio.TestTools.UnitTesting;
using spec.core;

namespace SampleSpecs
{
  public class Calculator
  {
    public int Value { get; private set; }
    public void reset()
    {
      Value = 0;
    }

    public void Add(int x, int y)
    {
      Value = x + y;
    }

    public void Subtract(int x, int y)
    {
      Value = x - y;
    }
  }


  public class CalculatorSpec : Spec
  {
    public CalculatorSpec()
    {
      describe("Parent Describe block", () =>
      {
        Calculator calc = null;
        beforeAll(() =>
        {
          calc = new Calculator();
        });

        describe("Test add", () =>
        {
          beforeEach("Setup at 10", () =>
          {
            calc.reset();
          });

          it("Should check correct value", () =>
          {
            calc.Add(0,10);
            Assert.AreEqual(calc.Value, 10,"Should be greater than zero");
          });
        });

        describe("Test subtract", () =>
        {
          beforeEach("Setup at -10", () =>
          {
            calc.reset();
          });

          it("Should check correct value", () =>
          {
            calc.Subtract(0, 10);
            Assert.AreEqual(calc.Value, -10, "Should be zero");
          });

          afterEach(() =>
          {
            calc.reset();
          });
        });

        describe("at the end the calculator", () =>
        {
          it("should be re-set", (() =>
          {
            Assert.AreEqual(calc.Value, 0, "Should be reset");
          }));
        });

        afterAll(() =>
        {
          calc = null;
        });
      });
    }
  }
}