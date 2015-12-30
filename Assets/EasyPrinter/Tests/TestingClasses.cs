using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EasyPrinter.Test {
    internal class TestClass_NoMembers {
    }

    internal struct TestStruct_NoMembers {
    }

    internal class TestClass_OneSimpleMember {
        public int i;
    }

    internal class TestClass_TwoSimpleMembers {
        public int i;
        public int j;
    }

    internal class TestClass_StringReference {
        public string a;
    }

    internal class TestClass_ObjectReference {
        public TestClass_OneSimpleMember a;
    }

    internal class TestClass_Property {
        public int a { get; set; }

        [DontPrint]private int _b = 0;
        public int b { get { return _b; } set { _b = value; } }

		[DontPrint]private int _c = 0;
        public int c { get { return _c; } }

//the following warning is variable assigned but not used, we do this for testing purposes, I could write something to use the variable but seems cleaner to just do this - Dave
#pragma warning disable 414
		[DontPrint]private int _d = 0;
        public int d { set { _d = value; } }
    }

    internal class TestClass_SimpleDictionary {
        public Dictionary<string, int> a;
    }

    internal class TestClass_ComplexDictionary {
        public Dictionary<TestClass_StringReference, TestClass_OneSimpleMember> a;
    }

    internal class TestClass_Enumeration {
        public int[] a;
    }

    internal class TestClass_Cycle {
        public string name;
        public object nextObject;
    }

    internal class TestClass_LocalExclusions {
        public int a;
        public int b;
        public int c { get; set; }
        public int d { get; set; }
    }

    [PrintOnly("a,b")]
    internal class TestClass_PrintOnlyClassAttribute {
        public int a;
        public int b;
        public int c;
        public int d;
    }

    [DontPrint("a,b")]
    internal class TestClass_DontPrintClassAttribute {
        public int a;
        public int b;
        public int c;
        public int d;
    }

    internal class TestClass_PrintOnlyFieldAttribute {
        [PrintOnly]
        public int a;
        [PrintOnly]
        public int b;
        public int c;
        public int d;
    }

    internal class TestClass_DontPrintFieldAttribute {
        [DontPrint]
        public int a;
        [DontPrint]
        public int b;
        public int c;
        public int d;
    }

    [DontPrint]
    [PrintOnly]
    internal class TestClass_BothDontPrintAndPrintOnlyBothOnClassNotInherited {
        public int a;
        public int b;
    }
    
    internal class TestClass_BothDontPrintAndPrintOnlyBothOnFieldsNotInherited {
        [DontPrint]
        public int a;
        [PrintOnly]
        public int b;
    }

    [DontPrint]
    internal class TestClass_BothDontPrintAndPrintOnlyMixedNotInheritedA {
        public int a;
        [PrintOnly]
        public int b;
    }

    [PrintOnly]
    internal class TestClass_BothDontPrintAndPrintOnlyMixedNotInheritedB {
        public int a;
        [DontPrint]
        public int b;
    }

    [PrintOnly(true, "a")]
    internal class TestClass_PrintOnlyInheritedClass{
        public int a;
        public int b;
    }
    
    [DontPrint(false, "b")]
    internal class TestClass_DontPrintNotInheritedClass {
        public int a;
        public int b;
    }
    
    internal class TestClass_PrintOnlyNotInheritedField {
        [PrintOnly(false)]
        public int a = 0;
        public int b = 1;
    }

    internal class TestClass_DontPrintInheritedField {
        public int a;
        [DontPrint(true)]
        public int b;
    }
    
    [DontPrint]
    internal class TestClass_ExceptionBothOnRoot : TestClass_PrintOnlyInheritedClass {
        public int c;
    }

    [PrintOnly("c")]
    internal class TestClass_NoExceptionSameOnRoot : TestClass_PrintOnlyInheritedClass {
        public int c;
    }

    [PrintOnly("c")]
    internal class TestClass_NoExceptionNonInehritedOnRoot : TestClass_DontPrintNotInheritedClass {
        public int c;
    }
    
    internal class TestClass_ExceptionOnOnRootOneOnMember : TestClass_PrintOnlyInheritedClass {
        [DontPrint]
        public int c;
    }
    
	internal class TestClass_ExceptionSameOnRootAsOnField : TestClass_DontPrintInheritedField {
        [PrintOnly]
        public int c;
    }

    internal class TestClass_NoExceptionNonInehritedOnRootAndOnField : TestClass_DontPrintNotInheritedClass {
        [PrintOnly]
        public int c;
    }

	internal class TestClass_PrivateVarTest {
		public int a = 0;
		protected int b = 1;
		internal int c = 2;
		private int d = 3;
		protected internal int e = 4;
		int f = 5;

		public static int g = 0;
		protected static int h = 1;
		internal static int i = 2;
		private static int j = 3;
		protected internal static int k = 4;
		static int l = 5;
	}

	internal class ObjectWithToString {
		public override string ToString (){return "ObjectWithToString_ToStringReturnResult";}
	}

	internal class ObjectInheritingFromAnObjectWithToString : ObjectWithToString {}

	internal class ObjectWithoutToString {}

	internal class TestClass_ToStringTest{
		public int a = 0;
		public ObjectWithToString b = new ObjectWithToString();
		public ObjectInheritingFromAnObjectWithToString c = new ObjectInheritingFromAnObjectWithToString();
		public ObjectWithoutToString d = new ObjectWithoutToString();
	}

	[UseToString]internal class ClassWithInheritingUseToString : ObjectWithToString {}
	[UseToString(false)]internal class ClassWithNoninheritingUseToString : ObjectWithToString {}
	[DontUseToString]internal class ClassWithInheritingDontUseToString : ObjectWithToString {}
	[DontUseToString(false)]internal class ClassWithNoninheritingDontUseToString : ObjectWithToString {}

	internal class ClassInhertingFrom_ClassWithInheritingUseToString : ClassWithInheritingUseToString {}
	internal class ClassInhertingFrom_ClassWithNoninheritingUseToString : ClassWithNoninheritingUseToString {}
	internal class ClassInhertingFrom_ClassWithInheritingDontUseToString : ClassWithInheritingDontUseToString {}
	internal class ClassInhertingFrom_ClassWithNoninheritingDontUseToString : ClassWithNoninheritingDontUseToString {}

	[DontUseToString]internal class ExceptionBecauseOfBothUseToStringAndDontUseToString : ClassWithInheritingUseToString{}
	[DontUseToString]internal class NoExceptionBecauseOfBothUseToStringAndDontUseToString : ObjectWithToString{}
	[DontUseToString]internal class NoExceptionBecauseTwoCopiesOfDontUseToString : ClassInhertingFrom_ClassWithInheritingDontUseToString{}

	internal class TestClass_TestingPrintingUseToStringAndDontUseToString {
		public ClassWithInheritingUseToString a = new ClassWithInheritingUseToString();
		public ClassWithNoninheritingUseToString b = new ClassWithNoninheritingUseToString();
		public ClassWithInheritingDontUseToString c = new ClassWithInheritingDontUseToString();
		public ClassWithNoninheritingDontUseToString d = new ClassWithNoninheritingDontUseToString();

		public ClassInhertingFrom_ClassWithInheritingUseToString e = new ClassInhertingFrom_ClassWithInheritingUseToString();
		public ClassInhertingFrom_ClassWithNoninheritingUseToString f = new ClassInhertingFrom_ClassWithNoninheritingUseToString();
		public ClassInhertingFrom_ClassWithInheritingDontUseToString g = new ClassInhertingFrom_ClassWithInheritingDontUseToString();
		public ClassInhertingFrom_ClassWithNoninheritingDontUseToString h = new ClassInhertingFrom_ClassWithNoninheritingDontUseToString();
	}

	[DontUseToString]internal class TestClass_ExceptionBecauseOfBothUseToStringAndDontUseToString {public ExceptionBecauseOfBothUseToStringAndDontUseToString a = new ExceptionBecauseOfBothUseToStringAndDontUseToString();}
	[DontUseToString]internal class TestClass_NoExceptionBecauseOfBothUseToStringAndDontUseToString : ObjectWithToString{public NoExceptionBecauseOfBothUseToStringAndDontUseToString a = new NoExceptionBecauseOfBothUseToStringAndDontUseToString();}
	[DontUseToString]internal class TestClass_NoExceptionBecauseTwoCopiesOfDontUseToString : ClassInhertingFrom_ClassWithInheritingDontUseToString{public NoExceptionBecauseTwoCopiesOfDontUseToString a = new NoExceptionBecauseTwoCopiesOfDontUseToString();}

	[UseToString]internal class TestClass_ClassForPrintingWithUseToStringOnRoot {
		public override string ToString (){return "This shouldn't be visible in the tests!";}
	}

	internal class TestClass_CheckBackerHidingSupport {
		public int a { get; set;}
		public float b { get; set;}
		public string c { get; set; }
		public TestClass_OneSimpleMember d {get; set;}
	}
}