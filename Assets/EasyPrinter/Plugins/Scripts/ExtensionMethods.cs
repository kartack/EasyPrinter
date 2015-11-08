using UnityEngine;
using System.Collections;
using System.Text;
using System.Threading;

namespace EasyPrinter {
    public static class ExtensionMethods {
        public static string EasyPrint(this System.Object toPrint) {
            return SerializationMethods.ConvertToSingleLineString(ObjectConverter.ConvertObject(toPrint, AttributeExtensions.GetListOfFieldsToPrint(toPrint)));
        }

        public static string EasyPrintMultiline(this System.Object toPrint) {
            return SerializationMethods.ConvertToMultilineLineString(ObjectConverter.ConvertObject(toPrint, AttributeExtensions.GetListOfFieldsToPrint(toPrint)));
        }

        public static string EasyPrintPrintOnly(this System.Object toPrint, params string[] ourParams) {
            return SerializationMethods.ConvertToSingleLineString(ObjectConverter.ConvertObject(toPrint, AttributeExtensions.GetListOfFieldsToPrint(toPrint, ourParams, InputListType.PRINT_ONLY)));
        }

        public static string EasyPrintMultilinePrintOnly(this System.Object toPrint, params string[] ourParams) {
            return SerializationMethods.ConvertToMultilineLineString(ObjectConverter.ConvertObject(toPrint, AttributeExtensions.GetListOfFieldsToPrint(toPrint, ourParams, InputListType.PRINT_ONLY)));
        }

        public static string EasyPrintDontPrint(this System.Object toPrint, params string[] ourParams) {
            return SerializationMethods.ConvertToSingleLineString(ObjectConverter.ConvertObject(toPrint, AttributeExtensions.GetListOfFieldsToPrint(toPrint, ourParams, InputListType.DONT_PRINT)));
        }

        public static string EasyPrintMultilineDontPrint(this System.Object toPrint, params string[] ourParams) {
            return SerializationMethods.ConvertToMultilineLineString(ObjectConverter.ConvertObject(toPrint, AttributeExtensions.GetListOfFieldsToPrint(toPrint, ourParams, InputListType.DONT_PRINT)));
        }

    }
}
