using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 索引设置
    /// </summary>
    public class IndexSettings
    {
        /// <summary>
        /// 分片数 number_of_shards，默认值是5
        /// </summary>
        public int NumberOfShards { get; set; } = -1;

        /// <summary>
        /// 副本数 number_of_replicas，默认值是1
        /// </summary>
        public int NumberOfReplicas { get; set; } = -1;

        /// <summary>
        /// 最大字段数
        /// </summary>
        public int MappingTotalFieldsLimit { get; set; } = -1;

        /// <summary>
        /// index.max_ngram_diff
        /// <see cref="https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-ngram-tokenfilter.html"/>
        /// </summary>
        public int IndexMaxNGramDiff { get; set; }
    }
}
