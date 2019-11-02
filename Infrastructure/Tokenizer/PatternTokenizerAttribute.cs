using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// <see cref="https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-pattern-tokenizer.html"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class PatternTokenizerAttribute : CustomTokenizerAttribute
    {
        public PatternTokenizerAttribute(string name) : base(name, "pattern")
        {
        }

        public PatternTokenizerAttribute(string name, string pattern) : base(name, "pattern")
        {
            this.Pattern = pattern;
        }

        public string Pattern { get; set; }
    }
}
