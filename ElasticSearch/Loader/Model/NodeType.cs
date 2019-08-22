using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSearch.Loader.Model
{
    public enum NodeType
    {
        None = 1,

        Class = 2,

        Enum = 3,

        Property = 4,

        Method = 5,

        Namespace = 7,

        Root = 8
    }
}
