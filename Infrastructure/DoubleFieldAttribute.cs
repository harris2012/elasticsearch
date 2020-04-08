using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// double型
    /// https://www.elastic.co/guide/en/elasticsearch/reference/current/number.html
    /// </summary>
    public sealed class DoubleFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Double;

        public override string Type => "double";

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/null-value.html
        /// </summary>
        public double? NullValue { get; private set; }

        public DoubleFieldAttribute()
        {
        }

        public DoubleFieldAttribute(double nullValue)
        {
            this.NullValue = nullValue;
        }
    }
}
