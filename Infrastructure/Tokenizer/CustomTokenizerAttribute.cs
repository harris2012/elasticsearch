using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 自定义分析器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class CustomTokenizerAttribute : Attribute
    {
        /// <summary>
        /// 自定义分析器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public CustomTokenizerAttribute(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name { get; private set; }

        public string Type { get; private set; }
    }
}
