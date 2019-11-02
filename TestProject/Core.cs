using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TestProject
{
    public class Core
    {
        /// <summary>
        /// 用于拆分枚举
        /// </summary>
        private static readonly string[] CommaAndWhitespace = new string[] { " ", "," };

        MappingBuilder builder = new MappingBuilder();

        public string BuildMappings(Type type)
        {
            var indexAttribute = type.GetCustomAttribute<IndexAttribute>(false);
            if (indexAttribute == null)
            {
                return $"IndexAttribute is required on class {type.FullName}";
            }

            var customAnalyzerAttributeList = type.GetCustomAttributes<CustomAnalyzerAttribute>(false).ToList();

            Dictionary<string, Action<MappingBuilder, Type>> items = new Dictionary<string, Action<MappingBuilder, Type>>();
            if (indexAttribute.NumberOfReplicas > 0 || indexAttribute.NumberOfShards > 0 || (customAnalyzerAttributeList != null && customAnalyzerAttributeList.Count > 0))
            {
                items.Add("settings", BuildSettings);
            }
            items.Add("mappings", BuildMappings);

            builder.LeftBracket();

            bool first = true;
            foreach (var item in items)
            {
                if (!first)
                {
                    builder.AppendLine(",");
                }

                builder.KeyValue(item.Key, type, item.Value);
                first = false;
            }

            builder.RightBracket();

            return builder.Build();
        }

        private static void BuildSettings(MappingBuilder builder, Type type)
        {
            var indexAttribute = type.GetCustomAttribute<IndexAttribute>(false);

            Dictionary<string, object> items = new Dictionary<string, object>();
            AddNotNegative(items, "number_of_shards", indexAttribute.NumberOfShards);
            AddNotNegative(items, "number_of_replicas", indexAttribute.NumberOfReplicas);
            AddNotNegative(items, "mapping.total_fields.limit", indexAttribute.MappingTotalFieldsLimit);

            builder.KeyValue(items);

            var customAnalyzerAttributeList = type.GetCustomAttributes<CustomAnalyzerAttribute>(false).ToList();
            var customTokenizerAttributeList = type.GetCustomAttributes<CustomTokenizerAttribute>(false).ToList();

            //为后面的`analyzers`做准备
            if (items.Count > 0 && (customAnalyzerAttributeList != null && customAnalyzerAttributeList.Count > 0 || customTokenizerAttributeList != null && customTokenizerAttributeList.Count > 0))
            {
                builder.AppendLine(",");
            }

            builder.KeyValue("analysis", v =>
            {
                BuildTokenizers(v, customTokenizerAttributeList);

                if (customTokenizerAttributeList != null && customTokenizerAttributeList.Count > 0 && customAnalyzerAttributeList != null && customAnalyzerAttributeList.Count > 0)
                {
                    builder.AppendLine(",");
                }

                BuildAnalyzers(v, customAnalyzerAttributeList);
            });
        }

        private static void BuildTokenizers(MappingBuilder builder, List<CustomTokenizerAttribute> customTokenizerAttributeList)
        {
            if (customTokenizerAttributeList == null || customTokenizerAttributeList.Count == 0)
            {
                return;
            }
            builder.KeyValue("tokenizer", value =>
            {
                for (int i = 0; i < customTokenizerAttributeList.Count; i++)
                {
                    if (i > 0)
                    {
                        builder.AppendLine(",");
                    }
                    BuildTokenizer(builder, customTokenizerAttributeList[i]);
                }
            });
        }

        private static void BuildTokenizer(MappingBuilder builder, CustomTokenizerAttribute customTokenizerAttribute)
        {
            builder.KeyValue(customTokenizerAttribute.Name, v =>
            {
                v.KeyValue("type", customTokenizerAttribute.Type);

                if (customTokenizerAttribute is AbstractNGramTokenizerAttribute)
                {
                    BuildNGramTokenizer(builder, customTokenizerAttribute as AbstractNGramTokenizerAttribute);
                }

                if (customTokenizerAttribute is PatternTokenizerAttribute)
                {
                    BuildPatternTokenier(builder, customTokenizerAttribute as PatternTokenizerAttribute);
                }
            });
        }

        private static void BuildNGramTokenizer(MappingBuilder builder, AbstractNGramTokenizerAttribute abstractNGramTokenizerAttribute)
        {
            if (abstractNGramTokenizerAttribute.MinGram > 0)
            {
                builder.AppendLine(",").KeyValue("min_gram", abstractNGramTokenizerAttribute.MinGram);
            }

            if (abstractNGramTokenizerAttribute.MaxGram > 0)
            {
                builder.AppendLine(",").KeyValue("max_gram", abstractNGramTokenizerAttribute.MaxGram);
            }

            var tokenChars = abstractNGramTokenizerAttribute.TokenChars.ToString()
                .ToLower()
                .Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries)
                .Where(v => !"none".Equals(v))
                .ToList();
            if (tokenChars.Count > 0)
            {
                builder.AppendLine(",").KeyValue("token_chars", tokenChars);
            }
        }

        private static void BuildPatternTokenier(MappingBuilder builder, PatternTokenizerAttribute patternTokenizerAttribute)
        {
            if (!string.IsNullOrEmpty(patternTokenizerAttribute.Pattern))
            {
                builder.AppendLine(",").KeyValue("pattern", patternTokenizerAttribute.Pattern);
            }
        }

        private static void BuildAnalyzers(MappingBuilder builder, List<CustomAnalyzerAttribute> customAnalyzerAttributeList)
        {
            if (customAnalyzerAttributeList == null || customAnalyzerAttributeList.Count == 0)
            {
                return;
            }
            builder.KeyValue("analyzer", value =>
            {
                for (int i = 0; i < customAnalyzerAttributeList.Count; i++)
                {
                    if (i > 0)
                    {
                        builder.AppendLine(",");
                    }
                    BuildAnalyzer(builder, customAnalyzerAttributeList[i]);
                }
            });
        }

        private static void BuildAnalyzer(MappingBuilder builder, CustomAnalyzerAttribute customAnalyzerAttribute)
        {
            builder.KeyValue(customAnalyzerAttribute.Name, v =>
            {
                BuildAnalyzerProperties(v, customAnalyzerAttribute);
            });
        }

        private static void BuildAnalyzerProperties(MappingBuilder builder, CustomAnalyzerAttribute customAnalyzerAttribute)
        {
            builder.KeyValue("tokenizer", customAnalyzerAttribute.Tokenizer);

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
                    builder.AppendLine(",");
                    builder.KeyValue("filter", tokenFilters);
                }
            }
        }

        private static void AddNotNegative(Dictionary<string, object> items, string key, int value)
        {
            if (value >= 0)
            {
                items.Add(key, value);
            }
        }

        private static void BuildMappings(MappingBuilder builder, Type type)
        {
            var indexAttribute = type.GetCustomAttribute<IndexAttribute>(false);
            if (indexAttribute == null)
            {
                return;
            }

            builder.KeyValue(indexAttribute.TypeName ?? "_doc", type, BuildDoc);
        }

        private static void BuildDoc(MappingBuilder builder, Type type)
        {
            var typeAttribute = type.GetCustomAttribute<IndexAttribute>(false);
            if (typeAttribute == null)
            {
                return;
            }

            switch (typeAttribute.Dynamic)
            {
                case Dynamic.False:
                    builder.KeyValue("dynamic", false).AppendLine(",");
                    break;
                case Dynamic.Strict:
                    builder.KeyValue("dynamic", "strict").AppendLine(",");
                    break;
                case Dynamic.True:
                default:
                    break;
            }

            builder.KeyValue("properties", type, BuildProperties);
        }

        private static void BuildProperties(MappingBuilder builder, Type type)
        {
            var fieldAttributes = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(field => GetFieldAttribute(field, field.Name.ToLowerCaseUnderLine()))
                .Where(fieldAttribute => fieldAttribute != null)
                .OrderBy(v => v.Name)
                .ToList();

            for (int i = 0; i < fieldAttributes.Count; i++)
            {
                if (i > 0)
                {
                    builder.AppendLine(",");
                }
                builder.KeyValue(fieldAttributes[i].Name.ToLowerCaseUnderLine(), fieldAttributes[i], BuildProperty);
            }
        }

        private static void BuildProperty(MappingBuilder builder, FieldAttribute fieldAttribute)
        {
            if (!string.IsNullOrEmpty(fieldAttribute.Type))
            {
                builder.KeyValue("type", fieldAttribute.Type);
            }

            if (!fieldAttribute.Index)
            {
                builder.AppendLine(",").KeyValue("index", false);
            }

            switch (fieldAttribute.FieldType)
            {
                case FieldType.Integer:
                    {
                        IntegerFieldAttribute integerFieldAttribute = fieldAttribute as IntegerFieldAttribute;
                        if (integerFieldAttribute.NullValue.HasValue)
                        {
                            builder.AppendLine(",").KeyValue("null_value", integerFieldAttribute.NullValue.Value);
                        }
                    }
                    break;
                case FieldType.Long:
                    {
                        LongFieldAttribute longFieldAttribute = fieldAttribute as LongFieldAttribute;
                        if (longFieldAttribute.NullValue.HasValue)
                        {
                            builder.AppendLine(",").KeyValue("null_value", longFieldAttribute.NullValue.Value);
                        }
                    }
                    break;
                case FieldType.Keyword:
                    {
                        KeywordFieldAttribute keywordFieldAttribute = fieldAttribute as KeywordFieldAttribute;

                        builder.AppendLine(",").KeyValue("ignore_above", (fieldAttribute as KeywordFieldAttribute).IgnoreAbove);

                        if (keywordFieldAttribute.NullValue != null)
                        {
                            builder.AppendLine(",").KeyValue("null_value", keywordFieldAttribute.NullValue);
                        }
                    }
                    break;
                case FieldType.Text:
                    {
                        TextFieldAttribute textFieldAttribute = fieldAttribute as TextFieldAttribute;

                        if (!string.IsNullOrEmpty(textFieldAttribute.DefaultAnalyzer))
                        {
                            builder.AppendLine(",").KeyValue("analyzer", textFieldAttribute.DefaultAnalyzer);
                        }

                        if (textFieldAttribute.NullValue != null)
                        {
                            builder.AppendLine(",").KeyValue("null_value", textFieldAttribute.NullValue);
                        }

                        BuildTextField_Fields(builder, textFieldAttribute);
                    }
                    break;
                default:
                    break;
            }
        }

        private static void BuildTextField_Fields(MappingBuilder builder, TextFieldAttribute textFieldAttribute)
        {
            builder.AppendLine(",").KeyValue("fields", v =>
            {
                BuildTextFields(builder, textFieldAttribute, textFieldAttribute.KeywordIgnoreAbove);
            });
        }

        private static void BuildTextFields(MappingBuilder builder, TextFieldAttribute textFieldAttribute, int keywordIgnoreAbove)
        {
            //keyword
            {
                Dictionary<string, object> items = new Dictionary<string, object>();
                items.Add("type", "keyword");
                items.Add("ignore_above", keywordIgnoreAbove);

                builder.KeyValue("keyword", items);
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
                    Dictionary<string, object> analyzer_items = new Dictionary<string, object>();
                    analyzer_items.Add("type", "text");
                    analyzer_items.Add("analyzer", analyzer);

                    builder.AppendLine(",").KeyValue(analyzer, analyzer_items);
                }
            }
        }

        private static FieldAttribute GetFieldAttribute(PropertyInfo field, string fieldName)
        {
            //处理复杂类型
            //ComplexFieldAttribute complexFieldAttribute = field.GetCustomAttribute<ComplexFieldAttribute>(false);
            //if (complexFieldAttribute != null)
            //{
            //    if (complexFieldAttribute.Name == null)
            //    {
            //        complexFieldAttribute.Name = fieldName;
            //    }
            //    complexFieldAttribute.ComplexType = field.PropertyType;
            //    return complexFieldAttribute;
            //}

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

            if (!CoreConfig.PropertyTypeAsFiledAttribute)
            {
                return null;
            }

            return PropertyTypeAsFieldAttribute(field.PropertyType, fieldName);
        }

        /// <summary>
        /// 根据c#类型，推断field类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static FieldAttribute PropertyTypeAsFieldAttribute(Type type, string fieldName)
        {
            switch (type.ToString())
            {
                case "System.Int32":
                    return new IntegerFieldAttribute { Name = fieldName };
                case "System.Int64":
                    return new LongFieldAttribute { Name = fieldName };
                case "System.String":
                    return new KeywordFieldAttribute { Name = fieldName };
                default:
                    break;
            }

            return null;
        }
    }
}
