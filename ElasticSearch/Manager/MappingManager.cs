using Infrastructure;
using Panosen.CodeDom;
using Panosen.CodeDom.CSharp.Engine;
using Panosen.CodeDom.JavaScript.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ElasticSearch.Manager
{
    public partial class MappingManager : ManagerBase
    {
        /// <summary>
        /// 用于拆分枚举
        /// </summary>
        private static readonly string[] CommaAndWhitespace = new string[] { " ", "," };

        public static void Process(Param param)
        {
            Assembly assembly = Assembly.LoadFrom(param.DLLPath);

            var types = assembly.GetTypes();

            var folder = Environment.CurrentDirectory;

            var mappingsFolder = Path.Combine(folder, "Mappings");

            JsCodeEngine jsCodeEngine = new JsCodeEngine();
            foreach (var type in types)
            {
                var indexAttribute = type.GetCustomAttribute<IndexAttribute>(false);
                if (indexAttribute == null)
                {
                    continue;
                }

                var dataObject = BuildMappingsFile(type);

                var stringBuilder = new StringBuilder();
                jsCodeEngine.GenerateDataObject(dataObject, new StringWriter(stringBuilder), new Panosen.CodeDom.JavaScript.Engine.GenerateOptions
                {
                    TabString = "  ",
                    DataArrayItemBreakLine = true
                });

                WriteToFile(Path.Combine(mappingsFolder, $"{type.Name.ToLowerCaseBreakLine()}.json"), stringBuilder.ToString());
            }
        }

        private static DataObject BuildMappingsFile(Type type)
        {
            var indexAttribute = type.GetCustomAttribute<IndexAttribute>(false);

            var customAnalyzerAttributeList = type.GetCustomAttributes<CustomAnalyzerAttribute>(false).ToList();
            var customTokenizerAttributeList = type.GetCustomAttributes<CustomTokenizerAttribute>(false).ToList();

            var dataObject = new DataObject();

            //alias
            if (indexAttribute.Aliases != null && indexAttribute.Aliases.Length > 0)
            {
                var aliasDataObject = dataObject.AddDataObject(DataKey.DoubleQuotationString("aliases"));
                foreach (var alias in indexAttribute.Aliases)
                {
                    aliasDataObject.AddDataObject(alias);
                }
            }

            //settings
            var settingsDataObject = BuildSettings(indexAttribute, customTokenizerAttributeList, customAnalyzerAttributeList);
            if (settingsDataObject != null)
            {
                dataObject.AddDataObject(DataKey.DoubleQuotationString("settings"), settingsDataObject);
            }

            //mappings
            var mappingsDataObject = BuildMappings(type);
            if (mappingsDataObject != null)
            {
                dataObject.AddDataObject(DataKey.DoubleQuotationString("mappings"), mappingsDataObject);
            }

            return dataObject;
        }
    }
}
