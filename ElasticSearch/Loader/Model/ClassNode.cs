using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSearch.Loader.Model
{
    public class ClassNode : Node
    {
        public string Namespace { get; set; }

        public string FullName { get; set; }

        public bool IsAbstract { get; set; }

        public string BaseTypeName { get; set; }

        public string BaseTypeFullName { get; set; }

        /// <summary>
        /// 需要包含的其他类型(给Baiji用)
        /// </summary>
        public List<Type> Includes { get; set; }

        /// <summary>
        /// 需要import的其他类型(给java用)
        /// </summary>
        public List<string> Imports { get; set; }

        /// <summary>
        /// 服务原始命名空间
        /// </summary>
        public string OriginalServiceNamespace { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public List<PropertyNode> PropertyNodeList { get; private set; } = new List<PropertyNode>();

        /// <summary>
        /// 方法
        /// </summary>
        public List<MethodNode> MethodNodeList { get; private set; } = new List<MethodNode>();
    }
}
