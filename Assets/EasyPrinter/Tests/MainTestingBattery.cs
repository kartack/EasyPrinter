using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EasyPrinter.Test {
    internal class MainTestingBattery : TestingFramework {

        private const int NORMAL_TIME = 100;

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

        private static TestClass_PrintOnlyClassAttribute testClassForPrintOnlyClassAttribute = new TestClass_PrintOnlyClassAttribute() { a = 1, b = 2, c = 3, d = 4 };
        private static TestClass_DontPrintClassAttribute testClassForDontPrintClassAttribute = new TestClass_DontPrintClassAttribute() { a = 1, b = 2, c = 3, d = 4 };
        private static TestClass_PrintOnlyFieldAttribute testClassForPrintOnlyFieldAttribute = new TestClass_PrintOnlyFieldAttribute() { a = 1, b = 2, c = 3, d = 4 };
        private static TestClass_DontPrintFieldAttribute testClassForDontPrintFieldAttribute = new TestClass_DontPrintFieldAttribute() { a = 1, b = 2, c = 3, d = 4 };

        private static TestClass_BothDontPrintAndPrintOnlyBothOnClassNotInherited testClassForExceptionFieldAttributeyBothOnClassNotInherited = new TestClass_BothDontPrintAndPrintOnlyBothOnClassNotInherited() { a = 0, b = 1 };
        private static TestClass_BothDontPrintAndPrintOnlyBothOnFieldsNotInherited testClassForExceptionFieldAttributeBothOnFieldsNotInherited = new TestClass_BothDontPrintAndPrintOnlyBothOnFieldsNotInherited() { a = 0, b = 1 };
        private static TestClass_BothDontPrintAndPrintOnlyMixedNotInheritedA testClassForExceptionMixedNotInheritedA = new TestClass_BothDontPrintAndPrintOnlyMixedNotInheritedA() { a = 0, b = 1 };
        private static TestClass_BothDontPrintAndPrintOnlyMixedNotInheritedB testClassForExceptionMixedNotInheritedB = new TestClass_BothDontPrintAndPrintOnlyMixedNotInheritedB() { a = 0, b = 1 };
        
		private static TestClass_ExceptionBothOnRoot testClass_ExceptionBothOnRoot = new TestClass_ExceptionBothOnRoot(){a = 0, b = 1, c = 2};
		private static TestClass_NoExceptionSameOnRoot testClass_NoExceptionSameOnRoot = new TestClass_NoExceptionSameOnRoot(){a = 0, b = 1, c = 2};
		private static TestClass_NoExceptionNonInehritedOnRoot testClass_NoExceptionNonInehritedOnRoot = new TestClass_NoExceptionNonInehritedOnRoot(){a = 0, b = 1, c = 2};
		private static TestClass_ExceptionOnOnRootOneOnMember testClass_ExceptionOnOnRootOneOnMember = new TestClass_ExceptionOnOnRootOneOnMember(){a = 0, b = 1, c = 2};
		private static TestClass_ExceptionSameOnRootAsOnField testClass_ExceptionSameOnRootAsOnField = new TestClass_ExceptionSameOnRootAsOnField(){a = 0, b = 1, c = 2};
		private static TestClass_NoExceptionNonInehritedOnRootAndOnField testClass_NoExceptionNonInehritedOnRootAndOnField = new TestClass_NoExceptionNonInehritedOnRootAndOnField(){a = 0, b = 1, c = 2};

		private static TestClass_PrivateVarTest testClass_PrivateVarTest = new TestClass_PrivateVarTest ();

		private static TestClass_ToStringTest testClass_ToStringTest = new TestClass_ToStringTest();

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
            new ExpectedTestResult() {testName = "Local Exclusion Object Make Sure Dont Print Doesn't Carry on", toPerform = (a) => a.EasyPrintPrintOnly("nextObject", "d"), input = testClassMakingSureThingsDontCarry, expectedOutput = "{TestClass_Cycle: nextObject = {TestClass_LocalExclusions: a = 1, b = 2, c = 3, d = 4}}",  expectedMS = NORMAL_TIME },

            new ExpectedTestResult() {testName = "Print Only Class Attribute", toPerform = (a) => a.EasyPrint(), input = testClassForPrintOnlyClassAttribute, expectedOutput = "{TestClass_PrintOnlyClassAttribute: a = 1, b = 2}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Don't Print Class Attribute", toPerform = (a) => a.EasyPrint(), input = testClassForDontPrintClassAttribute, expectedOutput = "{TestClass_DontPrintClassAttribute: c = 3, d = 4}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Print Only Field Attribute", toPerform = (a) => a.EasyPrint(), input = testClassForPrintOnlyFieldAttribute, expectedOutput = "{TestClass_PrintOnlyFieldAttribute: a = 1, b = 2}",  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Don't Print Field Attribute", toPerform = (a) => a.EasyPrint(), input = testClassForDontPrintFieldAttribute, expectedOutput = "{TestClass_DontPrintFieldAttribute: c = 3, d = 4}",  expectedMS = NORMAL_TIME },

            new ExpectedTestResult() {testName = "Exception Handling Test Both On Class No Inheritance", toPerform = (a) => a.EasyPrint(), input = testClassForExceptionFieldAttributeyBothOnClassNotInherited, expectedException = new System.ArgumentException("We are trying to print an object of type: EasyPrinter.Test.TestClass_BothDontPrintAndPrintOnlyBothOnClassNotInherited. You are trying to print an object that has both DontPrint and PrintOnly fields, we can't do both. Check your class definition, all fields, all properties, to correct. You can also use EasyPrintPrintOnly or EasyPrintDontPrint to avoid this. Also be sure to check all classes and interfaces you inheirt from."),  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Exception Handling Test Both On Field No Inheritance", toPerform = (a) => a.EasyPrint(), input = testClassForExceptionFieldAttributeBothOnFieldsNotInherited, expectedException = new System.ArgumentException("We are trying to print an object of type: EasyPrinter.Test.TestClass_BothDontPrintAndPrintOnlyBothOnFieldsNotInherited. You are trying to print an object that has both DontPrint and PrintOnly fields, we can't do both. Check your class definition, all fields, all properties, to correct. You can also use EasyPrintPrintOnly or EasyPrintDontPrint to avoid this. Also be sure to check all classes and interfaces you inheirt from."),  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Exception Handling Mixed Case A", toPerform = (a) => a.EasyPrint(), input = testClassForExceptionMixedNotInheritedA, expectedException = new System.ArgumentException("We are trying to print an object of type: EasyPrinter.Test.TestClass_BothDontPrintAndPrintOnlyMixedNotInheritedA. You are trying to print an object that has both DontPrint and PrintOnly fields, we can't do both. Check your class definition, all fields, all properties, to correct. You can also use EasyPrintPrintOnly or EasyPrintDontPrint to avoid this. Also be sure to check all classes and interfaces you inheirt from."),  expectedMS = NORMAL_TIME },
            new ExpectedTestResult() {testName = "Exception Handling Mixed Case B", toPerform = (a) => a.EasyPrint(), input = testClassForExceptionMixedNotInheritedB, expectedException = new System.ArgumentException("We are trying to print an object of type: EasyPrinter.Test.TestClass_BothDontPrintAndPrintOnlyMixedNotInheritedB. You are trying to print an object that has both DontPrint and PrintOnly fields, we can't do both. Check your class definition, all fields, all properties, to correct. You can also use EasyPrintPrintOnly or EasyPrintDontPrint to avoid this. Also be sure to check all classes and interfaces you inheirt from."),  expectedMS = NORMAL_TIME },
        	
			new ExpectedTestResult() {testName = "Both On Root Inherited Exception", toPerform = (a) => a.EasyPrint(), input = testClass_ExceptionBothOnRoot, expectedException = new System.ArgumentException("We are trying to print an object of type: EasyPrinter.Test.TestClass_ExceptionBothOnRoot. You are trying to print an object that has both DontPrint and PrintOnly fields, we can't do both. Check your class definition, all fields, all properties, to correct. You can also use EasyPrintPrintOnly or EasyPrintDontPrint to avoid this. Also be sure to check all classes and interfaces you inheirt from."),  expectedMS = NORMAL_TIME },
			new ExpectedTestResult() {testName = "Mixed Inheritance Exception", toPerform = (a) => a.EasyPrint(), input = testClass_ExceptionOnOnRootOneOnMember, expectedException = new System.ArgumentException("We are trying to print an object of type: EasyPrinter.Test.TestClass_ExceptionOnOnRootOneOnMember. You are trying to print an object that has both DontPrint and PrintOnly fields, we can't do both. Check your class definition, all fields, all properties, to correct. You can also use EasyPrintPrintOnly or EasyPrintDontPrint to avoid this. Also be sure to check all classes and interfaces you inheirt from."),  expectedMS = NORMAL_TIME },
			new ExpectedTestResult() {testName = "Both On Field Inherited Exception", toPerform = (a) => a.EasyPrint(), input = testClass_ExceptionSameOnRootAsOnField, expectedException = new System.ArgumentException("We are trying to print an object of type: EasyPrinter.Test.TestClass_ExceptionSameOnRootAsOnField. You are trying to print an object that has both DontPrint and PrintOnly fields, we can't do both. Check your class definition, all fields, all properties, to correct. You can also use EasyPrintPrintOnly or EasyPrintDontPrint to avoid this. Also be sure to check all classes and interfaces you inheirt from."),  expectedMS = NORMAL_TIME },

			new ExpectedTestResult() {testName = "Both On Root No Exception", toPerform = (a) => a.EasyPrint(), input = testClass_NoExceptionSameOnRoot, expectedOutput = "{TestClass_NoExceptionSameOnRoot: c = 2, a = 0}",  expectedMS = NORMAL_TIME },
			new ExpectedTestResult() {testName = "Mixed Inheritance No Exception", toPerform = (a) => a.EasyPrint(), input = testClass_NoExceptionNonInehritedOnRoot, expectedOutput = "{TestClass_NoExceptionNonInehritedOnRoot: c = 2}",  expectedMS = NORMAL_TIME },
			new ExpectedTestResult() {testName = "Both on Filed Inherited No Exception", toPerform = (a) => a.EasyPrint(), input = testClass_NoExceptionNonInehritedOnRootAndOnField, expectedOutput = "{TestClass_NoExceptionNonInehritedOnRootAndOnField: c = 2}",  expectedMS = NORMAL_TIME },

			new ExpectedTestResult() {testName = "Various Accessor Type Test", toPerform = (a) => a.EasyPrint(), input = testClass_PrivateVarTest, expectedOutput = "{TestClass_PrivateVarTest: a = 0, b = 1, c = 2, d = 3, e = 4, f = 5, g = 0, h = 1, i = 2, j = 3, k = 4, l = 5}",  expectedMS = NORMAL_TIME },

			new ExpectedTestResult() {testName = "ToString detection test", toPerform = (a) => a.EasyPrint(), input = testClass_ToStringTest, expectedOutput = "{TestClass_ToStringTest: a = 0, b = ObjectWithToString_ToStringReturnResult, c = ObjectWithToString_ToStringReturnResult, d = {ObjectWithoutToString}}",expectedMS = NORMAL_TIME}
		}; 
        
        protected override List<ExpectedTestResult> getExpectedResults() {
            return expectedResults;
        }
    }
}