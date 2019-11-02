using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSearch.Loader.Model
{
    public class PropertyNode : Node
    {
        public Type PropertyType { get; set; }

        public FieldAttribute FieldAttribute { get; set; }
    }
}
