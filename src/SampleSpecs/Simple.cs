using System.Collections.Generic;
using System.Linq;
using ExpectBetter;
using FluentAssertions;
using NFluent;
using spec.core;
using Shouldly;
using Xunit;

namespace SampleSpecs
{

    public class DynamicSpec : Spec
    {
        public DynamicSpec()
        {
            describe("test describe", () =>
            {
                new List<int>() { 0, 2, 4 }.ForEach(i =>
                {
                    it("it should be dynamic " + i.ToString(), () =>
                    {
                        (i % 2).Should().Be(0);
                    });
                });

                Enumerable.Range(1, 10).ToList().ForEach(x =>
                {
                    it("the int " + x, () =>
                    {
                        Assert.True(x > 0);
                    });
                });
            });
        }
    }
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
                    context("There are many assertion libraries out there:", () =>
                    {
                        it("Fluent Assertions",
                            () => { new[] {1, 5, 3}.Any(x => x == 1).Should().BeFalse("Because it should fail"); });

                        it("Assert", () => { Assert.False(true, "this is the regular assert library - failing "); });

                        it("Expect", () => { Expect.The(true).ToBeFalse(); });

                        it("Shouldy", () => { true.ShouldBe(false); });

                        it("NFluent", () => { Check.That(true).IsFalse(); });
                    });
                });
            });
        }
    }
}