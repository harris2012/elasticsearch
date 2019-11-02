using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 索引分析器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class IndexAnalyzerAttribute : Attribute
    {
        /// <summary>
        /// 索引分析器
        /// </summary>
        public IndexAnalyzerAttribute(Type analyzerType)
        {
            this.AnalyzerType = analyzerType;
        }

        /// <summary>
        /// 分析器类型
        /// </summary>
        public Type AnalyzerType { get; private set; }
    }
}
