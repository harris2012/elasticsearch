using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch
{
    public static class TypeExtension
    {
        public static string ToJavaType(this Type type, string rootNamespace)
        {
            if (type.IsGenericType)
            {
                var genericTypeName = type.Name.Split(new string[] { "`" }, StringSplitOptions.RemoveEmptyEntries)[0];
                switch (genericTypeName)
                {
                    case "List":
                        genericTypeName = "java.util.List";
                        break;
                    case "Dictionary":
                        genericTypeName = "java.util.Map";
                        break;
                    default:
                        break;
                }

                return string.Format("{0}<{1}>",
                    genericTypeName,
                    string.Join(", ", type.GetGenericArguments().Select(v => ToJavaType(v, rootNamespace))));
            }

            string typeName = type.FullName;

            switch (type.FullName)
            {
                case "System.String":
                    typeName = "String";
                    break;
                case "System.Int32":
                    typeName = "Integer";
                    break;
                case "System.Boolean":
                    typeName = "Boolean";
                    break;
                case "System.Void":
                    typeName = "void";
                    break;
                case "System.Int16":
                    typeName = "Short";
                    break;
                case "System.Byte":
                    typeName = "Byte";
                    break;
                case "System.Int64":
                    typeName = "Long";
                    break;
                case "System.DateTime":
                    typeName = "java.util.Date";
                    break;
                case "System.Single":
                    typeName = "Float";
                    break;
                case "System.Double":
                    typeName = "Double";
                    break;
                case "System.Decimal":
                    typeName = "java.math.BigDecimal";
                    break;

                default:
                    if (typeName.StartsWith(rootNamespace))
                    {
                        typeName = typeName.Substring(rootNamespace.Length + 1);
                    }
                    else
                    {
                        typeName = type.FullName;
                    }
                    break;
            }

            return typeName;
        }
    }
}
