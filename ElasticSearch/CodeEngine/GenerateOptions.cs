using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.CodeDom.Js.Engine
{
    public class GenerateOptions
    {
#if DEBUG
        public string TabString { get; set; } = "\t";
#else
        public string TabString { get; set; } = "    ";
#endif

        private int indentSize = 0;

        /// <summary>
        /// 缩进个数
        /// </summary>
        public int IndentSize
        {
            get
            {
                return this.indentSize;
            }
            set
            {
                this.indentSize = value;
                BuildIndentString();
            }
        }

        /// <summary>
        /// 缩进字符
        /// </summary>
        public string IndentString { get; private set; } = string.Empty;

        /// <summary>
        /// 增加缩进
        /// </summary>
        public void PushIndent()
        {
            this.indentSize = this.indentSize + 1;

            BuildIndentString();
        }

        /// <summary>
        /// 减少缩进
        /// </summary>
        public void PopIndent()
        {
            if (this.indentSize > 0)
            {
                this.indentSize = this.indentSize - 1;
            }

            BuildIndentString();
        }

        /// <summary>
        /// 构建缩进
        /// </summary>
        /// <returns></returns>
        private void BuildIndentString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.indentSize; i++)
            {
                builder.Append(this.TabString);
            }
            this.IndentString = builder.ToString();
        }
    }
}
