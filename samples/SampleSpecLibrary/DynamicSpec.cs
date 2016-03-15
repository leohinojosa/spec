using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using spec.core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleSpecLibrary
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
                        System.Threading.Thread.Sleep(1000);
                        (i % 2).Should().Be(0);
                    });
                });

                Enumerable.Range(1, 10).ToList().ForEach(x =>
                {
                    it("the int " + x, () =>
                    {
                        Assert.IsTrue(x > 0);
                    });
                });
            });
        }
    }
}
