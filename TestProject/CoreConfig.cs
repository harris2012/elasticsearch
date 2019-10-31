using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    /// <summary>
    /// 生成配置
    /// </summary>
    public static class CoreConfig
    {
        /// <summary>
        /// 如果属性没有指定FieldAttribute，根据属性的类型推断默认的FieldAttribute
        /// </summary>
        public static bool PropertyTypeAsFiledAttribute
        {
            get
            {
                return "true".Equals(ConfigurationManager.AppSettings["PropertyTypeAsFiledAttribute"], StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
