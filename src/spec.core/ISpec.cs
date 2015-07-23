using System;

namespace spec.core
{
  public interface ISpec
  {
    void afterAll(string description, Action operation);
    void afterAll(Action operation);
    void afterEach(Action operation);
    void afterEach(string description, Action operation);
    void beforeAll(string description, Action operation);
    void beforeAll(Action operation);
    void beforeEach(Action operation);
    void beforeEach(string description, Action operation);
    void context(string name, Action operation);
    void describe(string name, Action operation);
    void describe(string name, Action<object> operation);
    void it(string name, Action operation);
    void xdescribe(string name, Action operation);
    void xdescribe(string name, Action<object> operation);
    void xit(string name, Action operation);
  }
}