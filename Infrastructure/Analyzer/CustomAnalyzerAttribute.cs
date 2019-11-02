using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 自定义分词器
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-custom-analyzer.html
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class CustomAnalyzerAttribute : Attribute
    {
        /// <summary>
        /// 自定义分词名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tokenizer"></param>
        public CustomAnalyzerAttribute(string name, string tokenizer)
        {
            this.Name = name;
            this.Tokenizer = tokenizer;
        }

        /// <summary>
        /// 分析器名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 分词器
        /// </summary>
        public string Tokenizer { get; private set; }

        /// <summary>
        /// 内置字符过滤器
        /// </summary>
        public BuiltInCharacterFilters BuiltInCharacterFilters { get; set; }

        /// <summary>
        /// 内置token过滤器
        /// </summary>
        public BuiltInTokenFilters BuiltInTokenFilters { get; set; }

        /// <summary>
        /// PositionIncrementGap
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/position-increment-gap.html
        /// </summary>
        public int position_increment_gap { get; set; }

    }
}
