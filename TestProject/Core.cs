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
        MappingBuilder builder = new MappingBuilder();

        public string BuildMappings(Type type)
        {
            var indexAttribute = type.GetCustomAttribute<IndexAttribute>(false);
            if (indexAttribute == null)
            {
                return $"IndexAttribute is required on class {type.FullName}";
            }

            Dictionary<string, Action<MappingBuilder, Type>> items = new Dictionary<string, Action<MappingBuilder, Type>>();
            if (indexAttribute.NumberOfReplicas > 0 || indexAttribute.NumberOfShards > 0)
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
            if (indexAttribute == null)
            {
                return;
            }

            Dictionary<string, object> items = new Dictionary<string, object>();
            if (indexAttribute.NumberOfShards > 0)
            {
                items.Add("number_of_shards", indexAttribute.NumberOfShards);
            }
            if (indexAttribute.NumberOfReplicas > 0)
            {
                items.Add("number_of_replicas", indexAttribute.NumberOfReplicas);
            }

            builder.KeyValue(items);
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
            switch (fieldAttribute.FieldType)
            {
                case FieldType.Integer:
                    BuildIntegerField(builder);
                    break;
                case FieldType.Long:
                    BuildLongField(builder);
                    break;
                case FieldType.Keyword:
                    BuildKeywordField(builder);
                    break;
                case FieldType.Text:
                    BuildTextField(builder, fieldAttribute as TextFieldAttribute);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 根据c#类型，推断field类型
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private static FieldAttribute GetFieldAttribute(PropertyInfo field, string fieldName)
        {
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

        private static void BuildIntegerField(MappingBuilder builder)
        {
            builder.KeyValue("type", "integer");
        }

        private static void BuildLongField(MappingBuilder builder)
        {
            builder.KeyValue("type", "long");
        }

        private static void BuildKeywordField(MappingBuilder builder)
        {
            builder.KeyValue("type", "keyword");
        }

        private static void BuildTextField(MappingBuilder builder, TextFieldAttribute textFieldAttribute)
        {
            builder.KeyValue("type", "text");

            //if (CoreConfig.EvenWithoutFiledAttribute)
            {
                builder.AppendLine(",").KeyValue("fields", BuildKeyword);
            }
        }

        private static void BuildKeyword(MappingBuilder builder)
        {
            Dictionary<string, object> items = new Dictionary<string, object>();
            items.Add("type", "keyword");
            items.Add("ignore_above", 256);

            builder.KeyValue("keyword", items);
        }
    }
}
