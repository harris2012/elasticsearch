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
        public string RootNamespace { get; set; }

        public string JavaRoot { get; set; }

        public ClassNode ClassNode { get; set; }
    }
}
