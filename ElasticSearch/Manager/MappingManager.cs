using Infrastructure;
using Savory.CodeDom.Js;
using Savory.CodeDom.Js.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ElasticSearch.Manager
{
    public class MappingManager : ManagerBase
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
                jsCodeEngine.GenerateDataObject(dataObject, new StringWriter(stringBuilder), new GenerateOptions
                {
                    TabString = "  "
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
                var aliasDataObject = dataObject.AddDataObject("aliases");
                foreach (var alias in indexAttribute.Aliases)
                {
                    aliasDataObject.AddDataObject(alias);
                }
            }

            //settings
            var settingsDataObject = BuildSettings(indexAttribute, customTokenizerAttributeList, customAnalyzerAttributeList);
            if (settingsDataObject != null)
            {
                dataObject.AddDataObject("settings", settingsDataObject);
            }

            //mappings
            var mappingsDataObject = BuildMappings(type);
            if (mappingsDataObject != null)
            {
                dataObject.AddDataObject("mappings", mappingsDataObject);
            }

            return dataObject;
        }

        private static DataObject BuildSettings(IndexAttribute indexAttribute, List<CustomTokenizerAttribute> customTokenizerAttributeList, List<CustomAnalyzerAttribute> customAnalyzerAttributeList)
        {
            if (indexAttribute.NumberOfReplicas == 0 && indexAttribute.NumberOfShards > 0 && (customAnalyzerAttributeList == null || customAnalyzerAttributeList.Count == 0) && (customTokenizerAttributeList == null || customTokenizerAttributeList.Count == 0))
            {
                return null;
            }

            var returnSettingsDataObject = false;
            var settingsDataObject = new DataObject();
            if (indexAttribute.NumberOfShards > 0)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataValue("number_of_shards", indexAttribute.NumberOfShards);
            }
            if (indexAttribute.NumberOfReplicas > 0)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataValue("number_of_replicas", indexAttribute.NumberOfReplicas);
            }
            if (indexAttribute.MappingTotalFieldsLimit > 0)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataValue("mapping.total_fields.limit", indexAttribute.MappingTotalFieldsLimit);
            }

            var withAnalysisDataObject = false;
            var analysisDataObject = new DataObject();

            if (customTokenizerAttributeList != null && customTokenizerAttributeList.Count > 0)
            {
                withAnalysisDataObject = true;
                var tokenizerDataObject = analysisDataObject.AddDataObject("tokenizer");
                foreach (var customTokenizerAttribute in customTokenizerAttributeList)
                {
                    var tokenizerBody = BuildTokenizerBody(customTokenizerAttribute);
                    tokenizerDataObject.AddDataObject(customTokenizerAttribute.Name, tokenizerBody);
                }
            }

            if (customAnalyzerAttributeList != null && customAnalyzerAttributeList.Count > 0)
            {
                withAnalysisDataObject = true;
                var analyzerDataObject = analysisDataObject.AddDataObject("analyzer");
                foreach (var customAnalyzerAttribute in customAnalyzerAttributeList)
                {
                    var analyzerProperties = BuildAnalyzerProperties(customAnalyzerAttribute);
                    analyzerDataObject.AddDataObject(customAnalyzerAttribute.Name, analyzerProperties);
                }
            }

            if (withAnalysisDataObject)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataObject("analysis", analysisDataObject);
            }

            if (returnSettingsDataObject)
            {
                return settingsDataObject;
            }
            else
            {
                return null;
            }
        }

        private static DataObject BuildTokenizerBody(CustomTokenizerAttribute customTokenizerAttribute)
        {
            if (customTokenizerAttribute is AbstractNGramTokenizerAttribute)
            {
                return BuildNGramTokenizer(customTokenizerAttribute as AbstractNGramTokenizerAttribute);
            }

            if (customTokenizerAttribute is PatternTokenizerAttribute)
            {
                return BuildPatternTokenier(customTokenizerAttribute as PatternTokenizerAttribute);
            }

            if (customTokenizerAttribute is CharGroupTokenizerAttribute)
            {
                return BuildCharGroupTokenizer(customTokenizerAttribute as CharGroupTokenizerAttribute);
            }

            return null;
        }

        private static DataObject BuildNGramTokenizer(AbstractNGramTokenizerAttribute abstractNGramTokenizerAttribute)
        {
            DataObject dataObject = new DataObject();
            dataObject.AddDataValue("type", abstractNGramTokenizerAttribute.Type);

            if (abstractNGramTokenizerAttribute.MinGram > 0)
            {
                dataObject.AddDataValue("min_gram", abstractNGramTokenizerAttribute.MinGram);
            }

            if (abstractNGramTokenizerAttribute.MaxGram > 0)
            {
                dataObject.AddDataValue("max_gram", abstractNGramTokenizerAttribute.MaxGram);
            }

            var tokenChars = abstractNGramTokenizerAttribute.TokenChars.ToString()
                .ToLower()
                .Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries)
                .Where(v => !"none".Equals(v))
                .ToList();

            if (tokenChars.Count > 0)
            {
                var tokenCharsArray = dataObject.AddDataArray("token_chars");
                foreach (var item in tokenChars)
                {
                    tokenCharsArray.AddDataValue(item);
                }
            }

            return dataObject;
        }

        private static DataObject BuildPatternTokenier(PatternTokenizerAttribute patternTokenizerAttribute)
        {
            DataObject dataObject = new DataObject();
            dataObject.AddDataValue("type", patternTokenizerAttribute.Type);

            if (!string.IsNullOrEmpty(patternTokenizerAttribute.Pattern))
            {
                dataObject.AddDataValue("pattern", patternTokenizerAttribute.Pattern);
            }

            return dataObject;
        }

        private static DataObject BuildCharGroupTokenizer(CharGroupTokenizerAttribute charGroupTokenizerAttribute)
        {
            DataObject dataObject = new DataObject();
            dataObject.AddDataValue("type", charGroupTokenizerAttribute.Type);

            List<string> tokenizeOChars = new List<string>();

            if (charGroupTokenizerAttribute.Chars != null && charGroupTokenizerAttribute.Chars.Length > 0)
            {
                tokenizeOChars.AddRange(charGroupTokenizerAttribute.Chars.Select(v => v.ToString()));
            }

            var charGroupTokenizeOnChars = charGroupTokenizerAttribute.CharGroupTokenizeOnChars.ToString()
                .ToLower()
                .Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries)
                .Where(v => !"none".Equals(v))
                .ToList();
            if (charGroupTokenizeOnChars.Count > 0)
            {
                tokenizeOChars.AddRange(charGroupTokenizeOnChars);
            }

            var array = dataObject.AddDataArray("tokenize_on_chars");
            foreach (var item in tokenizeOChars)
            {
                array.AddDataValue(item);
            }

            return dataObject;
        }

        private static DataObject BuildAnalyzerProperties(CustomAnalyzerAttribute customAnalyzerAttribute)
        {
            DataObject dataObject = new DataObject();

            dataObject.AddDataValue("tokenizer", customAnalyzerAttribute.Tokenizer);

            {
                List<string> tokenFilters = new List<string>();

                var builtInTokenFilters = customAnalyzerAttribute.BuiltInTokenFilters.ToString()
                    .ToLower()
                    .Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries)
                    .Where(v => !"none".Equals(v))
                    .ToList();
                tokenFilters.AddRange(builtInTokenFilters);

                if (tokenFilters.Count > 0)
                {
                    var dataArray = dataObject.AddDataArray("filter");
                    foreach (var item in tokenFilters)
                    {
                        dataArray.AddDataValue(item);
                    }
                }
            }

            return dataObject;
        }

        private static DataObject BuildMappings(Type type)
        {
            var indexAttribute = type.GetCustomAttribute<IndexAttribute>(false);
            if (indexAttribute == null)
            {
                return null;
            }

            DataObject dataObject = new DataObject();
            var _doc = dataObject.AddDataObject(indexAttribute.TypeName ?? "_doc");

            switch (indexAttribute.Dynamic)
            {
                case Dynamic.False:
                    _doc.AddDataValue("dynamic", false);
                    break;
                case Dynamic.Strict:
                    _doc.AddDataValue("dynamic", "strict");
                    break;
                case Dynamic.True:
                default:
                    break;
            }

            var propertyMap = BuildProperties(type);
            if (propertyMap != null && propertyMap.Count > 0)
            {
                var _properties = _doc.AddDataObject("properties");
                foreach (var item in propertyMap)
                {
                    _properties.AddDataObject(item.Key, item.Value);
                }
            }

            return dataObject;
        }

        private static Dictionary<string, DataObject> BuildProperties(Type type)
        {
            var fieldAttributes = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(field => GetFieldAttribute(field, field.Name.ToLowerCaseUnderLine()))
                .Where(fieldAttribute => fieldAttribute != null)
                .OrderBy(v => v.Name)
                .ToList();

            Dictionary<string, DataObject> returnValue = new Dictionary<string, DataObject>();

            foreach (var item in fieldAttributes)
            {
                returnValue.Add(item.Name.ToLowerCaseUnderLine(), BuildProperty(item));
            }

            return returnValue;
        }

        private static DataObject BuildProperty(FieldAttribute fieldAttribute)
        {
            DataObject dataObject = new DataObject();

            if (!string.IsNullOrEmpty(fieldAttribute.Type))
            {
                dataObject.AddDataValue("type", fieldAttribute.Type);
            }

            if (!fieldAttribute.Index)
            {
                dataObject.AddDataValue("index", false);
            }

            if (!fieldAttribute.DocValues)
            {
                dataObject.AddDataValue("doc_values", false);
            }

            switch (fieldAttribute.FieldType)
            {
                case FieldType.Integer:
                    {
                        IntegerFieldAttribute integerFieldAttribute = fieldAttribute as IntegerFieldAttribute;
                        if (integerFieldAttribute.NullValue.HasValue)
                        {
                            dataObject.AddDataValue("null_value", integerFieldAttribute.NullValue.Value);
                        }
                    }
                    break;
                case FieldType.Long:
                    {
                        LongFieldAttribute longFieldAttribute = fieldAttribute as LongFieldAttribute;
                        if (longFieldAttribute.NullValue.HasValue)
                        {
                            dataObject.AddDataValue("null_value", longFieldAttribute.NullValue.Value);
                        }
                    }
                    break;
                case FieldType.Keyword:
                    {
                        KeywordFieldAttribute keywordFieldAttribute = fieldAttribute as KeywordFieldAttribute;

                        dataObject.AddDataValue("ignore_above", (fieldAttribute as KeywordFieldAttribute).IgnoreAbove);

                        if (keywordFieldAttribute.NullValue != null)
                        {
                            dataObject.AddDataValue("null_value", keywordFieldAttribute.NullValue);
                        }
                    }
                    break;
                case FieldType.Text:
                    {
                        TextFieldAttribute textFieldAttribute = fieldAttribute as TextFieldAttribute;

                        if (!string.IsNullOrEmpty(textFieldAttribute.DefaultAnalyzer))
                        {
                            dataObject.AddDataValue("analyzer", textFieldAttribute.DefaultAnalyzer);
                        }

                        if (textFieldAttribute.NullValue != null)
                        {
                            dataObject.AddDataValue("null_value", textFieldAttribute.NullValue);
                        }

                        dataObject.AddDataObject("fields", BuildTextFields(textFieldAttribute, textFieldAttribute.KeywordIgnoreAbove));
                    }
                    break;
                default:
                    break;
            }

            return dataObject;
        }

        private static DataObject BuildTextFields(TextFieldAttribute textFieldAttribute, int keywordIgnoreAbove)
        {
            DataObject dataObject = new DataObject();

            //keyword
            {
                var keyword = dataObject.AddDataObject("keyword");
                keyword.AddDataValue("type", "keyword");
                keyword.AddDataValue("ignore_above", keywordIgnoreAbove);
            }

            //analyzer
            {
                //分词器
                List<string> analyzers = new List<string>();

                //内置分词器
                var builtInAnalyzers = textFieldAttribute.BuiltInAnalyzer.ToString().ToLower().Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries).OrderBy(x => x).ToList();
                analyzers.AddRange(builtInAnalyzers);

                //ik分词器
                var ikAnalyzers = textFieldAttribute.IKAnalyzer.ToString().ToLower().Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries).OrderBy(x => x).ToList();
                analyzers.AddRange(ikAnalyzers);

                //自定义分词器
                if (textFieldAttribute.CustomAnalyzer != null && textFieldAttribute.CustomAnalyzer.Length > 0)
                {
                    analyzers.AddRange(textFieldAttribute.CustomAnalyzer);
                }

                foreach (var analyzer in analyzers)
                {
                    if ("none".Equals(analyzer, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var mmm = dataObject.AddDataObject(analyzer);
                    mmm.AddDataValue("type", "text");
                    mmm.AddDataValue("analyzer", analyzer);


                    //builder.AppendLine(",").KeyValue(analyzer, analyzer_items);
                }
            }

            return dataObject;
        }

        private static FieldAttribute GetFieldAttribute(PropertyInfo field, string fieldName)
        {
            //处理简单类型
            FieldAttribute fieldAttribute = field.GetCustomAttribute<FieldAttribute>(false);
            if (fieldAttribute != null)
            {
                if (fieldAttribute.Name == null)
                {
                    fieldAttribute.Name = fieldName;
                }
                return fieldAttribute;
            }

            return PropertyTypeAsFieldAttribute(field.PropertyType, fieldName);
        }

        /// <summary>
        /// 根据c#类型，推断field类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private static FieldAttribute PropertyTypeAsFieldAttribute(Type type, string fieldName)
        {
            var fieldAttribute = BasicPropertyTypeAsFieldAttribute(type, fieldName);
            if (fieldAttribute != null)
            {
                return fieldAttribute;
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                if (genericType.FullName == "System.Collections.Generic.List`1")
                {
                    return BasicPropertyTypeAsFieldAttribute(type.GenericTypeArguments[0], fieldName);
                }
            }

            return null;
        }

        private static FieldAttribute BasicPropertyTypeAsFieldAttribute(Type type, string fieldName)
        {
            switch (type.ToString())
            {
                case "System.Int32":
                    return new IntegerFieldAttribute { Name = fieldName };
                case "System.Int64":
                    return new LongFieldAttribute { Name = fieldName };
                case "System.Double":
                    return new DoubleFieldAttribute { Name = fieldName };
                case "System.Boolean":
                    return new BooleanFieldAttribute { Name = fieldName };
                case "System.String":
                    return new KeywordFieldAttribute { Name = fieldName };
                default:
                    return null;
            }
        }
    }
}
