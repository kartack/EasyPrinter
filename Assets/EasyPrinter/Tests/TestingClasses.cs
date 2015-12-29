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

	internal class PrivateVarTest {
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
}