using System;
using System.Threading.Tasks;
using spec.core;
using Shouldly;

namespace SampleSpecs
{
    public class AsyncSpec : Spec
    {
        public AsyncSpec()
        {
            describe("A async spec", () =>
            {
                AsyncObject sut = null;
                var resultValue = 1000;

                beforeEach(() => { sut = new AsyncObject(); });

                it("An async metod can be called and waited with " +
                   "the async keyword in the spec lambda", async () =>
                {
                    var result = await sut.RunAsync(1000, 1);
                    result.ShouldBe(1);
                });

                it("An async exception has to be catched with a special idiom", () =>
                {
                    Func<Task> a = async () => { await sut.RunAsyncException(10, resultValue); };
                    a.ShouldThrow<Exception>();
                });
            });
        }
    }

    public class AsyncObject
    {
        public async Task<int> RunAsync(int milliseconds, int resultValue)
        {
            var result = 0;
            await Task.Delay(milliseconds);
            result = resultValue;
            return result;
        }

        public async Task<int> RunAsyncException(int milliseconds, int resultValue)
        {
            await Task.Delay(milliseconds);
            throw new Exception("It should explode");
        }
    }
}