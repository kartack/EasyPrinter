using UnityEngine;
using System.Collections;
using System.Text;
using System.Threading;

namespace EasyPrinter {
    public static class ExtensionMethods {
        public static string EasyPrint(this System.Object toPrint) {
            return SerializationMethods.ConvertToSingleLineString(ObjectConverter.ConvertObject(toPrint));
        }

        public static string EasyPrintMultiline(this System.Object toPrint) {
            return SerializationMethods.ConvertToMultilineLineString(ObjectConverter.ConvertObject(toPrint));
        }
    }
}
