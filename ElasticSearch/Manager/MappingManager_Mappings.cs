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
    partial class MappingManager
    {
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
                    _doc.AddDataValue(DataKey.DoubleQuotationString("dynamic"), "strict");
                    break;
                case Dynamic.True:
                default:
                    break;
            }

            var propertyMap = BuildProperties(type);
            if (propertyMap != null && propertyMap.Count > 0)
            {
                var _properties = _doc.AddDataObject(DataKey.DoubleQuotationString("properties"));
                foreach (var item in propertyMap)
                {
                    _properties.AddDataObject(item.Key, item.Value);
                }
            }

            return dataObject;
        }

        private static Dictionary<DataKey, DataObject> BuildProperties(Type type)
        {
            var fieldAttributes = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(field => GetFieldAttribute(field, field.Name.ToLowerCaseUnderLine()))
                .Where(fieldAttribute => fieldAttribute != null)
                .OrderBy(v => v.Name)
                .ToList();

            Dictionary<DataKey, DataObject> returnValue = new Dictionary<DataKey, DataObject>();

            foreach (var item in fieldAttributes)
            {
                returnValue.Add(DataKey.DoubleQuotationString(item.Name.ToLowerCaseUnderLine()), BuildProperty(item));
            }

            return returnValue;
        }

        private static DataObject BuildProperty(FieldAttribute fieldAttribute)
        {
            DataObject dataObject = new DataObject();

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

            return dataObject;
        }

        private static SortedDataObject BuildTextFields(TextFieldAttribute textFieldAttribute, int keywordIgnoreAbove)
        {
            SortedDataObject sortedDataObject = new SortedDataObject();

            //keyword
            {
                var keyword = sortedDataObject.AddDataObject(DataKey.DoubleQuotationString("keyword"));
                keyword.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("keyword"));
                keyword.AddDataValue(DataKey.DoubleQuotationString("ignore_above"), keywordIgnoreAbove);
            }

            //analyzer
            {
                //分词器
                List<string> analyzers = new List<string>();

                //内置分词器
                var builtInAnalyzers = textFieldAttribute
                    .BuiltInAnalyzer
                    .ToString()
                    .ToLower()
                    .Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries)
                    .OrderBy(x => x)
                    .ToList();
                analyzers.AddRange(builtInAnalyzers);

                //ik分词器
                var ikAnalyzers = textFieldAttribute.IKAnalyzer
                    .ToString()
                    .ToLower()
                    .Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries)
                    .OrderBy(x => x)
                    .ToList();
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

                    var mmm = sortedDataObject.AddDataObject(DataKey.DoubleQuotationString(analyzer));
                    mmm.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString("text"));
                    mmm.AddDataValue(DataKey.DoubleQuotationString("analyzer"), DataValue.DoubleQuotationString(analyzer));
                }
            }

            return sortedDataObject;
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
