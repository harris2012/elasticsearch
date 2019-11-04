using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch
{
    public class Param
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [JsonProperty("projectName")]
        public string ProjectName { get; set; } = "Search.Entity";

        /// <summary>
        /// CodeFirst 程序集名称
        /// </summary>
        [JsonProperty("assemblyName")]
        public string AssemblyName { get; set; } = "Search.Entity";

        /// <summary>
        /// CodeFirst 根命名空间
        /// </summary>
        [JsonIgnore]
        public string RootNamespace { get { return AssemblyName; } }

        /// <summary>
        /// CodeFirst 项目 Guid
        /// </summary>
        public string ProjectGuid { get; set; } = Guid.NewGuid().ToString("D").ToUpper();

        /// <summary>
        /// 解决方案文件
        /// </summary>
        public string ExampleGuid { get; set; } = Guid.NewGuid().ToString("D").ToUpper();

        /// <summary>
        /// CodeFirst 解决方案 Guid
        /// </summary>
        public string SolutionGuid { get; set; } = Guid.NewGuid().ToString("D").ToUpper();

        /// <summary>
        /// DLL 路径
        /// </summary>
        [JsonIgnore]
        public string DLLPath { get { return Path.Combine("CodeFirst", $"{ProjectName}", "bin", "Debug", "net452", $"{AssemblyName}.dll"); } }

        /// <summary>
        /// XML 路径
        /// </summary>
        [JsonIgnore]
        public string XMLPath { get { return Path.Combine("CodeFirst", $"{ProjectName}", "bin", "Debug", "net452", $"{AssemblyName}.xml"); } }

        public string JavaRootPackage { get; set; } = "com.ctrip.demo";

        public string GroupId { get; set; } = "com.ctrip.tour";

        public string ArtifactId { get; set; } = "booking-service-contract";

        public string Version { get; set; } = "1.0.0";
    }
}
