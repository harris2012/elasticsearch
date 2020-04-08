using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 字符串(不分词)
    /// </summary>
    public sealed class KeywordFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Keyword;

        /// <summary>
        /// `ignore_above`
        /// https://www.elastic.co/guide/en/elasticsearch/reference/current/ignore-above.html
        /// </summary>
        public int IgnoreAbove { get; set; } = 256;

        public override string Type => "keyword";

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/null-value.html
        /// </summary>
        public string NullValue { get; set; }
    }
}
