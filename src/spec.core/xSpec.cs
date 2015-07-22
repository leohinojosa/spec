using System;

namespace spec.core
{
  public abstract class xSpec : ISpec
  {
    public void beforeEach(Action operation)
    {
    }

    public void beforeAll(string description, Action operation)
    {
    }

    public void afterAll(string description, Action operation)
    {
    }

    public void beforeEach(string description, Action operation)
    {
    }

    public void afterEach(Action operation)
    {
    }

    public void afterEach(string description, Action operation)
    {
    }

    public void context(string name, Action operation)
    {
    }

    public void describe(string name, Action operation)
    {
    }

    public void xdescribe(string name, Action operation)
    {
    }

    public void xdescribe(string name, Action<object> operation)
    {
    }

    public void it(string name, Action operation)
    {
    }

    public void xit(string name, Action operation)
    {
    }

    public void describe(string name, Action<object> operation)
    {
    }
  }
}