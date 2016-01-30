using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace EasyPrinter {

	public class EasyPrinterAttributeRoot : Attribute {
		internal bool inherits = true;

		public EasyPrinterAttributeRoot(bool inherits) {
			this.inherits = inherits;
		}
	}

	public class EasyPrinterFieldAttribute : EasyPrinterAttributeRoot {
        internal List<string> ourParams = null;
        
		public EasyPrinterFieldAttribute(bool inherits, params string[] appliesTo) : base(inherits) {
            this.ourParams = AttributeExtensions.ConvertToStringList(appliesTo);
        }
    }

	[AttributeUsage(AttributeExtensions.CLASS_AND_FIELD_ATTRIBUTE_TARGET, AllowMultiple = true)]
	public class PrintOnly : EasyPrinterFieldAttribute {
        public PrintOnly(params string[] appliesTo) : this(true, appliesTo) { }
        public PrintOnly(bool inherits, params string[] appliesTo) : base(inherits, appliesTo) {}
    }

	[AttributeUsage(AttributeExtensions.CLASS_AND_FIELD_ATTRIBUTE_TARGET, AllowMultiple = true)]
	public class DontPrint : EasyPrinterFieldAttribute {
        public DontPrint(params string[] appliesTo) : this(true, appliesTo){}
        public DontPrint(bool inherits, params string[] appliesTo) : base(inherits, appliesTo) {}
    }

	[AttributeUsage(AttributeExtensions.CLASS_ONLY_ATTRIBUTE_TARGET, AllowMultiple = true)]
	public class UseToString : EasyPrinterAttributeRoot {
		public UseToString(bool inherits = true) : base(inherits) {}
	}

	[AttributeUsage(AttributeExtensions.CLASS_ONLY_ATTRIBUTE_TARGET, AllowMultiple = true)]
	public class DontUseToString : EasyPrinterAttributeRoot {
		public DontUseToString(bool inherits = true) : base(inherits) {}
	}

	internal struct IEnumerablePrintingRange {
		internal int printFrom;
		internal int printTo;

		internal IEnumerablePrintingRange(int printFrom, int printTo){
			this.printFrom = printFrom;
			this.printTo = printTo;
		}
	}

	[AttributeUsage(AttributeExtensions.FIELD_ONLY_ATTRIBUTE_TARGET, AllowMultiple = false)]
	public class PrintOnlyRange : EasyPrinterAttributeRoot {
		private int? numToPrint = null;
		private int? startFrom = null;
		private int? endAt = null;

		internal PrintOnlyRange(int? numToPrint = null, int? startFrom = null, int? endAt = null, bool inherits = true) : base(inherits) {
			this.numToPrint = numToPrint;
			this.startFrom = startFrom;
			this.endAt = endAt;
		}
	
		internal IEnumerablePrintingRange GetPrintingRange(int length){

		}
	}


    internal enum InputListType { NONE, PRINT_ONLY, DONT_PRINT}

    internal static class AttributeExtensions {
		internal const AttributeTargets CLASS_ONLY_ATTRIBUTE_TARGET = AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Interface;
		internal const AttributeTargets FIELD_ONLY_ATTRIBUTE_TARGET = AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Property;
		internal const AttributeTargets CLASS_AND_FIELD_ATTRIBUTE_TARGET = CLASS_ONLY_ATTRIBUTE_TARGET | FIELD_ONLY_ATTRIBUTE_TARGET;

		internal const BindingFlags BINDING_FLAGS_TO_USE_TO_RETRIEVE = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
		private static char[] separatorCharacters = new char[] { ',', ';', '|', '/', '\\', '.' };

        internal static List<String> ConvertToStringList(string[] input) {
            List<string> toRet = new List<string>(input);
            foreach(var toSplitOn in separatorCharacters) {
                for(int i = 0; i<toRet.Count; i++) {
                    if (toRet[i].Contains(toSplitOn.ToString())) {
                        toRet.AddRange(toRet.RemoveAtAndReturn(i).Split(toSplitOn));
                    }
                }
            }
            return toRet;
        }

        internal static T RemoveAtAndReturn<T>(this List<T> a, int i) {
            T toRet = a[i];
            a.RemoveAt(i);
            return toRet;
        }
        
        internal static bool IsNullOrEmpty(this IEnumerable a) {
            if(object.ReferenceEquals(a, null)) {
                return true;
            }
            return !a.GetEnumerator().MoveNext();
        }

        private static List<MemberInfo> GetFieldsAndProperties(object obj) {
            List<MemberInfo> toRet = new List<MemberInfo>();
            System.Type toConvertType = obj.GetType();
			foreach (var curField in toConvertType.GetFields(BINDING_FLAGS_TO_USE_TO_RETRIEVE)) {
                toRet.Add(curField);
            }

			foreach (var curProperty in toConvertType.GetProperties(BINDING_FLAGS_TO_USE_TO_RETRIEVE)) {
                toRet.Add(curProperty);
            }
            return toRet;
        }

        private struct AttributeReport {
            public bool hasAttribute;
            public List<string> markedFields;
        }
        private static bool[] ALL_BOOL_VALUES = new bool[] { false, true };
		private static AttributeReport GetAttributeReport<T>(object obj) where T : EasyPrinterFieldAttribute {
			AttributeReport toRet = new AttributeReport(){hasAttribute = false, markedFields = new List<string>()};
            
            //check the root object for the tag
            foreach(var curInheritance in ALL_BOOL_VALUES) {
				foreach (T cur in obj.GetType().GetCustomAttributes(typeof(T), curInheritance)) {//check for both inheriting and non-inheriting tags
					if (cur.inherits == curInheritance || !curInheritance) {
		                toRet.hasAttribute = true;
                        toRet.markedFields.AddRange(cur.ourParams);
                    }
                }
            }
            
            //now check all its fields
            foreach(var curField in GetFieldsAndProperties(obj)) {
                foreach (var curInheritance in ALL_BOOL_VALUES) {
                    foreach (T cur in curField.GetCustomAttributes(typeof(T), curInheritance)) {//first check for inheriting tags
						if (cur.inherits == curInheritance || !curInheritance) {
                            toRet.hasAttribute = true;
                            toRet.markedFields.Add(curField.Name);
                        }
                    }
                }
            }

            return toRet;
        }

        private static List<string> GetNamesOfAllFieldsAndParameters(object obj) {
            List<string> toRet = new List<string>();
            foreach(var c in AttributeExtensions.GetFieldsAndProperties(obj)) {
                toRet.Add(c.Name);
            }
            return toRet;
        }

        private static List<string> RemoveAll(List<string> toRemoveFrom, List<string> toRemove) {
            List<string> toRet = new List<string>(toRemoveFrom);
            foreach (var c in toRemove) {
                toRet.Remove(c);
            }
            return toRet;
        }

        internal static List<string> GetListOfFieldsToPrint(object obj, string[] inputList = null, InputListType listType = InputListType.NONE) {
			switch (listType) {
                case InputListType.PRINT_ONLY:
                    if (!inputList.IsNullOrEmpty()) {
                        return AttributeExtensions.ConvertToStringList(inputList);
                    }else {
                        return new List<string>();
                    }
                case InputListType.DONT_PRINT:
                    if (!inputList.IsNullOrEmpty()) {
                        return AttributeExtensions.RemoveAll(AttributeExtensions.GetNamesOfAllFieldsAndParameters(obj), AttributeExtensions.ConvertToStringList(inputList));
                    } else {
                        return null;
                    }
			case InputListType.NONE:
				if (ReferenceEquals (obj, null)) {
					return null;
				}

				AttributeReport printOnlyFields = AttributeExtensions.GetAttributeReport<PrintOnly> (obj);
				AttributeReport dontPrintFields = AttributeExtensions.GetAttributeReport<DontPrint> (obj);

				if(printOnlyFields.hasAttribute && dontPrintFields.hasAttribute) {
                    throw new System.ArgumentException("We are trying to print an object of type: "+obj.GetType().FullName+". You are trying to print an object that has both DontPrint and PrintOnly fields, we can't do both. Check your class definition, all fields, all properties, to correct. You can also use EasyPrintPrintOnly or EasyPrintDontPrint to avoid this. Also be sure to check all classes and interfaces you inheirt from.");
                } else if(printOnlyFields.hasAttribute){
                    return printOnlyFields.markedFields;
                } else {
                    return AttributeExtensions.RemoveAll(AttributeExtensions.GetNamesOfAllFieldsAndParameters(obj), dontPrintFields.markedFields);
                }

            default:
                throw new ArgumentException("GetListOfFieldsToPrint given a non-null input list but not given a InputListType we know, listType = " + listType);                   
            }
        }

		internal static bool IsModifiedByClassAttribute<T>(object obj) where T : EasyPrinterAttributeRoot {
			return GetClassAttribute<T> (obj) != null;
		}

		internal static T GetClassAttribute<T>(object obj) where T : EasyPrinterAttributeRoot {
			Type objType = obj.GetType ();
			foreach (var curInheritance in ALL_BOOL_VALUES) {
				foreach (var cur in objType.GetCustomAttributes(curInheritance)) {
					if(cur is T && ((cur as EasyPrinterAttributeRoot).inherits == curInheritance || !curInheritance)){
						return cur as T;
					}
				}
			}
			return null;
		}

		internal static bool IsModifiedByFieldAttribute<T>(object obj, string field) where T : EasyPrinterAttributeRoot {
			return GetFieldAttribute<T> (obj, field) != null;
		}

		internal static T GetFieldAttribute<T>(object obj, string field) where T : EasyPrinterAttributeRoot {
			Type objType = obj.GetType ();

			MemberInfo[] fieldInfo = new MemberInfo[]{
				objType.GetField (field, BINDING_FLAGS_TO_USE_TO_RETRIEVE),
				objType.GetProperty(field, BINDING_FLAGS_TO_USE_TO_RETRIEVE)
			};

			foreach (var curMemberType in fieldInfo) {
				if (curMemberType == null) {
					continue;
				}
				foreach (var curInheritance in ALL_BOOL_VALUES) {
					foreach (var cur in curMemberType.GetCustomAttributes(curInheritance)) {
						if (cur is T && (cur as EasyPrinterFieldAttribute).ourParams.Contains (field)
							&& ((cur as EasyPrinterFieldAttribute).inherits == curInheritance || !curInheritance)) {
							return cur as T;
						}
					}
				}
			}

			return null;
		}

		internal static bool IsModifiedByClassOrFieldAttribute<T>(object obj, string field) where T : EasyPrinterFieldAttribute{
			return GetClassOrFieldAttribute<T> (obj, field) != null;
		}

		internal static T GetClassOrFieldAttribute<T>(object obj, string field) where T : EasyPrinterFieldAttribute {
			Type objType = obj.GetType ();
			foreach (var curInheritance in ALL_BOOL_VALUES) {
				foreach (var cur in objType.GetCustomAttributes(curInheritance)) {
					if(cur is T && (cur as EasyPrinterFieldAttribute).ourParams.Contains(field)
						&& ((cur as EasyPrinterFieldAttribute).inherits == curInheritance || !curInheritance)){
						return cur as T;
					}
				}
			}

			return GetFieldAttribute<T> (obj, field);
		}

        internal static string ToExplodedString(this IEnumerable a) {
            if(ReferenceEquals(a, null)) {
                return "null";
            }
                
            string toRet = "";
            foreach(var c in a) {
                toRet += c.ToString() + ", ";
            }
            if(toRet.Length > 2) {
                return toRet.Substring(0, toRet.Length - 2);
            } else {
                return toRet;
            }
            
        }
    }
}
