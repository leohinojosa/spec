using Microsoft.VisualStudio.TestTools.UnitTesting;
using spec.core;

namespace SampleSpecLibrary
{
    public class tests : Spec
    {
        public tests()
        {
            describe("This is a sample spec", () =>
            {
                it("Should test for a true value", () =>
                {
                    Assert.IsTrue(true);
                });

                it("Should not really test anything");

                it("run a validation", ()=>
                {
                    Assert.IsFalse(true);
                });
            });
        }
    }
}
