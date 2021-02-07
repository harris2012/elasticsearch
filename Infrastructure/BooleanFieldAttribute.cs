using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// boolean
    /// </summary>
    public sealed class BooleanFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Boolean
        /// </summary>
        public override FieldType FieldType => FieldType.Boolean;

        /// <summary>
        /// boolean
        /// </summary>
        public override string Type => "boolean";

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/null-value.html
        /// </summary>
        public bool? NullValue { get; private set; }

        /// <summary>
        /// boolean
        /// </summary>
        public BooleanFieldAttribute()
        {
        }

        /// <summary>
        /// boolean
        /// </summary>
        /// <param name="nullValue"></param>
        public BooleanFieldAttribute(bool nullValue)
        {
            this.NullValue = nullValue;
        }
    }
}
