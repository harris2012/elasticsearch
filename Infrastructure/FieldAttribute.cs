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
        /// 字段类型
        /// </summary>
        public abstract FieldType FieldType { get; }
    }
}
