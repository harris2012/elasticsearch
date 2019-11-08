using Infrastructure;
using System;
using System.Collections.Generic;

namespace Example
{
    /// <summary>
    /// 图书
    /// </summary>
    [Index(IndexName = "book-index", Aliases = new string[] { "book-alias" }, Dynamic = Dynamic.False, NumberOfShards = 1, NumberOfReplicas = 4, MappingTotalFieldsLimit = 50000)]
    [NGramTokenizer("trigram_tokenizer", 1, 3, NGramTokenChar.LETTER | NGramTokenChar.DIGIT)]
    [EdgeNGramTokenizer("edge_ten_tokenizer", 1, 10, NGramTokenChar.LETTER | NGramTokenChar.DIGIT)]
    [EdgeNGramTokenizer("edge_twenty_tokenizer", 1, 20, NGramTokenChar.LETTER | NGramTokenChar.DIGIT)]
    [PatternTokenizer("comma_tokenizer", ",")]
    [CustomAnalyzer("trigram_analyzer", "trigram_tokenizer", BuiltInTokenFilters = BuiltInTokenFilters.LOWERCASE)]
    [CustomAnalyzer("edge_ten_analyzer", "edge_ten_tokenizer", BuiltInTokenFilters = BuiltInTokenFilters.LOWERCASE)]
    [CustomAnalyzer("edge_twenty_analyzer", "edge_twenty_tokenizer", BuiltInTokenFilters = BuiltInTokenFilters.LOWERCASE)]
    [CustomAnalyzer("comma_analyzer", "comma_tokenizer")]
    public class Book
    {
        /// <summary>
        /// 推断 int
        /// </summary>
        public int AssumeInt { get; set; }

        /// <summary>
        /// 推断 long
        /// </summary>
        public long AssumeLong { get; set; }

        /// <summary>
        /// 推断 keyword
        /// </summary>
        public string AssumeKeyword { get; set; }

        /// <summary>
        /// 使用 integer
        /// </summary>
        [IntegerField(123)]
        public int UseInteger { get; set; }

        /// <summary>
        /// 使用 long
        /// </summary>
        [LongField]
        public long UseLong { get; set; }

        /// <summary>
        /// 使用 keyword
        /// </summary>
        [KeywordField(IgnoreAbove = 128)]
        public string UseKeyword { get; set; }

        /// <summary>
        /// 使用 text
        /// </summary>
        [TextField]
        public string UseText { get; set; }

        /// <summary>
        /// 只存储，不索引
        /// </summary>
        [TextField(Index = false, DocValues = false)]
        public string NotIndexMe { get; set; }

        /// <summary>
        /// 只存储，不索引
        /// </summary>
        [TextField(NullValue = "NULL")]
        public string WithNullValue { get; set; }

        /// <summary>
        /// 使用分词器
        /// </summary>
        [TextField(
            IKAnalyzer = IKAnalyzer.IK_SMART | IKAnalyzer.IK_MAX_WORD,
            BuiltInAnalyzer = BuiltInAnalyzer.SIMPLE | BuiltInAnalyzer.WHITESPACE,
            CustomAnalyzer = new string[] { "ngram_1_1" })]
        public string UseAnalyzer { get; set; }

        /// <summary>
        /// 带默认分析器
        /// </summary>
        [TextField(IKAnalyzer.IK_SMART)]
        public string WithDefaultAnalyzer { get; set; }
    }
}
