using ElasticSearch.Template.CodeFirst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Manager
{
    public class SetupManager : ManagerBase
    {
        public static void Process(Param param)
        {
            var folder = Environment.CurrentDirectory;

            var codeFirstFolder = Path.Combine(folder, "CodeFirst");

            var projectFolder = Path.Combine(codeFirstFolder, param.ProjectName);

            WriteToFile(Path.Combine(codeFirstFolder, ".gitignore"), CodeResource.CSharpGitIgnore);

            WriteToFile(Path.Combine(codeFirstFolder, "Library", "Infrastructure.dll"), CodeResource.Infrastructure_dll);

            WriteToFile(Path.Combine(codeFirstFolder, "Library", "Infrastructure.xml"), CodeResource.Infrastructure_xml);

            WriteToFile(Path.Combine(codeFirstFolder, $"{param.ProjectName}.sln"), new SolutionTemplate
            {
                ProjectName = param.ProjectName,
                SolutionGuid = param.SolutionGuid,
                ProjectGuid = param.ProjectGuid
            }.TransformText());

            WriteToFile(Path.Combine(projectFolder, $"{param.ProjectName}.csproj"), new ProjectTemplate
            {
                RootNamespace = param.AssemblyName,
                AssemblyName = param.AssemblyName
            }.TransformText());

            WriteToFile(Path.Combine(projectFolder, "Product.cs"), new ProductTemplate
            {
                RootNamespace = param.RootNamespace
            }.TransformText());

            WriteToFile(Path.Combine(projectFolder, "Book.cs"), new BookTemplate
            {
                RootNamespace = param.RootNamespace
            }.TransformText());

            WriteToFile(Path.Combine(projectFolder, "Tag.cs"), new TagTemplate
            {
                RootNamespace = param.RootNamespace
            }.TransformText());
        }
    }
}
