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

        private int _b = 0;
        public int b { get { return _b; } set { _b = value; } }

        private int _c = 0;
        public int c { get { return _c; } }

//the following warning is variable assigned but not used, we do this for testing purposes, I could write something to use the variable but seems cleaner to just do this - Dave
#pragma warning disable 414
        private int _d = 0;
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
}

