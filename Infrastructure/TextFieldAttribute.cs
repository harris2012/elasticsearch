using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 字符串(分词)
    /// </summary>
    public sealed class TextFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Text;

        public override string Type => "text";

        /// <summary>
        /// `ignore_above`
        /// https://www.elastic.co/guide/en/elasticsearch/reference/current/ignore-above.html
        /// </summary>
        public int KeywordIgnoreAbove { get; set; } = 256;

        /// <summary>
        /// 内置分词器
        /// </summary>
        public BuiltInAnalyzer BuiltInAnalyzer { get; set; } = BuiltInAnalyzer.NONE;

        /// <summary>
        /// IK分词器
        /// </summary>
        public IKAnalyzer IKAnalyzer { get; set; } = IKAnalyzer.NONE;

        /// <summary>
        /// 自定义分词器
        /// </summary>
        public string[] CustomAnalyzer { get; set; }

        /// <summary>
        /// 默认分析器
        /// </summary>
        public string DefaultAnalyzer { get; private set; }

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/null-value.html
        /// </summary>
        public string NullValue { get; set; }

        /// <summary>
        /// 使用`标准分析器`作为基础分析器
        /// </summary>
        /// <param name="defaultAnalyzer">基础分析器</param>
        public TextFieldAttribute(BuiltInAnalyzer defaultAnalyzer)
        {
            if (defaultAnalyzer == BuiltInAnalyzer.NONE)
            {
                return;
            }

            this.DefaultAnalyzer = defaultAnalyzer.ToString().ToLower();
        }

        /// <summary>
        /// 使用`IK分析器`作为基础分析器
        /// </summary>
        /// <param name="defaultAnalyzer">基础分析器</param>
        public TextFieldAttribute(IKAnalyzer defaultAnalyzer)
        {
            if (defaultAnalyzer == IKAnalyzer.NONE)
            {
                return;
            }

            this.DefaultAnalyzer = defaultAnalyzer.ToString().ToLower();
        }

        /// <summary>
        /// 使用`自定义分析器`作为基础分析器
        /// </summary>
        /// <param name="defaultAnalyzer">基础分析器</param>
        public TextFieldAttribute(string defaultAnalyzer)
        {
            this.DefaultAnalyzer = defaultAnalyzer;
        }

        /// <summary>
        /// 不指定基础分析器
        /// </summary>
        public TextFieldAttribute()
        {
        }
    }
}
