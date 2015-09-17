using System;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentAssertions;
using spec.core;

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

        beforeEach(() =>
        {
          sut = new AsyncObject();
        });

        it("when calling directly Result from Async task it should return value", () =>
        {
          var result = sut.RunAsync(10,resultValue).Result;
          result.Should().Be(resultValue);
        });

        it("async should assert incorrect value",  () =>
        {
          Func<Task<int>> act = async () => await sut.RunAsync(10, resultValue);
          act().Result.Should().Be(resultValue+15);
        });

        it("async should return correct value, faking async", () =>
        {
          //correct syntax
          Func<Task<int>> act = async () => await sut.RunAsync(10, resultValue);
          act().Result.Should().Be(resultValue);
           
       /*   //incorrect syntax
          var result = sut.RunAsync(10, resultValue);
          result.Should().Be(resultValue);*/
        });

        ///async keyword is not supported
        it("async should return correct value, with real async", (async () => 
        {
          var result = await sut.RunAsync(10, resultValue);
          result.Should().Be(resultValue );
          //problem with this is when we get an exception, it breaks the runner because the exception originates in a separate thread pool
        }));

        it("should throw exception", () =>
        {
          Func<Task> act = async () => { await sut.RunAsyncException(10, resultValue); };
          act.ShouldThrow<Exception>("because its expected");
        });

        it("it should display an incorrect spec that throws an exception", () =>
        {
          Func<Task> act = async () => { await sut.RunAsync(10, resultValue); };
          act.ShouldThrow<Exception>("because its expected");
        });

      });
    }
  }

  public class AsyncObject
  {
    
    public async Task<int> RunAsync(int milliseconds, int resultValue)
    {
      await Task.Delay(milliseconds);
      return resultValue;
    }

    public async Task<int> RunAsyncException(int milliseconds, int resultValue)
    {
      await Task.Delay(milliseconds);
      throw new Exception("It should explode");
      return resultValue;
    }
  }
}