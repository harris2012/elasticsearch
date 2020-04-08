using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 整型
    /// https://www.elastic.co/guide/en/elasticsearch/reference/current/number.html
    /// </summary>
    public sealed class IntegerFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Integer;

        public override string Type => "integer";

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/null-value.html
        /// </summary>
        public int? NullValue { get; private set; }

        public IntegerFieldAttribute()
        {
        }

        public IntegerFieldAttribute(int nullValue)
        {
            this.NullValue = nullValue;
        }
    }
}
