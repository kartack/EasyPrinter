using UnityEngine;
using System.Collections;
using System.Text;

namespace EasyPrinter {
    internal static class SerializationMethods {

        public static string ConvertToSingleLineString(ConvertedObject toStringify) {
            return new StringBuilder().ConvertToSingleLineString(toStringify).ToString();
        }

        private static StringBuilder ConvertToSingleLineString(this StringBuilder toAddTo, ConvertedObject toAdd) {
            toAddTo.Append("{").Append(toAdd.name);

            if (toAdd.fields.Count > 0) {
                toAddTo.Append(": ");
                bool isFirst = true;
                foreach (var curKVP in toAdd.fields) {
                    if (isFirst) {
                        isFirst = false;
                    } else {
                        toAddTo.Append(", ");
                    }
                    toAddTo.Append(curKVP.Key).Append(" = ").AddFieldSingleLine(curKVP.Value);
                }
            }

            return toAddTo.Append("}");
        }

        private static StringBuilder ConvertToSingleLineString(this StringBuilder toAddTo, ConvertedMap toAdd) {
            toAddTo.Append("[").Append(toAdd.name).Append(" ").Append(toAdd.values.Count);

            if (toAdd.values.Count > 0) {
                toAddTo.Append(": ");
                bool isFirst = true;
                foreach (var curKVP in toAdd.values) {
                    if (isFirst) {
                        isFirst = false;
                    } else {
                        toAddTo.Append(", ");
                    }
                    toAddTo.AddFieldSingleLine(curKVP.Key).Append(" => ").AddFieldSingleLine(curKVP.Value);
                }
            }

            return toAddTo.Append("]");
        }

        private static StringBuilder ConvertToSingleLineString(this StringBuilder toAddTo, ConvertedList toAdd) {
            toAddTo.Append("[").Append(toAdd.name).Append(" ").Append(toAdd.values.Count);

            if (toAdd.values.Count > 0) {
                toAddTo.Append(": ");
                bool isFirst = true;
                foreach (var cur in toAdd.values) {
                    if (isFirst) {
                        isFirst = false;
                    } else {
                        toAddTo.Append(", ");
                    }
                    toAddTo.AddFieldSingleLine(cur);
                }
            }

            return toAddTo.Append("]");
        }

        private static StringBuilder AddFieldSingleLine(this StringBuilder toAddTo, object toAdd) {
            if (toAdd is string) {
                toAddTo.Append(toAdd);
            } else if (toAdd is ConvertedObject) {
                toAddTo.ConvertToSingleLineString((ConvertedObject)toAdd);
            } else if (toAdd is ConvertedMap) {
                toAddTo.ConvertToSingleLineString((ConvertedMap)toAdd);
            } else if (toAdd is ConvertedList) {
                toAddTo.ConvertToSingleLineString((ConvertedList)toAdd);
            } else {
                toAddTo.Append(toAdd.ToString());
            }
            return toAddTo;
        }
    }
}