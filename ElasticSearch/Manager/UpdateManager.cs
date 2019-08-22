using ElasticSearch.Loader;
using ElasticSearch.Template;
using ElasticSearch.Template.Java;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Manager
{
    public class UpdateManager : ManagerBase
    {
        public static void Process(Param param)
        {
            var mainModel = AssemblyLoader.LoadAssembly(param.DLLPath, param.XMLPath);
            if (mainModel == null)
            {
                return;
            }

            if (mainModel.ClassNodeList == null && mainModel.ClassNodeList.Count == 0)
            {
                return;
            }

            var folder = Environment.CurrentDirectory;

            var projectFolder = Path.Combine(folder, "Java");

            var srcPath = Path.Combine(projectFolder, "src", "main", "java");

            WriteToFile(Path.Combine(projectFolder, "pom.xml"), new PomXmlTemplate
            {
                GroupId = param.GroupId,
                ArtifactId = param.ArtifactId,
                Version = param.Version,
                BomVersion = ConfigurationManager.AppSettings["BomVersion"],
                ReleaseRepo = ConfigurationManager.AppSettings["ReleaseRepo"],
                SnapshotRepo = ConfigurationManager.AppSettings["SnapshotRepo"]
            }.TransformText());

            WriteToFile(Path.Combine(projectFolder, ".gitignore"), CodeResource.JavaGitIgnore);

            foreach (var classNode in mainModel.ClassNodeList)
            {
                WriteToFile(Path.Combine(srcPath, classNode.FullName.Replace(mainModel.AssemblyFullName, param.JavaRootPackage).Replace(".", "\\") + ".java"), new EsEntityTemplate
                {
                    RootNamespace = param.RootNamespace,
                    JavaRoot = param.JavaRootPackage,
                    ClassNode = classNode
                }.TransformText());

                WriteToFile(Path.Combine(srcPath, classNode.FullName.Replace(mainModel.AssemblyFullName, param.JavaRootPackage).Replace(".", "\\") + "Fields.java"), new EsFieldsTemplate
                {
                    RootNamespace = param.RootNamespace,
                    JavaRoot = param.JavaRootPackage,
                    ClassNode = classNode
                }.TransformText());
            }
        }
    }
}
