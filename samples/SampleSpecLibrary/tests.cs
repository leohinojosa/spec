using Microsoft.VisualStudio.TestTools.UnitTesting;
using spec.core;
using System.Threading.Tasks;

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

            describe("Asyncronus tasks", () => {
                it("Should await the async call", async () =>
                {
                    var asyncObject = new AsyncSample();
                    Assert.IsTrue(await asyncObject.GetAsyncValue());
                });
            });
        }
    }

    public class AsyncSample
    {
        public async Task<bool> GetAsyncValue()
        {
            await Task.Delay(1000);
            return true;
        }
    }
}
