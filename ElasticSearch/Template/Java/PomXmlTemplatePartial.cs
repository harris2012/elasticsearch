using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Template.Java
{
    partial class PomXmlTemplate
    {
        public string GroupId { get; set; }

        public string ArtifactId { get; set; }

        public string Version { get; set; }

        public string BomVersion { get; set; }

        public string ReleaseRepo { get; set; }

        public string SnapshotRepo { get; set; }
    }
}
