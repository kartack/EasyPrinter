using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace EasyPrinter {
	internal class ConvertedObject {
        public string name;
        public List<KeyValuePair<string, object>> fields;
		public object originalObject;
		public bool hasToStringMethod;

        internal static ConvertedObject StartNewConvertedObject(object toConvert) {
			return new ConvertedObject() {
                name = ObjectConverter.GetNameOfObject(toConvert),
                fields = new List<KeyValuePair<string, object>>(),
				originalObject = toConvert,
				hasToStringMethod = (toConvert.GetType().GetMethod("ToString").DeclaringType != typeof(System.Object))
            };
        }
    }

    internal class ConvertedList {
        public string name;
        public List<object> values;
    }

    internal class ConvertedMap {
        public string name;
        public List<KeyValuePair<object, object>> values;
    }

    internal class ObjectConverter {
		internal const string BACKING_FIELD_NAME = ">k__BackingField";

        private static HashSet<object> previouslyViewedObjects = null;

        internal static ConvertedObject ConvertObject(object toConvert, List<string> toPrint) {
            if (object.ReferenceEquals(toConvert, null)) {
                return null;
            }
            
            if (previouslyViewedObjects == null) {
                previouslyViewedObjects = new HashSet<object>();
            } else {
                previouslyViewedObjects.Clear();
            }

            return ProduceConvetedObject(toConvert, toPrint);
        }

        private static ConvertedObject ProduceConvetedObject(object toConvert, List<string> toPrint = null) {
            previouslyViewedObjects.Add(toConvert);

            if(toPrint == null) {
                toPrint = AttributeExtensions.GetListOfFieldsToPrint(toConvert);
            }

            ConvertedObject toRet = ConvertedObject.StartNewConvertedObject(toConvert);
            System.Type toConvertType = toConvert.GetType();

			foreach (var curField in toConvertType.GetFields(AttributeExtensions.BINDING_FLAGS_TO_USE_TO_RETRIEVE)) {
				//we automatically exclude all backing fields because we include the property that they back,
				//consider adding a class or field level attribute to override this behaviour, but likely its unecessary.
				if(curField.Name.Contains(BACKING_FIELD_NAME)){
					continue;
				}
				if(toPrint == null || toPrint.Contains(curField.Name)) {
                    toRet.fields.Add(new KeyValuePair<string, object>(curField.Name, ProduceFieldEntry(curField.GetValue(toConvert))));
                }
                
            }
            
			foreach (var curProperty in toConvertType.GetProperties(AttributeExtensions.BINDING_FLAGS_TO_USE_TO_RETRIEVE)) {
                if (curProperty.CanRead && (toPrint == null || toPrint.Contains(curProperty.Name))) {
                    toRet.fields.Add(new KeyValuePair<string, object>(curProperty.Name, ProduceFieldEntry(curProperty.GetValue(toConvert, null))));
                }
            }

            return toRet;
        }

        internal static string GetNameOfObject(object toGetNameOf) {
            //return toGetNameOf.GetType().Name+" "+toGetNameOf.GetHashCode().ToString();
            return toGetNameOf.GetType().Name;
        }

        private static object ProduceFieldEntry(object toProduceFor) {
            if (object.ReferenceEquals(toProduceFor, null)) {
                return null;
            }

            if (toProduceFor is string) {
                return "\"" + toProduceFor + "\"";
            }
            
            if (HasSeenObjectBefore(toProduceFor)) {
                return GetNameOfObject(toProduceFor) + " see above";
            }

            if (toProduceFor is IDictionary) {
                ConvertedMap toRet = new ConvertedMap() { name = GetNameOfObject(toProduceFor), values = new List<KeyValuePair<object, object>>() };
                foreach(var cur in  (toProduceFor as IDictionary).Keys) {
                    toRet.values.Add(new KeyValuePair<object, object>(ProduceFieldEntry(cur), ProduceFieldEntry((toProduceFor as IDictionary)[cur])));
                }
                return toRet;
            }

            if (toProduceFor is IEnumerable) {
                ConvertedList toRet = new ConvertedList() { name = GetNameOfObject(toProduceFor), values = new List<object>() };
                foreach (var cur in toProduceFor as IEnumerable) {
                    toRet.values.Add(ProduceFieldEntry(cur));
                }
                return toRet;
            }

            Type ourType = toProduceFor.GetType();

            if (ourType.IsPrimitive) {
                return toProduceFor.ToString();
            }

            return ProduceConvetedObject(toProduceFor);
        }

        private static bool HasSeenObjectBefore(object toCheck) {
            foreach (var cur in previouslyViewedObjects) {
                if (object.ReferenceEquals(cur, toCheck)) {
                    return true;
                }
            }
            return false;
        }
    }
}

