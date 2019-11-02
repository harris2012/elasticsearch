using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// NGram 元分词器
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-ngram-tokenizer.html
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class NGramTokenizerAttribute : AbstractNGramTokenizerAttribute
    {
        public NGramTokenizerAttribute(string name) : base(name, "ngram")
        {
        }

        public NGramTokenizerAttribute(string name, int minGram, int maxGram, NGramTokenChar nGramTokenChar) : base(name, minGram, maxGram, nGramTokenChar, "ngram")
        {
        }
    }

    /// <summary>
    /// NGram 元分词器
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-ngram-tokenizer.html
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class EdgeNGramTokenizerAttribute : AbstractNGramTokenizerAttribute
    {
        public EdgeNGramTokenizerAttribute(string name) : base(name, "edge_ngram")
        {
        }

        public EdgeNGramTokenizerAttribute(string name, int minGram, int maxGram, NGramTokenChar nGramTokenChar) : base(name, minGram, maxGram, nGramTokenChar, "edge_ngram")
        {
        }
    }

    public abstract class AbstractNGramTokenizerAttribute : CustomTokenizerAttribute
    {
        public AbstractNGramTokenizerAttribute(string name, string type) : base(name, type)
        {
        }

        public AbstractNGramTokenizerAttribute(string name, int minGram, int maxGram, NGramTokenChar nGramTokenChar, string type) : base(name, type)
        {
            this.MinGram = minGram;
            this.MaxGram = maxGram;
            this.TokenChars = nGramTokenChar;
        }

        /// <summary>
        /// min_gram es默认值是1
        /// </summary>
        public int MinGram { get; set; }

        /// <summary>
        /// max_gram es默认值是 2
        /// </summary>
        public int MaxGram { get; set; }

        /// <summary>
        /// token_chars es默认值是 []
        /// </summary>
        public NGramTokenChar TokenChars { get; set; } = NGramTokenChar.NONE;
    }

    /// <summary>
    /// ngram tokenizer token_chars
    /// </summary>
    [Flags]
    public enum NGramTokenChar
    {
        /// <summary>
        /// 未设置
        /// </summary>
        NONE = 0,

        /// <summary>
        /// letter
        /// </summary>
        LETTER = 1,

        /// <summary>
        /// digit
        /// </summary>
        DIGIT = 2,

        /// <summary>
        /// whitespace
        /// </summary>
        WHITESPACE = 4,

        /// <summary>
        /// punctuation
        /// </summary>
        PUNCTUATION = 8,

        /// <summary>
        /// symbol
        /// </summary>
        SYMBOL = 16
    }
}
