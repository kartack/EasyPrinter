using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EasyPrinter {
    internal struct PrintingConfiguration {
        public readonly string objectStart;
        public readonly string objectStartIndenter;
        public readonly string objectNameValuesSeparator;
        public readonly string objectNameValuesSeparatorIndenter;
        public readonly string objectEnd;
        public readonly string objectEndIndenter;
        public readonly string nameValueSeparator;
        public readonly string nameValueSeparatorIndenter;
        public readonly string valueSeparator;
        public readonly string valueSeparatorIndenter;
        public readonly string listOpener;
        public readonly string listOpenerIndenter;
        public readonly string listTypeLengthSeparator;
        public readonly string listTypeLengthSeparatorIndenter;
        public readonly string listTypeValuesSparator;
        public readonly string listTypeValuesSparatorIndenter;
        public readonly string listEntrySeparator;
        public readonly string listEntrySeparatorIndenter;
        public readonly string listCloser;
        public readonly string listCloserIndenter;
        public readonly string mapSeparator;
        public readonly string mapSeparatorIndenter;
        public readonly string stringOpener;
        public readonly string stringOpenerIndenter;
        public readonly string stringCloser;
        public readonly string stringCloserIndenter;

        public PrintingConfiguration(string objectStart, string objectNameValuesSeparator, string objectEnd, string nameValueSeparator, string valueSeparator,
            string listOpener, string listTypeLengthSeparator, string listTypeValuesSparator, string listCloser, string listEntrySeparator, string mapSeparator,
            string stringOpener, string stringCloser, string objectStartIndenter = "", string objectNameValuesSeparatorIndenter = "", string objectEndIndenter = "",
            string nameValueSeparatorIndenter = "", string valueSeparatorIndenter = "", string listOpenerIndenter = "", string listTypeLengthSeparatorIndenter = "",
            string listTypeValuesSparatorIndenter = "", string listEntrySeparatorIndenter = "", string listCloserIndenter = "", string mapSeparatorIndenter = "",
            string stringOpenerIndenter = "", string stringCloserIndenter = "") {

            this.objectStart = objectStart;
            this.objectNameValuesSeparator = objectNameValuesSeparator;
            this.objectEnd = objectEnd;
            this.nameValueSeparator = nameValueSeparator;
            this.valueSeparator = valueSeparator;
            this.listOpener = listOpener;
            this.listTypeLengthSeparator = listTypeLengthSeparator;
            this.listTypeValuesSparator = listTypeValuesSparator;
            this.listEntrySeparator = listEntrySeparator;
            this.listCloser = listCloser;
            this.mapSeparator = mapSeparator;
            this.stringOpener = stringOpener;
            this.stringCloser = stringCloser;
            
            this.objectStartIndenter = objectStartIndenter;
            this.objectNameValuesSeparatorIndenter = objectNameValuesSeparatorIndenter;
            this.objectEndIndenter = objectEndIndenter;
            this.nameValueSeparatorIndenter = nameValueSeparatorIndenter;
            this.valueSeparatorIndenter = valueSeparatorIndenter;
            this.listOpenerIndenter = listOpenerIndenter;
            this.listTypeLengthSeparatorIndenter = listTypeLengthSeparatorIndenter;
            this.listTypeValuesSparatorIndenter = listTypeValuesSparatorIndenter;
            this.listEntrySeparatorIndenter = listEntrySeparatorIndenter;
            this.listCloserIndenter = listCloserIndenter;
            this.mapSeparatorIndenter = mapSeparatorIndenter;
            this.stringOpenerIndenter = stringOpenerIndenter;
            this.stringCloserIndenter = stringCloserIndenter;
        }
    }

    internal static class SerializationMethods {

        private static PrintingConfiguration oneLinePrintingConfig = new PrintingConfiguration(
            objectStart: "{",
            objectNameValuesSeparator: ": ",
            objectEnd: "}",
            nameValueSeparator: " = ",
            valueSeparator: ", ",
            listOpener: "[",
            listTypeLengthSeparator: " ",
            listTypeValuesSparator: ": ",
            listEntrySeparator: ", ",
            listCloser: "]",
            mapSeparator: " => ",
            stringOpener: "\"",
            stringCloser: "\""
        );

        private static PrintingConfiguration multiLinePrintingConfig = new PrintingConfiguration(
            objectStart: "",
            objectNameValuesSeparator: ":\n",
            objectEnd: "",
            nameValueSeparator: " = ",
            valueSeparator: "\n",
            listOpener: "",
            listTypeLengthSeparator: " ",
            listTypeValuesSparator: ":\n",
            listEntrySeparator: "\n",
            listCloser: "",
            mapSeparator: " => ",
            stringOpener: "\"",
            stringCloser: "\"",
            objectNameValuesSeparatorIndenter: "  ",
            valueSeparatorIndenter: "  ",
            listTypeValuesSparatorIndenter: "  ",
            listEntrySeparatorIndenter: "  "
        );

        public static string ConvertToSingleLineString(ConvertedObject toStringify) {
            return new StringBuilder().ConvertToString(toStringify, oneLinePrintingConfig, 0).ToString();
        }

        public static string ConvertToMultilineLineString(ConvertedObject toStringify) {
            return new StringBuilder().ConvertToString(toStringify, multiLinePrintingConfig, 0).ToString();
        }

        private static StringBuilder AppendMultiple(this StringBuilder toAddTo, string toAdd, int numTimes) {
            if(numTimes < 0) {
                throw new System.ArgumentException("SerializationMethods.AppendMultiple called with numTimes < 0, not sure how we can add something a negative number of times.");
            }
            
            for (int i = 0; i < numTimes; i++) {
                toAddTo.Append(toAdd);
            }
            return toAddTo;
        }

        private static StringBuilder AppendMultipleWithHeader(this StringBuilder toAddTo, string header, string multipleToAdd, int numTimes) {
            return toAddTo.Append(header).AppendMultiple(multipleToAdd, numTimes);
        }

        private static StringBuilder ConvertToString(this StringBuilder toAddTo, ConvertedObject toAdd, PrintingConfiguration printingConfiguration, int depth) {
            if(object.ReferenceEquals(toAdd, null)) {
                return toAddTo.Append("null");
            }

            toAddTo.Append(printingConfiguration.objectStart).Append(toAdd.name);

            if (toAdd.fields.Count > 0) {
                toAddTo.AppendMultipleWithHeader(printingConfiguration.objectNameValuesSeparator, printingConfiguration.objectNameValuesSeparatorIndenter, depth + 1);
                bool isFirst = true;
                foreach (var curKVP in toAdd.fields) {
                    if (isFirst) {
                        isFirst = false;
                    } else {
                        toAddTo.AppendMultipleWithHeader(printingConfiguration.valueSeparator, printingConfiguration.valueSeparatorIndenter, depth + 1);
                    }
                    toAddTo.Append(curKVP.Key).AppendMultipleWithHeader(printingConfiguration.nameValueSeparator, printingConfiguration.nameValueSeparatorIndenter, depth + 1)
                        .AddField(curKVP.Value, printingConfiguration, depth+1);
                }
            }

            return toAddTo.AppendMultipleWithHeader(printingConfiguration.objectEnd, printingConfiguration.objectEndIndenter, depth + 1);
        }

        private static StringBuilder ConvertToString(this StringBuilder toAddTo, ConvertedMap toAdd, PrintingConfiguration printingConfiguration, int depth) {
            toAddTo.AppendMultipleWithHeader(printingConfiguration.listOpener, printingConfiguration.listOpenerIndenter, depth).Append(toAdd.name)
                .AppendMultipleWithHeader(printingConfiguration.listTypeLengthSeparator, printingConfiguration.listTypeLengthSeparatorIndenter, depth).Append(toAdd.values.Count);

            if (toAdd.values.Count > 0) {
                toAddTo.AppendMultipleWithHeader(printingConfiguration.listTypeValuesSparator, printingConfiguration.listTypeValuesSparatorIndenter, depth);
                bool isFirst = true;
                foreach (var curKVP in toAdd.values) {
                    if (isFirst) {
                        isFirst = false;
                    } else {
                        toAddTo.AppendMultipleWithHeader(printingConfiguration.listEntrySeparator, printingConfiguration.listEntrySeparatorIndenter, depth);
                    }
                    toAddTo.AddField(curKVP.Key, printingConfiguration, depth).AppendMultipleWithHeader(printingConfiguration.mapSeparator, printingConfiguration.mapSeparatorIndenter, depth)
                        .AddField(curKVP.Value, printingConfiguration, depth);
                }
            }

            return toAddTo.AppendMultipleWithHeader(printingConfiguration.listCloser, printingConfiguration.listCloserIndenter, depth);
        }

        private static StringBuilder ConvertToString(this StringBuilder toAddTo, ConvertedList toAdd, PrintingConfiguration printingConfiguration, int depth) {
            toAddTo.AppendMultipleWithHeader(printingConfiguration.listOpener, printingConfiguration.listOpenerIndenter, depth).Append(toAdd.name)
                .AppendMultipleWithHeader(printingConfiguration.listTypeLengthSeparator, printingConfiguration.listTypeLengthSeparatorIndenter, depth).Append(toAdd.values.Count);

            if (toAdd.values.Count > 0) {
                toAddTo.AppendMultipleWithHeader(printingConfiguration.listTypeValuesSparator, printingConfiguration.listTypeValuesSparatorIndenter, depth);
                bool isFirst = true;
                foreach (var cur in toAdd.values) {
                    if (isFirst) {
                        isFirst = false;
                    } else {
                        toAddTo.AppendMultipleWithHeader(printingConfiguration.listEntrySeparator, printingConfiguration.listEntrySeparatorIndenter, depth);
                    }
                    toAddTo.AddField(cur, printingConfiguration, depth);
                }
            }

            return toAddTo.AppendMultipleWithHeader(printingConfiguration.listCloser, printingConfiguration.listCloserIndenter, depth);
        }

        private static StringBuilder AddField(this StringBuilder toAddTo, object toAdd, PrintingConfiguration printingConfiguration, int depth) {
            if (toAdd is string) {
                toAddTo.Append(toAdd);
            } else if (toAdd is ConvertedObject) {
				if ((toAdd as ConvertedObject).hasToStringMethod) {
					toAddTo.Append ((toAdd as ConvertedObject).originalObject.ToString ());
				}else{
					toAddTo.ConvertToString ((ConvertedObject)toAdd, printingConfiguration, depth);
				}
            } else if (toAdd is ConvertedMap) {
                toAddTo.ConvertToString((ConvertedMap)toAdd, printingConfiguration, depth+1);
            } else if (toAdd is ConvertedList) {
                toAddTo.ConvertToString((ConvertedList)toAdd, printingConfiguration, depth+1);
            } else {
                toAddTo.Append(toAdd.ToString());
            }
            return toAddTo;
        }
    }
}