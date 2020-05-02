using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.CodeDom
{
    public class CodeWriter
    {
        private readonly TextWriter textWriter;

        public CodeWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public static implicit operator CodeWriter(TextWriter textWriter)
        {
            return new CodeWriter(textWriter);
        }

        #region Call TextWriter Members

        public CodeWriter Write(string value)
        {
            textWriter.Write(value);
            return this;
        }

        public CodeWriter WriteLine(string value)
        {
            textWriter.WriteLine(value);
            return this;
        }

        public CodeWriter WriteLine()
        {
            textWriter.WriteLine();
            return this;
        }

        #endregion
    }
}
