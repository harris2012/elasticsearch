using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSearch.Loader.Model
{
    public class MethodNode : Node
    {
        /// <summary>
        /// 【Java】方法返回类型
        /// </summary>
        public Type ReturnType { get; set; }

        /// <summary>
        /// 方法的参数
        /// </summary>
        public List<MethodParameter> Parameters { get; set; }

        /// <summary>
        /// 属性名称
        /// example: Name
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 属性名称
        /// example: nameProperty
        /// </summary>
        public string PropertyJavaName { get; set; }
    }
}
