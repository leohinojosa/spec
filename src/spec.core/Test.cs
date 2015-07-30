using System;

namespace spec.core
{
  public abstract class Test : Spec
  {
    public void suite(string name, Action action)
    {
      describe(name, action);
    }

    public void test(string name, Action action)
    {
      it(name, action);
    }

    public void suiteSetup(Action operation)
    {
      beforeAll(operation);
    }

    public void suiteTearDown(Action operation)
    {
      afterAll(operation);
    }

    public void setup(Action operation)
    {
      beforeEach(operation);
    }

    public void tearDown(Action operation)
    {
      afterEach(operation);
    }
  }
}