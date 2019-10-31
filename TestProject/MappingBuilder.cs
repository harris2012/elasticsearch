using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject
{
    public class MappingBuilder
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        private Stack<string> indents = new Stack<string>();

        private string indent = "    ";

        private string GetIndent()
        {
            return string.Join(string.Empty, indents);
        }

        public string Build()
        {
            return stringBuilder.ToString();
        }

        private MappingBuilder PushIndent()
        {
            indents.Push(indent);

            return this;
        }

        private MappingBuilder PopIndent()
        {
            indents.Pop();

            return this;
        }

        public MappingBuilder Append(string text)
        {
            stringBuilder.Append(text);

            return this;
        }

        public MappingBuilder AppendLine(string text)
        {
            stringBuilder.AppendLine(text);

            return this;
        }

        public MappingBuilder KeyValue(string key, object value)
        {
            switch (value.GetType().FullName)
            {
                case "System.Int32":
                case "System.Int64":
                    stringBuilder.Append(GetIndent()).Append("\"").Append(key).Append("\": ").Append(value);
                    break;
                case "System.Boolean":
                    stringBuilder.Append(GetIndent()).Append("\"").Append(key).Append("\": ").Append((bool)value ? "true" : "false");
                    break;
                case "System.String":
                    stringBuilder.Append(GetIndent()).Append("\"").Append(key).Append("\": \"").Append(value).Append("\"");
                    break;
                default:
                    break;
            }

            return this;
        }

        public MappingBuilder KeyValue(string key, Action<MappingBuilder> value)
        {
            stringBuilder.Append(GetIndent()).Append("\"").Append(key).AppendLine("\": {");
            PushIndent();

            value(this);

            PopIndent();
            stringBuilder.AppendLine().Append(GetIndent()).Append("}");

            return this;
        }

        public MappingBuilder KeyValue(string key, Dictionary<string, object> items)
        {
            stringBuilder.Append(GetIndent()).Append("\"").Append(key).AppendLine("\": {");
            PushIndent();

            KeyValue(items);

            PopIndent();
            stringBuilder.AppendLine().Append(GetIndent()).Append("}");

            return this;
        }

        public void KeyValue(Dictionary<string, object> items)
        {
            bool first = true;
            foreach (var item in items)
            {
                if (!first)
                {
                    stringBuilder.AppendLine(",");
                }

                this.KeyValue(item.Key, item.Value);
                first = false;
            }
        }

        public MappingBuilder KeyValue(string key, Type type, Action<MappingBuilder, Type> value)
        {
            stringBuilder.Append(GetIndent()).Append("\"").Append(key).AppendLine("\": {");
            PushIndent();

            value(this, type);

            PopIndent();
            stringBuilder.AppendLine().Append(GetIndent()).Append("}");

            return this;
        }

        public MappingBuilder KeyValue(string key, FieldAttribute fieldAttribute, Action<MappingBuilder, FieldAttribute> value)
        {
            stringBuilder.Append(GetIndent()).Append("\"").Append(key).AppendLine("\": {");
            PushIndent();

            value(this, fieldAttribute);

            PopIndent();
            stringBuilder.AppendLine().Append(GetIndent()).Append("}");

            return this;
        }

        public MappingBuilder LeftBracket()
        {
            stringBuilder.AppendLine("{");
            this.PushIndent();

            return this;
        }

        public MappingBuilder RightBracket()
        {
            stringBuilder.AppendLine().AppendLine("}");
            this.PopIndent();

            return this;
        }
    }
}
