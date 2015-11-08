using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EasyPrinter.Test {
    internal class MainTestingBattery : TestingFramework {

        private const int NORMAL_TIME = 1;

        private static TestClass_NoMembers testClass_NoMembers = new TestClass_NoMembers();
        private static TestStruct_NoMembers testStruct_NoMembers = new TestStruct_NoMembers();
        private static TestClass_OneSimpleMember testClass_OneSimpleMember = new TestClass_OneSimpleMember() { i = 5 };
        private static TestClass_TwoSimpleMembers testClass_TwoSimpleMembers = new TestClass_TwoSimpleMembers() { i = 5, j = 12 };
        private static TestClass_StringReference testClass_StringReference = new TestClass_StringReference() { a = "hello" };
        private static TestClass_ObjectReference testClass_ObjectReference = new TestClass_ObjectReference() { a = testClass_OneSimpleMember };
        private static TestClass_Property testClass_Property = new TestClass_Property() { a = 1, b = 2, d = 4 };
        private static TestClass_SimpleDictionary testClass_EmptyDictionary = new TestClass_SimpleDictionary() { a = new Dictionary<string, int>() };
        private static TestClass_SimpleDictionary testClass_Dictionary = new TestClass_SimpleDictionary() { a = new Dictionary<string, int>() { { "a", 1 }, { "b", 2 }, { "c", 3 } } };
        private static TestClass_ComplexDictionary testClass_ComplexDictionary = new TestClass_ComplexDictionary { a = new Dictionary<TestClass_StringReference, TestClass_OneSimpleMember>() { { new TestClass_StringReference() { a = "a" }, new TestClass_OneSimpleMember() { i = 1 } }, { new TestClass_StringReference() { a = "b" }, new TestClass_OneSimpleMember() { i = 2 } }, { new TestClass_StringReference() { a = "c" }, new TestClass_OneSimpleMember() { i = 3 } } } };
        private static TestClass_Enumeration testClass_Enumeration = new TestClass_Enumeration() { a = new int[] { 0, 1, 2, 3, 4 } };
        private static TestClass_LocalExclusions testClass_LocalExclusions = new TestClass_LocalExclusions() { a = 1, b = 2, c = 3, d = 4 };

        private static TestClass_Cycle testClass_CycleA = new TestClass_Cycle() { name = "a", nextObject = null };
        private static TestClass_Cycle testClass_CycleB = new TestClass_Cycle() { name = "b", nextObject = testClass_CycleA };
        private static TestClass_Cycle testClass_CycleC = new TestClass_Cycle() { name = "c", nextObject = testClass_CycleB };

        private static TestClass_Cycle testClassMakingSureThingsDontCarry = new TestClass_Cycle { name = "a", nextObject = testClass_LocalExclusions };

        static MainTestingBattery() {
            testClass_CycleA.nextObject = testClass_CycleC;
        }

        private static List<ExpectedTestResult> expectedResults = new List<ExpectedTestResult>(){
            new ExpectedTestResult() {testName = "Null Test", toPerform = (a) => a.EasyPrint(), input = null, expectedOutput = "null",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "No Variable Class Test", toPerform = (a) => a.EasyPrint(), input = testClass_NoMembers, expectedOutput = "{TestClass_NoMembers}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "No Variable Struct Test", toPerform = (a) => a.EasyPrint(), input = testStruct_NoMembers, expectedOutput = "{TestStruct_NoMembers}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "One Variable Class Test", toPerform = (a) => a.EasyPrint(), input = testClass_OneSimpleMember, expectedOutput = "{TestClass_OneSimpleMember: i = 5}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Two Variable Class Test", toPerform = (a) => a.EasyPrint(), input = testClass_TwoSimpleMembers, expectedOutput = "{TestClass_TwoSimpleMembers: i = 5, j = 12}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "One Variable String Class Test", toPerform = (a) => a.EasyPrint(), input = testClass_StringReference, expectedOutput = "{TestClass_StringReference: a = \"hello\"}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "One Variable Object Class Test", toPerform = (a) => a.EasyPrint(), input = testClass_ObjectReference, expectedOutput = "{TestClass_ObjectReference: a = {TestClass_OneSimpleMember: i = 5}}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Property Object Class Test", toPerform = (a) => a.EasyPrint(), input = testClass_Property, expectedOutput = "{TestClass_Property: a = 1, b = 2, c = 0}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Empty Dictionary Object Class Test", toPerform = (a) => a.EasyPrint(), input = testClass_EmptyDictionary, expectedOutput = "{TestClass_SimpleDictionary: a = [Dictionary`2 0]}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Dictionary Object Class Test", toPerform = (a) => a.EasyPrint(), input = testClass_Dictionary, expectedOutput = "{TestClass_SimpleDictionary: a = [Dictionary`2 3: \"a\" => 1, \"b\" => 2, \"c\" => 3]}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Complex Dictionary Object Class Test", toPerform = (a) => a.EasyPrint(), input = testClass_ComplexDictionary, expectedOutput = "{TestClass_ComplexDictionary: a = [Dictionary`2 3: {TestClass_StringReference: a = \"a\"} => {TestClass_OneSimpleMember: i = 1}, {TestClass_StringReference: a = \"b\"} => {TestClass_OneSimpleMember: i = 2}, {TestClass_StringReference: a = \"c\"} => {TestClass_OneSimpleMember: i = 3}]}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Enumeration Object Class Test", toPerform = (a) => a.EasyPrint(), input = testClass_Enumeration, expectedOutput = "{TestClass_Enumeration: a = [Int32[] 5: 0, 1, 2, 3, 4]}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Cycle Test", toPerform = (a) => a.EasyPrint(), input = testClass_CycleA, expectedOutput = "{TestClass_Cycle: name = \"a\", nextObject = {TestClass_Cycle: name = \"c\", nextObject = {TestClass_Cycle: name = \"b\", nextObject = TestClass_Cycle see above}}}",  expectedMS = NORMAL_TIME },

            new ExpectedTestResult() {testName = "Null Test", toPerform = (a) => a.EasyPrintMultiline(), input = null, expectedOutput = "null",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "No Variable Class Test", toPerform = (a) => a.EasyPrintMultiline(), input = testClass_NoMembers, expectedOutput = "TestClass_NoMembers",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "No Variable Struct Test", toPerform = (a) => a.EasyPrintMultiline(), input = testStruct_NoMembers, expectedOutput = "TestStruct_NoMembers",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "One Variable Class Test", toPerform = (a) => a.EasyPrintMultiline(), input = testClass_OneSimpleMember, expectedOutput = "TestClass_OneSimpleMember:\n  i = 5",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Two Variable Class Test", toPerform = (a) => a.EasyPrintMultiline(), input = testClass_TwoSimpleMembers, expectedOutput = "TestClass_TwoSimpleMembers:\n  i = 5\n  j = 12",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "One Variable String Class Test", toPerform = (a) => a.EasyPrintMultiline(), input = testClass_StringReference, expectedOutput = "TestClass_StringReference:\n  a = \"hello\"",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "One Variable Object Class Test", toPerform = (a) => a.EasyPrintMultiline(), input = testClass_ObjectReference, expectedOutput = "TestClass_ObjectReference:\n  a = TestClass_OneSimpleMember:\n    i = 5",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Property Object Class Test", toPerform = (a) => a.EasyPrintMultiline(), input = testClass_Property, expectedOutput = "TestClass_Property:\n  a = 1\n  b = 2\n  c = 0",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Empty Dictionary Object Class Test", toPerform = (a) => a.EasyPrintMultiline(), input = testClass_EmptyDictionary, expectedOutput = "TestClass_SimpleDictionary:\n  a = Dictionary`2 0",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Dictionary Object Class Test", toPerform = (a) => a.EasyPrintMultiline(), input = testClass_Dictionary, expectedOutput = "TestClass_SimpleDictionary:\n  a = Dictionary`2 3:\n    \"a\" => 1\n    \"b\" => 2\n    \"c\" => 3",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Complex Dictionary Object Class Test", toPerform = (a) => a.EasyPrintMultiline(), input = testClass_ComplexDictionary, expectedOutput = "TestClass_ComplexDictionary:\n  a = Dictionary`2 3:\n    TestClass_StringReference:\n      a = \"a\" => TestClass_OneSimpleMember:\n      i = 1\n    TestClass_StringReference:\n      a = \"b\" => TestClass_OneSimpleMember:\n      i = 2\n    TestClass_StringReference:\n      a = \"c\" => TestClass_OneSimpleMember:\n      i = 3",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Enumeration Object Class Test", toPerform = (a) => a.EasyPrintMultiline(), input = testClass_Enumeration, expectedOutput = "TestClass_Enumeration:\n  a = Int32[] 5:\n    0\n    1\n    2\n    3\n    4",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Cycle Test", toPerform = (a) => a.EasyPrintMultiline(), input = testClass_CycleA, expectedOutput = "TestClass_Cycle:\n  name = \"a\"\n  nextObject = TestClass_Cycle:\n    name = \"c\"\n    nextObject = TestClass_Cycle:\n      name = \"b\"\n      nextObject = TestClass_Cycle see above",  expectedMS = NORMAL_TIME },

            new ExpectedTestResult() {testName = "Local Exclusion Object No Exclusions", toPerform = (a) => a.EasyPrint(), input = testClass_LocalExclusions, expectedOutput = "{TestClass_LocalExclusions: a = 1, b = 2, c = 3, d = 4}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Local Exclusion Object No Exclusions Print Only", toPerform = (a) => a.EasyPrintPrintOnly(), input = testClass_LocalExclusions, expectedOutput = "{TestClass_LocalExclusions}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Local Exclusion Object No Exclusions Dont Print", toPerform = (a) => a.EasyPrintDontPrint(), input = testClass_LocalExclusions, expectedOutput = "{TestClass_LocalExclusions: a = 1, b = 2, c = 3, d = 4}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Local Exclusion Object AC Print Only", toPerform = (a) => a.EasyPrintPrintOnly("a", "c"), input = testClass_LocalExclusions, expectedOutput = "{TestClass_LocalExclusions: a = 1, c = 3}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Local Exclusion Object AC Dont Print", toPerform = (a) => a.EasyPrintDontPrint("b", "d"), input = testClass_LocalExclusions, expectedOutput = "{TestClass_LocalExclusions: a = 1, c = 3}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Local Exclusion Object Make Sure Print Only Doesn't Carry on", toPerform = (a) => a.EasyPrintDontPrint("name", "d"), input = testClassMakingSureThingsDontCarry, expectedOutput = "{TestClass_Cycle: nextObject = {TestClass_LocalExclusions: a = 1, b = 2, c = 3, d = 4}}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Local Exclusion Object Make Sure Dont Print Doesn't Carry on", toPerform = (a) => a.EasyPrintPrintOnly("nextObject", "d"), input = testClassMakingSureThingsDontCarry, expectedOutput = "{TestClass_Cycle: nextObject = {TestClass_LocalExclusions: a = 1, b = 2, c = 3, d = 4}}",  expectedMS = NORMAL_TIME }
        };
        
        protected override List<ExpectedTestResult> getExpectedResults() {
            return expectedResults;
        }
    }
}