﻿using System;
using System.Linq;
using FluentAssertions;
using spec.core;
using Xunit;

namespace spec.tests
{
    public class AgentTests
    {
        private spec_double _suite;

        public AgentTests()
        {
            _suite = new spec_double();
        }

        //[TestMethod] - Nested describes
        public void spec_shouldNotAllowAddingNestedSuites()
        {
            _suite = new spec_double();
            _suite.describe("new Describe",
                () => { _suite.it("spec", () => { _suite.describe("invalid", () => { }); }); });

            Agent agent = new Agent();
            agent.RunSuite(_suite);
            _suite.Registry.ExecutableLookupTable.Any(x => x.RanSuccessfully)
                .Should()
                .BeFalse("describes should not be in specs");
        }

        [Fact]
        public void agent_executesASpecAndRunsSuccesfully()
        {
            var i = 0;
            _suite.describe("new", () =>
            {
                i = 0;
                _suite.beforeAll(() => { i++; });

                _suite.beforeEach(() => { i++; });

                _suite.it("spec1", () => { i++; });

                _suite.it("spec2", () => { i++; });

                _suite.afterEach(() => { i++; });

                _suite.afterAll(() => { i++; });
            });

            var agent = new Agent();
            agent.RunSuite(_suite);
            i.Should().Be(8);
        }

        [Fact]
        public void agent_executesASpecHooksEvenAfterErrors()
        {
            int i = 0,
                j = 0;
            _suite.describe("new", () =>
            {
                i = 0;
                _suite.beforeAll(() => { i++; });

                _suite.beforeEach(() =>
                {
                    i++;
                    j++;
                });

                _suite.it("spec1", () => { throw new Exception("im breaking the flow"); });

                _suite.afterEach(() =>
                {
                    i++;
                    ;
                    j++;
                });

                _suite.afterAll(() => { i++; });
            });

            var agent = new Agent();
            agent.RunSuite(_suite);
            i.Should().Be(4);
            j.Should().Be(2);
        }

        [Fact]
        public void agent_executesASpecAnd()
        {
            var i = 0;
            _suite.describe("new", () =>
            {
                i = 0;
                _suite.it("spec1", () => { i++; });

                _suite.it("spec2");
            });

            var agent = new Agent();
            _suite.Registry.ExecutableLookupTable.Count.Should().Be(2);
            agent.RunSuite(_suite);
            i.Should().Be(1);
        }

        [Fact]
        public void agent_executesASpecAndRunsSuccesfullyWithHooks()
        {
            var i = 0;
            _suite.describe("new", () =>
            {
                _suite.beforeAll(() => { i++; });

                _suite.beforeEach(() => { i++; });

                _suite.beforeEach(() => { i++; });

                _suite.it("spec1", () =>
                {
                    //
                });

                _suite.afterEach(() => { i--; });

                _suite.afterEach(() => { i--; });

                _suite.afterAll(() => { i--; });
            });

            var agent = new Agent();
            agent.RunSuite(_suite);
            i.Should().Be(0);
        }

        [Fact]
        public void agent_executesASpecAndRunsSuccesfullyWithHooksSiblingDescribes()
        {
            var i = 0;
            _suite.describe("new", () =>
            {
                _suite.beforeAll(() => { i++; });

                _suite.beforeEach(() => { i++; });

                _suite.it("spec1", () =>
                {
                    //
                });

                _suite.afterEach(() => { i--; });

                _suite.afterAll(() => { i--; });
            });
            _suite.describe("new1", () =>
            {
                _suite.beforeAll(() => { i++; });

                _suite.beforeEach(() => { i++; });

                _suite.it("spec1", () =>
                {
                    //
                });

                _suite.afterEach(() => { i--; });

                _suite.afterAll(() => { i--; });
            });

            var agent = new Agent();
            agent.RunSuite(_suite);
            i.Should().Be(0);
        }

        [Fact]
        public void agent_executesASpecAndRunsSuccesfullyWithHooksNestedDescribes()
        {
            var i = 0;
            _suite.describe("new", () =>
            {
                _suite.describe("Nested1", () =>
                {
                    _suite.beforeAll(() => { i++; });

                    _suite.beforeEach(() => { i++; });

                    _suite.it("spec1", () =>
                    {
                        //
                    });

                    _suite.afterEach(() => { i--; });

                    _suite.afterAll(() => { i--; });
                });
                _suite.describe("Nested2", () =>
                {
                    _suite.beforeAll(() => { i++; });

                    _suite.beforeEach(() => { i++; });

                    _suite.it("spec1", () =>
                    {
                        //
                    });

                    _suite.afterEach(() => { i--; });

                    _suite.afterAll(() => { i--; });
                });
            });

            var agent = new Agent();
            agent.RunSuite(_suite);
            i.Should().Be(0);
        }

        [Fact] // Dynamic specs have some issues
        public void suite_shouldAddDYnamicSpecs()
        {
            var dynamicSpecCount = 10;
            _suite.describe("test describe", () =>
            {
                for (int i = 0; i < dynamicSpecCount; i++)
                {
                    _suite.it("the int " + i, () => { Console.Write(i); });
                }
            });

            _suite.Registry.ExecutableLookupTable.Count.Should().Be(dynamicSpecCount);
        }

        [Fact]
        public void agent_executes()
        {
            var i = new AllHookDouble();
            var agent = new Agent();
            agent.RunSuite(i);
            //agent.RunSuite(i.Registry.CurrentSuite);
        }
    }

    public class EachHookDouble : Spec
    {
        public EachHookDouble()
        {
            describe("Init", () =>
            {
                var i = 0;
                beforeEach("mainforeach 0", () => { i = 0; });
                beforeEach("mainforeach 1", () => { i = 1; });
                it("1", () =>
                {
                    i = i + 1;
                    Console.WriteLine(i);
                    i.Should().Be(2);
                });
                it("2", () =>
                {
                    i = i + 1;
                    i.Should().Be(2);
                });
                describe("xx", () =>
                {
                    var j = 0;
                    beforeEach("nested foreach", () => { j = 1; });
                    it("3", () =>
                    {
                        i.Should().Be(1);
                        j = j + 1;
                        j.Should().Be(2);
                    });
                    afterEach("nested aftereach", () => { });
                });
                afterEach("main aftereach", () => { });
            });
        }
    }

    public class AllHookDouble : Spec
    {
        public AllHookDouble()
        {
            describe("level 1", () =>
            {
                beforeAll("Main BeforeAll", () => { Console.WriteLine("L1 - before all"); });
                beforeEach("Main Beforeach", () => { Console.WriteLine("  L1 - beforeEach"); });
                afterEach(() => { Console.WriteLine("  L1 - afterEach"); });
                afterAll("Main AfterALl", () => { Console.WriteLine("L1 - after all"); });

                it("L1 test A", () => { Console.WriteLine("    L1 - test A"); });
                it("L1 test B", () => { Console.WriteLine("    L1 - Test B"); });

                describe("level 2", () =>
                {
                    beforeAll("nested before all", () => { Console.WriteLine("    L2 - before all"); });
                    beforeEach("nested beforeach", () => { Console.WriteLine("      L2 - beforeEach"); });
                    afterEach("nested after each", () => { Console.WriteLine("      L2 - afterEach"); });
                    afterAll("nested after all", () => { Console.WriteLine("    L2 - after all"); });

                    it("L2 test A", () => { Console.WriteLine("        L2 - test A"); });
                    it("L2 test B", () => { Console.WriteLine("        L2 - test B"); });
                });
            });
        }
    }
}