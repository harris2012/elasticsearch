using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 内置分词器
    /// <see cref="https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-analyzers.html"/>
    /// </summary>
    [Flags]
    public enum BuiltInAnalyzer
    {
        /// <summary>
        /// 未设置
        /// </summary>
        NONE = 0,

        /// <summary>
        /// 【内置分词器】standard
        /// </summary>
        STANDARD = 1,

        /// <summary>
        /// 【内置分词器】simple
        /// </summary>
        SIMPLE = 2,

        /// <summary>
        /// 【内置分词器】whitespace
        /// </summary>
        WHITESPACE = 4,

        /// <summary>
        /// 【内置分词器】stop
        /// </summary>
        STOP = 8,

        /// <summary>
        /// 【内置分词器】keyword
        /// </summary>
        KEYWORD = 16,

        /// <summary>
        /// 【内置分词器】pattern
        /// </summary>
        PATTERN = 32,

        /// <summary>
        /// 【内置分词器】english
        /// </summary>
        ENGLISH = 64,

        /// <summary>
        /// 【内置分词器】french
        /// </summary>
        FRENCH = 128,

        /// <summary>
        /// 【内置分词器】chinese
        /// </summary>
        CHINESE = 256,

        /// <summary>
        /// 【被指分词器】fingerprint
        /// </summary>
        FINGERPRINT = 512
    }
}
