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
        /// 字段类型
        /// </summary>
        public abstract FieldType FieldType { get; }
    }

    /// <summary>
    /// 整型
    /// </summary>
    public sealed class IntegerFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Integer;
    }

    /// <summary>
    /// 长整型
    /// </summary>
    public sealed class LongFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Long;
    }

    /// <summary>
    /// 字符串(不分词)
    /// </summary>
    public sealed class KeywordFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Keyword;
    }

    /// <summary>
    /// 字符串(分词)
    /// </summary>
    public sealed class TextFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Text;
    }
}
