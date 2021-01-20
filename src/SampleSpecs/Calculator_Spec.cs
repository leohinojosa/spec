using spec.core;
using Xunit;

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
      describe("Calculator Suite", () =>
      {
        Calculator calc = null;

        context("Testing behavior of a calculator", () =>
        {
          beforeAll(() => { calc = new Calculator(); });

          describe("Test add", () =>
          {
            beforeEach("Setup at 10", () => { calc.reset(); });

            it("Should check correct value", () =>
            {
              calc.Add(0, 10);
              Assert.Equal(calc.Value, 10);
            });
          });

          describe("Test subtract", () =>
          {
            beforeEach("Setup at -10", () => { calc.reset(); });

            it("Should check correct value", () =>
            {
              calc.Subtract(0, 10);
              Assert.Equal(calc.Value, -10);
            });

            afterEach(() => { calc.reset(); });
          });

          describe("at the end the calculator",
            () => { it("should be re-set", (() => { Assert.Equal(calc.Value, 0); })); });

          afterAll(() => { calc = null; });
        });
      });
    }
  }
}