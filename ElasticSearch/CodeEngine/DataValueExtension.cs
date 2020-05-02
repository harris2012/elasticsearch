using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.CodeDom.Js
{
    public static class DataValueExtension
    {
        public static void SetValue(this DataValue dataValue, bool value)
        {
            dataValue.Value = value ? "true" : "false";
        }

        public static void SetValue(this DataValue dataValue, int value)
        {
            dataValue.Value = value.ToString();
        }

        public static void SetValue(this DataValue dataValue, long value)
        {
            dataValue.Value = value.ToString();
        }

        public static void SetValue(this DataValue dataValue, string value)
        {
            dataValue.Value = value;
        }
    }
}
