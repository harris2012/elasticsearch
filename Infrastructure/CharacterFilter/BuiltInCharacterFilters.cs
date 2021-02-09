using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-charfilters.html
    /// </summary>
    [Flags]
    public enum BuiltInCharacterFilters
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 【HTML Strip Character Filter】html_strip
        /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-htmlstrip-charfilter.html
        /// </summary>
        HtmlCtrip = 1
    }
}
