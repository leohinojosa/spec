﻿// ReSharper disable InconsistentNaming
namespace spec.core.Model
{
  public enum ExecStatus
  {
    NotRun,
    Running,
    Completed
  }

  public enum SpecType
  {
    describe,
    context,
    it,
    beforeEach,
    afterEach,
    beforeAll,
    afterAll,
    xdescribe,
    xit,
  }
}