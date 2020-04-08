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
        /// 是否需要doc_values。用于排序、聚合以及脚本操作
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

}
