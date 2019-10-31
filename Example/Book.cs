using Infrastructure;
using System;

namespace Example
{
    /// <summary>
    /// 产品
    /// </summary>
    [Index(Dynamic = Dynamic.False, NumberOfShards = 2)]
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
        [IntegerField]
        public int UseInteger { get; set; }

        /// <summary>
        /// 使用 long
        /// </summary>
        [LongField]
        public long UseLong { get; set; }

        /// <summary>
        /// 使用 keyword
        /// </summary>
        [KeywordField]
        public string UseKeyword { get; set; }

        /// <summary>
        /// 使用 text
        /// </summary>
        [TextField]
        public string UseText { get; set; }
    }
}
