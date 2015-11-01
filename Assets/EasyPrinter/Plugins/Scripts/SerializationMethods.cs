using UnityEngine;
using System.Collections;
using System.Text;

namespace EasyPrinter {
    internal struct PrintingConfiguration {
        public readonly string objectStart;
        public readonly string objectNameValuesSeparator;
        public readonly string objectEnd;
        public readonly string nameValueSeparator;
        public readonly string valueSeparator;
        public readonly string depthIndenter;
        public readonly string listOpener;
        public readonly string listTypeLengthSeparator;
        public readonly string listTypeValuesSparator;
        public readonly string listEntrySeparator;
        public readonly string listCloser;
        public readonly string mapSeparator;
        public readonly string stringOpener;
        public readonly string stringCloser;

        public PrintingConfiguration(string objectStart, string objectNameValuesSeparator, string objectEnd, string nameValueSeparator, string valueSeparator,
            string depthIndenter, string listOpener, string listTypeLengthSeparator, string listTypeValuesSparator, string listCloser,
            string listEntrySeparator, string mapSeparator, string stringOpener, string stringCloser) {
            this.objectStart = objectStart;
            this.objectNameValuesSeparator = objectNameValuesSeparator;
            this.objectEnd = objectEnd;
            this.nameValueSeparator = nameValueSeparator;
            this.valueSeparator = valueSeparator;
            this.depthIndenter = depthIndenter;
            this.listOpener = listOpener;
            this.listTypeLengthSeparator = listTypeLengthSeparator;
            this.listTypeValuesSparator = listTypeValuesSparator;
            this.listEntrySeparator = listEntrySeparator;
            this.listCloser = listCloser;
            this.mapSeparator = mapSeparator;
            this.stringOpener = stringOpener;
            this.stringCloser = stringCloser;
        }
    }

    internal static class SerializationMethods {

        private static PrintingConfiguration oneLinePrintingConfig = new PrintingConfiguration(objectStart: "{", objectNameValuesSeparator: ": ", objectEnd: "}", nameValueSeparator: " = ", valueSeparator: ", ",
            depthIndenter: "", listOpener: "[", listTypeLengthSeparator: " ", listTypeValuesSparator: ": ", listEntrySeparator: ", ", listCloser: "]", mapSeparator: " => ", stringOpener: "\"", stringCloser: "\"");

        public static string ConvertToSingleLineString(ConvertedObject toStringify) {
            return new StringBuilder().ConvertToSingleLineString(toStringify, oneLinePrintingConfig, 0).ToString();
        }

        private static StringBuilder ConvertToSingleLineString(this StringBuilder toAddTo, ConvertedObject toAdd, PrintingConfiguration printingConfiguration, int depth) {
            toAddTo.Append(printingConfiguration.objectStart).Append(toAdd.name);

            if (toAdd.fields.Count > 0) {
                toAddTo.Append(printingConfiguration.objectNameValuesSeparator);
                bool isFirst = true;
                foreach (var curKVP in toAdd.fields) {
                    if (isFirst) {
                        isFirst = false;
                    } else {
                        toAddTo.Append(printingConfiguration.valueSeparator);
                    }
                    toAddTo.Append(curKVP.Key).Append(printingConfiguration.nameValueSeparator).AddFieldSingleLine(curKVP.Value, printingConfiguration, depth);
                }
            }

            return toAddTo.Append(printingConfiguration.objectEnd);
        }

        private static StringBuilder ConvertToSingleLineString(this StringBuilder toAddTo, ConvertedMap toAdd, PrintingConfiguration printingConfiguration, int depth) {
            toAddTo.Append(printingConfiguration.listOpener).Append(toAdd.name).Append(printingConfiguration.listTypeLengthSeparator).Append(toAdd.values.Count);

            if (toAdd.values.Count > 0) {
                toAddTo.Append(printingConfiguration.listTypeValuesSparator);
                bool isFirst = true;
                foreach (var curKVP in toAdd.values) {
                    if (isFirst) {
                        isFirst = false;
                    } else {
                        toAddTo.Append(printingConfiguration.listEntrySeparator);
                    }
                    toAddTo.AddFieldSingleLine(curKVP.Key, printingConfiguration, depth).Append(printingConfiguration.mapSeparator).AddFieldSingleLine(curKVP.Value, printingConfiguration, depth);
                }
            }

            return toAddTo.Append(printingConfiguration.listCloser);
        }

        private static StringBuilder ConvertToSingleLineString(this StringBuilder toAddTo, ConvertedList toAdd, PrintingConfiguration printingConfiguration, int depth) {
            toAddTo.Append(printingConfiguration.listOpener).Append(toAdd.name).Append(printingConfiguration.listTypeLengthSeparator).Append(toAdd.values.Count);

            if (toAdd.values.Count > 0) {
                toAddTo.Append(printingConfiguration.listTypeValuesSparator);
                bool isFirst = true;
                foreach (var cur in toAdd.values) {
                    if (isFirst) {
                        isFirst = false;
                    } else {
                        toAddTo.Append(printingConfiguration.listEntrySeparator);
                    }
                    toAddTo.AddFieldSingleLine(cur, printingConfiguration, depth);
                }
            }

            return toAddTo.Append(printingConfiguration.listCloser);
        }

        private static StringBuilder AddFieldSingleLine(this StringBuilder toAddTo, object toAdd, PrintingConfiguration printingConfiguration, int depth) {
            if (toAdd is string) {
                toAddTo.Append(toAdd);
            } else if (toAdd is ConvertedObject) {
                toAddTo.ConvertToSingleLineString((ConvertedObject)toAdd, printingConfiguration, depth);
            } else if (toAdd is ConvertedMap) {
                toAddTo.ConvertToSingleLineString((ConvertedMap)toAdd, printingConfiguration, depth);
            } else if (toAdd is ConvertedList) {
                toAddTo.ConvertToSingleLineString((ConvertedList)toAdd, printingConfiguration, depth);
            } else {
                toAddTo.Append(toAdd.ToString());
            }
            return toAddTo;
        }
    }
}