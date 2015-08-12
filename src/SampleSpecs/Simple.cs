using System;
using System.Diagnostics;
using System.Linq;
using ExpectBetter;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using spec;
using spec.core;
using Shouldly;

namespace SampleSpecs
{
  public class Simple : Spec
  {
    public Simple()
    {
      describe("Assertions", () =>
      {
        xdescribe("IndexOf", () =>
        {
          it("should be false when the value is not present", () =>
          {
            new[] {1, 5, 3}.Any(x => x == 0).Should().BeFalse();
            new[] {5, 9, 10}.Any(x => x == 8).Should().BeFalse();
          });
        });

        describe("Multiple Assertion Libraries", () =>
        {
          context("There are many assertion libraries out there:", ()=>
          {
            it("Fluent Assertions", () =>
            {
              new[] { 1, 5, 3 }.Any(x => x == 1).Should().BeFalse("Because it should fail");
            });

            it("Assert", () =>
            {
              Assert.IsFalse(true, "this is the regular assert library - failing ");
            });

            it("Expect", () =>
            {
              Expect.The(true).ToBeFalse();
            });

            it("Shouldy", () =>
            {
              true.ShouldBe(false);
            });

            it("NFluent", () =>
            {
              Check.That(true).IsFalse();
            });

          }
        );
          
        });
      });
    }
  }
}