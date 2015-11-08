using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace EasyPrinter {
    
    [AttributeUsage(AttributeTargets.All)]
    public class PrintOnly : System.Attribute {
        internal List<string> ourParams = null;
        public PrintOnly(params string[] appliesTo) {
            this.ourParams = AttributeExtensions.ConvertToStringList(appliesTo);
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class DontPrint : System.Attribute {
        internal List<string> ourParams = null;
        public DontPrint(params string[] appliesTo) {
            this.ourParams = AttributeExtensions.ConvertToStringList(appliesTo);
        }
    }

    internal enum InputListType { NONE, PRINT_ONLY, DONT_PRINT}

    internal static class AttributeExtensions {
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
            foreach (var curField in toConvertType.GetFields()) {
                toRet.Add(curField);
            }

            foreach (var curProperty in toConvertType.GetProperties()) {
                toRet.Add(curProperty);
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
                    return null;//TODO: Add in processing actual tags on fields in the class
                default:
                    throw new ArgumentException("GetListOfFieldsToPrint given a non-null input list but not given a InputListType we know, listType = " + listType);                   
            }
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
