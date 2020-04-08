using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 长整型
    /// https://www.elastic.co/guide/en/elasticsearch/reference/current/number.html
    /// </summary>
    public sealed class LongFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Long;

        public override string Type => "long";

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/null-value.html
        /// </summary>
        public long? NullValue { get; private set; }

        public LongFieldAttribute()
        {
        }

        public LongFieldAttribute(int nullValue)
        {
            this.NullValue = nullValue;
        }
    }
}
