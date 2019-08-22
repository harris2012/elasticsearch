using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSearch.Loader.Model
{
    public class MainModel
    {
        public string AssemblyFullName { get; set; }

        public List<ClassNode> ClassNodeList { get; set; }
    }
}
