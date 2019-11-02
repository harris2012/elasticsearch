using ElasticSearch.Loader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Template.Java
{
    partial class EsFieldsTemplate
    {
        /// <summary>
        /// 用于拆分枚举
        /// </summary>
        private static readonly string[] CommaAndWhitespace = new string[] { " ", "," };

        public string RootNamespace { get; set; }

        public string JavaRoot { get; set; }

        public ClassNode ClassNode { get; set; }
    }
}
