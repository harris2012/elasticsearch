using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public abstract class TokenFilter
    {
    }

    /// <summary>
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-asciifolding-tokenfilter.html
    /// </summary>
    public sealed class AsciifoldingTokenFilter : TokenFilter
    {

    }

    /// <summary>
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-length-tokenfilter.html
    /// </summary>
    public sealed class LengthTokenFilter : TokenFilter
    {
        public int Min { get; set; }

        public int Max { get; set; }
    }

    /// <summary>
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-lowercase-tokenfilter.html
    /// </summary>
    public sealed class LowercaseTokenFilter : TokenFilter
    {
        public LowercaseLanguage Language { get; set; }
    }

    public enum LowercaseLanguage
    {
        None,

        Greek,

        Irish,

        Turkish
    }

    /// <summary>
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-ngram-tokenfilter.html
    /// </summary>
    public sealed class NGramTokenFilter : TokenFilter
    {
        public int MinGram { get; set; } = 1;

        public int MaxGram { get; set; } = 2;
    }
}
