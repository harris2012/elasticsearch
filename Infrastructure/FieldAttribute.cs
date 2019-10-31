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
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// 字段
        /// </summary>
        /// <param name="fieldType">字段类型</param>
        public FieldAttribute(FieldType fieldType)
        {
            this.FieldType = fieldType;
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        public FieldType FieldType { get; private set; }
    }
}
