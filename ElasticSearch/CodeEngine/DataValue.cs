using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.CodeDom.Js
{
    public class DataValue: DataValueOrObject
    {
        public string Value { get; set; }
        public static implicit operator DataValue(bool value)
        {
            var dataValue = new DataValue();
            dataValue.Value = value ? "true" : "false";
            return dataValue;
        }

        public static implicit operator DataValue(int value)
        {
            var dataValue = new DataValue();
            dataValue.Value = value.ToString();
            return dataValue;
        }

        public static implicit operator DataValue(long value)
        {
            var dataValue = new DataValue();
            dataValue.Value = value.ToString();
            return dataValue;
        }

        public static implicit operator DataValue(string value)
        {
            var stringValue = new DataValue();
            stringValue.Value = value;
            return stringValue;
        }
    }
}
