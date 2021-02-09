using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 自定义过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class CustomFilterAttribute : Attribute
    {
        /// <summary>
        /// 自定义过滤器
        /// </summary>
        public CustomFilterAttribute(string name, string type, string[] properties = null)
        {
            this.Name = name;
            this.Type = type;
            this.Properties = properties;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 自定义属性设置
        /// </summary>
        public string[] Properties { get; set; }
    }
}
