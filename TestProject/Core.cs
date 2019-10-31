using Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TestProject
{
    public class Core
    {
        private const string Mappings = "mappings";

        private const string Properties = "properties";

        MappingBuilder builder = new MappingBuilder();

        public string BuildMappings(Type type)
        {
            builder.LeftBracket();

            builder.KeyValueWithType(Mappings, BuildMappings, type);

            builder.RightBracket();

            return builder.Build();
        }

        private static void BuildMappings(MappingBuilder builder, Type type)
        {
            var typeAttribute = type.GetCustomAttribute<TypeAttribute>(false);
            if (typeAttribute == null)
            {
                return;
            }

            builder.KeyValueWithType(typeAttribute.Name, BuildDoc, type);
        }

        private static void BuildDoc(MappingBuilder builder, Type type)
        {
            var typeAttribute = type.GetCustomAttribute<TypeAttribute>(false);
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

            builder.KeyValueWithType(Properties, BuildProperties, type);
        }

        private static void BuildProperties(MappingBuilder builder, Type type)
        {
            //是否是遇到的第一个字段(如果不是，则需要在前面加逗号和换行)
            bool isFirstField = true;

            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var fieldAttribute = field.GetCustomAttribute<FieldAttribute>(false);
                if (fieldAttribute == null)
                {
                    continue;
                }

                //if (CoreConfig.UseStandardMode)
                //{
                //    switch (fieldAttribute.FieldType)
                //    {
                //        case FieldType.Keyword:
                //        case FieldType.Integer:
                //        case FieldType.Long:
                //            continue;
                //        default:
                //            break;
                //    }
                //}

                if (!isFirstField)
                {
                    builder.AppendLine(",");
                }
                builder.KeyValueWithType(field.Name.ToLowerCaseUnderLine(), (x, y) =>
                {
                    switch (fieldAttribute.FieldType)
                    {
                        case FieldType.Text:
                            BuildTextField(builder, fieldAttribute as TextFieldAttribute);
                            break;
                        case FieldType.Integer:
                            BuildIntegerField(builder);
                            break;
                        case FieldType.Long:
                            BuildLongField(builder);
                            break;
                        default:
                            break;
                    }

                }, field.PropertyType);

                isFirstField = false;
            }
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
            builder.KeyValue("keyword", v =>
            {
                v.KeyValue("type", "keyword").AppendLine(",")
                .KeyValue("ignore_above", 256);
            });
        }

        private static void BuildIntegerField(MappingBuilder builder)
        {
            builder.KeyValue("type", "integer");
        }

        private static void BuildLongField(MappingBuilder builder)
        {
            builder.KeyValue("type", "long");
        }
    }
}
