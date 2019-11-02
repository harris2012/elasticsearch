using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// Token Filters
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-tokenfilters.html
    /// </summary>
    [Flags]
    public enum BuiltInTokenFilters
    {
        /// <summary>
        /// 未配置
        /// </summary>
        None = 0,

        /// <summary>
        /// 控制长度 length
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-length-tokenfilter.html
        /// </summary>
        LENGTH = 1,

        /// <summary>
        /// 增加同义词 synonyms
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-tokenfilters.html
        /// </summary>
        SYNONYMS = 2,

        /// <summary>
        /// asciifolding
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-asciifolding-tokenfilter.html
        /// </summary>
        ASCIIFOLDING = 4,

        /// <summary>
        ///  转换为小写 lowercase
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-lowercase-tokenfilter.html
        /// </summary>
        LOWERCASE = 8,

        /// <summary>
        /// 转换为大写 uppercase
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-uppercase-tokenfilter.html
        /// </summary>
        UPPERCASE = 16,

        /// <summary>
        /// 删除重复 remove_duplicates
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-remove-duplicates-tokenfilter.html
        /// </summary>
        REMOVE_DUPLICATES = 32

    }
}
