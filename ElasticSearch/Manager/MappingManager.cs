using Infrastructure;
using Savory;
using Savory.CodeDom;
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
            var customFilterAttributeList = type.GetCustomAttributes<CustomFilterAttribute>(false).ToList();
            var customTokenizerAttributeList = type.GetCustomAttributes<CustomTokenizerAttribute>(false).ToList();

            var dataObject = new DataObject();

            //alias
            if (indexAttribute.Aliases != null && indexAttribute.Aliases.Length > 0)
            {
                var aliasDataObject = dataObject.AddDataObject(DataKey.DoubleQuotationString("aliases"));
                foreach (var alias in indexAttribute.Aliases)
                {
                    aliasDataObject.AddDataObject(DataKey.DoubleQuotationString(alias));
                }
            }

            //settings
            var settingsDataObject = BuildSettings(indexAttribute, customTokenizerAttributeList, customFilterAttributeList, customAnalyzerAttributeList);
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

        private static DataObject BuildSettings(IndexAttribute indexAttribute,
            List<CustomTokenizerAttribute> customTokenizerAttributeList,
            List<CustomFilterAttribute> customFilterAttributeList,
            List<CustomAnalyzerAttribute> customAnalyzerAttributeList)
        {
            if (indexAttribute.NumberOfReplicas == 0
                && indexAttribute.NumberOfShards == 0
                && (customAnalyzerAttributeList == null || customAnalyzerAttributeList.Count == 0)
                && (customFilterAttributeList == null || customFilterAttributeList.Count == 0)
                && (customTokenizerAttributeList == null || customTokenizerAttributeList.Count == 0))
            {
                return null;
            }

            var returnSettingsDataObject = false;
            var settingsDataObject = new DataObject();
            if (indexAttribute.NumberOfShards > 0)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("number_of_shards"), indexAttribute.NumberOfShards);
            }
            if (indexAttribute.NumberOfReplicas > 0)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("number_of_replicas"), indexAttribute.NumberOfReplicas);
            }
            if (indexAttribute.MappingTotalFieldsLimit > 0)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataValue(DataKey.DoubleQuotationString("mapping.total_fields.limit"), indexAttribute.MappingTotalFieldsLimit);
            }

            var withAnalysisDataObject = false;
            var analysisDataObject = new DataObject();

            if (customTokenizerAttributeList != null && customTokenizerAttributeList.Count > 0)
            {
                withAnalysisDataObject = true;
                var tokenizerDataObject = analysisDataObject.AddDataObject(DataKey.DoubleQuotationString("tokenizer"));
                foreach (var customTokenizerAttribute in customTokenizerAttributeList)
                {
                    var tokenizerBody = BuildTokenizerBody(customTokenizerAttribute);
                    tokenizerDataObject.AddDataObject(customTokenizerAttribute.Name, tokenizerBody);
                }
            }

            if (customFilterAttributeList != null && customFilterAttributeList.Count > 0)
            {
                withAnalysisDataObject = true;
                var analyzerDataObject = analysisDataObject.AddDataObject(DataKey.DoubleQuotationString("filter"));
                foreach (var customFilterAttribute in customFilterAttributeList)
                {
                    var filterProperties = BuildFilterProperties(customFilterAttribute);
                    analyzerDataObject.AddDataObject(DataKey.DoubleQuotationString(customFilterAttribute.Name), filterProperties);
                }
            }

            if (customAnalyzerAttributeList != null && customAnalyzerAttributeList.Count > 0)
            {
                withAnalysisDataObject = true;
                var analyzerDataObject = analysisDataObject.AddDataObject(DataKey.DoubleQuotationString("analyzer"));
                foreach (var customAnalyzerAttribute in customAnalyzerAttributeList)
                {
                    var analyzerProperties = BuildAnalyzerProperties(customAnalyzerAttribute);
                    analyzerDataObject.AddDataObject(DataKey.DoubleQuotationString(customAnalyzerAttribute.Name), analyzerProperties);
                }
            }

            if (withAnalysisDataObject)
            {
                returnSettingsDataObject = true;
                settingsDataObject.AddDataObject(DataKey.DoubleQuotationString("analysis"), analysisDataObject);
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
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), abstractNGramTokenizerAttribute.Type);

            if (abstractNGramTokenizerAttribute.MinGram > 0)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("min_gram"), abstractNGramTokenizerAttribute.MinGram);
            }

            if (abstractNGramTokenizerAttribute.MaxGram > 0)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("max_gram"), abstractNGramTokenizerAttribute.MaxGram);
            }

            var tokenChars = abstractNGramTokenizerAttribute.TokenChars.ToString()
                .ToLower()
                .Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries)
                .Where(v => !"none".Equals(v))
                .ToList();

            if (tokenChars.Count > 0)
            {
                var tokenCharsArray = dataObject.AddDataArray(DataKey.DoubleQuotationString("token_chars"));
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
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), patternTokenizerAttribute.Type);

            if (!string.IsNullOrEmpty(patternTokenizerAttribute.Pattern))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("pattern"), patternTokenizerAttribute.Pattern);
            }

            return dataObject;
        }

        private static DataObject BuildCharGroupTokenizer(CharGroupTokenizerAttribute charGroupTokenizerAttribute)
        {
            DataObject dataObject = new DataObject();
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), charGroupTokenizerAttribute.Type);

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

            var array = dataObject.AddDataArray(DataKey.DoubleQuotationString("tokenize_on_chars"));
            foreach (var item in tokenizeOChars)
            {
                array.AddDataValue(item);
            }

            return dataObject;
        }

        private static DataObject BuildFilterProperties(CustomFilterAttribute customFilterAttribute)
        {
            DataObject dataObject = new DataObject();

            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(customFilterAttribute.Type));

            if (customFilterAttribute.Properties != null && customFilterAttribute.Properties.Length > 0 && customFilterAttribute.Properties.Length % 2 == 0)
            {
                for (int i = 0; i < customFilterAttribute.Properties.Length; i += 2)
                {
                    dataObject.AddDataValue(DataKey.DoubleQuotationString(customFilterAttribute.Properties[i]), DataValue.DoubleQuotationString(customFilterAttribute.Properties[i + 1]));
                }
            }

            return dataObject;
        }

        private static DataObject BuildAnalyzerProperties(CustomAnalyzerAttribute customAnalyzerAttribute)
        {
            DataObject dataObject = new DataObject();

            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("custom"));
            dataObject.AddDataValue(DataKey.DoubleQuotationString("tokenizer"), DataValue.DoubleQuotationString(customAnalyzerAttribute.Tokenizer));

            {
                List<string> tokenFilters = new List<string>();

                var builtInTokenFilters = customAnalyzerAttribute.BuiltInTokenFilters.ToString()
                    .ToLower()
                    .Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries)
                    .Where(v => !"none".Equals(v))
                    .ToList();
                tokenFilters.AddRange(builtInTokenFilters);

                if (customAnalyzerAttribute.CustomTokenFilters != null && customAnalyzerAttribute.CustomTokenFilters.Length > 0)
                {
                    tokenFilters.AddRange(customAnalyzerAttribute.CustomTokenFilters);
                }

                if (tokenFilters.Count > 0)
                {
                    var dataArray = dataObject.AddDataArray(DataKey.DoubleQuotationString("filter"));
                    foreach (var item in tokenFilters)
                    {
                        dataArray.AddDataValue(DataValue.DoubleQuotationString(item));
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
            var _doc = dataObject.AddDataObject(DataKey.DoubleQuotationString(indexAttribute.TypeName ?? "_doc"));

            switch (indexAttribute.Dynamic)
            {
                case Dynamic.False:
                    _doc.AddDataValue(DataKey.DoubleQuotationString("dynamic"), false);
                    break;
                case Dynamic.Strict:
                    _doc.AddDataValue(DataKey.DoubleQuotationString("dynamic"), DataValue.DoubleQuotationString("strict"));
                    break;
                case Dynamic.True:
                default:
                    break;
            }

            var _properties = _doc.AddSortedDataObject(DataKey.DoubleQuotationString("properties"));
            BuildProperties(_properties, type);

            return dataObject;
        }

        private static void BuildProperties(SortedDataObject properties, Type type)
        {
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                var key = propertyInfo.Name.ToLowerCaseUnderLine();
                var field = GetFieldAttribute(propertyInfo, propertyInfo.Name.ToLowerCaseUnderLine());
                if (field != null)
                {
                    key = field.Name;
                }

                var dataObject = properties.AddDataObject(DataKey.DoubleQuotationString(key));

                if (propertyInfo.PropertyType.IsGenericType)
                {
                    //如果不是 List<T>，则跳过
                    var genericType = propertyInfo.PropertyType.GetGenericTypeDefinition();
                    if (genericType.FullName != "System.Collections.Generic.List`1")
                    {
                        continue;
                    }

                    if (field != null)
                    {
                        if (field is NestedFieldAttribute)
                        {
                            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("nested"));
                            var _properties = dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("properties"));
                            BuildProperties(_properties, propertyInfo.PropertyType.GenericTypeArguments[0]);
                        }
                        else
                        {
                            BuildProperty(dataObject, field);
                        }
                    }
                    else
                    {
                        var _properties = dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("properties"));
                        BuildProperties(_properties, propertyInfo.PropertyType.GenericTypeArguments[0]);
                    }
                }
                else
                {

                    if (field is NestedFieldAttribute)
                    {

                    }
                    else
                    {
                        BuildProperty(dataObject, field);
                    }


                    //var field = GetFieldAttribute(property, property.Name.ToLowerCaseUnderLine());
                    //if (field != null)
                    //{
                    //    returnValue.AddDataObject(DataKey.DoubleQuotationString(field.Name.ToLowerCaseUnderLine()), );
                    //}

                    //DataObject propertyDataObject = new DataObject();
                    //returnValue.AddDataObject(property.Name.ToLowerCaseUnderLine(), propertyDataObject);

                    //DataObject propertiesDataObject = propertyDataObject.AddDataObject("properties");
                    //if (field != null && field is NestedFieldAttribute)
                    //{
                    //    propertiesDataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("nested"));
                    //}
                    //foreach (var item in nestProperties.DataItemMap)
                    //{
                    //    propertiesDataObject.Add(item.Key, item.Value);
                    //}
                }
            }
        }

        private static void BuildProperty(DataObject dataObject, FieldAttribute fieldAttribute)
        {
            if (dataObject == null || fieldAttribute == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(fieldAttribute.Type))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(fieldAttribute.Type));
            }

            if (!fieldAttribute.Index)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("index"), false);
            }

            if (!fieldAttribute.DocValues)
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("doc_values"), false);
            }

            switch (fieldAttribute.FieldType)
            {
                case FieldType.Integer:
                    {
                        IntegerFieldAttribute integerFieldAttribute = fieldAttribute as IntegerFieldAttribute;
                        if (integerFieldAttribute.NullValue.HasValue)
                        {
                            dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), integerFieldAttribute.NullValue.Value);
                        }
                    }
                    break;
                case FieldType.Long:
                    {
                        LongFieldAttribute longFieldAttribute = fieldAttribute as LongFieldAttribute;
                        if (longFieldAttribute.NullValue.HasValue)
                        {
                            dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), longFieldAttribute.NullValue.Value);
                        }
                    }
                    break;
                case FieldType.Keyword:
                    {
                        KeywordFieldAttribute keywordFieldAttribute = fieldAttribute as KeywordFieldAttribute;

                        dataObject.AddDataValue(DataKey.DoubleQuotationString("ignore_above"), (fieldAttribute as KeywordFieldAttribute).IgnoreAbove);

                        if (keywordFieldAttribute.NullValue != null)
                        {
                            dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), keywordFieldAttribute.NullValue);
                        }
                    }
                    break;
                case FieldType.Text:
                    {
                        TextFieldAttribute textFieldAttribute = fieldAttribute as TextFieldAttribute;

                        if (!string.IsNullOrEmpty(textFieldAttribute.DefaultAnalyzer))
                        {
                            dataObject.AddDataValue(DataKey.DoubleQuotationString("analyzer"), textFieldAttribute.DefaultAnalyzer);
                        }

                        if (textFieldAttribute.NullValue != null)
                        {
                            dataObject.AddDataValue(DataKey.DoubleQuotationString("null_value"), textFieldAttribute.NullValue);
                        }

                        dataObject.AddSortedDataObject(DataKey.DoubleQuotationString("fields"), BuildTextFields(textFieldAttribute, textFieldAttribute.KeywordIgnoreAbove));
                    }
                    break;
                default:
                    break;
            }
        }

        private static SortedDataObject BuildTextFields(TextFieldAttribute textFieldAttribute, int keywordIgnoreAbove)
        {
            SortedDataObject dataObject = new SortedDataObject();

            //keyword
            {
                var keyword = dataObject.AddDataObject(DataKey.DoubleQuotationString("keyword"));
                keyword.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("keyword"));
                keyword.AddDataValue(DataKey.DoubleQuotationString("ignore_above"), keywordIgnoreAbove);
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
                    mmm.AddDataValue(DataKey.DoubleQuotationString("type"), "text");
                    mmm.AddDataValue(DataKey.DoubleQuotationString("analyzer"), analyzer);
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
                    return new TextFieldAttribute { Name = fieldName };
                default:
                    return null;
            }
        }
    }
}
