using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 对应一个文档里面的字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public abstract class FieldAttribute : Attribute
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否索引该字段
        /// 默认值是true
        /// </summary>
        public bool Index { get; set; } = true;

        /// <summary>
        /// 是否需要doc_values
        /// 默认值是true
        /// </summary>
        public bool DocValues { get; set; } = true;

        /// <summary>
        /// 字段类型
        /// </summary>
        public abstract FieldType FieldType { get; }

        /// <summary>
        /// ES内部字段类型
        /// </summary>
        public abstract string Type { get; }
    }

    ///// <summary>
    ///// 复杂类型，对应类或者数组或者List
    ///// </summary>
    //public sealed class ComplexFieldAttribute : FieldAttribute
    //{
    //    public override FieldType FieldType => FieldType.Complex;

    //    public override string Type => string.Empty;

    //    public Type ComplexType { get; set; }
    //}

    /// <summary>
    /// 整型
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

    /// <summary>
    /// 长整型
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
