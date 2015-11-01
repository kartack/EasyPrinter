using UnityEngine;
using System.Collections;
using System.Text;
using System.Threading;

namespace EasyPrinter {
    public static class ExtensionMethods {
        public static string EasyPrint(this System.Object toPrint) {
            if(System.Object.ReferenceEquals(toPrint, null)) {
                return "null";
            }

            return SerializationMethods.ConvertToSingleLineString(ObjectConverter.ConvertObject(toPrint));
        }
    }
}
