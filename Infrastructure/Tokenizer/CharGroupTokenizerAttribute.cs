using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 字符组分词器
    /// https://www.elastic.co/guide/en/elasticsearch/reference/6.8/analysis-chargroup-tokenizer.html
    /// </summary>
    public class CharGroupTokenizerAttribute : CustomTokenizerAttribute
    {
        public CharGroupTokenizerAttribute(string name, char[] chars) : base(name, "char_group")
        {
            this.Chars = chars;
        }

        public CharGroupTokenizerAttribute(string name, CharGroupTokenizeOnChars charGroupTokenizeOnChar) : base(name, "char_group")
        {
            this.CharGroupTokenizeOnChars = charGroupTokenizeOnChar;
        }

        public CharGroupTokenizerAttribute(string name, char[] chars, CharGroupTokenizeOnChars charGroupTokenizeOnChars) : base(name, "char_group")
        {
            this.Chars = chars;
            this.CharGroupTokenizeOnChars = charGroupTokenizeOnChars;
        }

        public char[] Chars { get; private set; }

        public CharGroupTokenizeOnChars CharGroupTokenizeOnChars { get; set; } = CharGroupTokenizeOnChars.None;
    }

    /// <summary>
    /// 内置字符组字符
    /// </summary>
    [Flags]
    public enum CharGroupTokenizeOnChars
    {
        /// <summary>
        /// 未配置
        /// </summary>
        None = 0,

        /// <summary>
        /// 空格
        /// </summary>
        Whitespace = 1,

        /// <summary>
        /// 字母
        /// </summary>
        Letter = 2,

        /// <summary>
        /// 数字
        /// </summary>
        Digit = 4,

        /// <summary>
        /// 标点符号
        /// </summary>
        Punctuation = 8,

        /// <summary>
        /// 符号
        /// </summary>
        Symbol = 16
    }
}
