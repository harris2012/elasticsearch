using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 布尔型
    /// </summary>
    public sealed class BooleanFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Boolean;

        public override string Type => "boolean";

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/null-value.html
        /// </summary>
        public bool? NullValue { get; private set; }

        public BooleanFieldAttribute()
        {
        }

        public BooleanFieldAttribute(bool nullValue)
        {
            this.NullValue = nullValue;
        }
    }
}
