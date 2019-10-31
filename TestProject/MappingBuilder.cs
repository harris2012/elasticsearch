using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject
{
    public class MappingBuilder
    {
        private readonly StringBuilder builder = new StringBuilder();

        private Stack<string> indents = new Stack<string>();

        private string indent = "    ";

        private string GetIndent()
        {
            return string.Join(string.Empty, indents);
        }

        public string Build()
        {
            return builder.ToString();
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
            builder.Append(text);

            return this;
        }

        public MappingBuilder AppendLine(string text)
        {
            builder.AppendLine(text);

            return this;
        }

        public MappingBuilder KeyValue(string key, string value)
        {
            builder.Append(GetIndent()).Append("\"").Append(key).Append("\": \"").Append(value).Append("\"");

            return this;
        }

        public MappingBuilder KeyValue(string key, int value)
        {
            builder.Append(GetIndent()).Append("\"").Append(key).Append("\": ").Append(value);

            return this;
        }

        public MappingBuilder KeyValue(string key, bool value)
        {
            builder.Append(GetIndent()).Append("\"").Append(key).Append("\": ").Append(value ? "true" : "false");

            return this;
        }

        public MappingBuilder KeyValue(string key, Action<MappingBuilder> value)
        {
            builder.Append(GetIndent()).Append("\"").Append(key).AppendLine("\": {");
            PushIndent();

            value(this);

            PopIndent();
            builder.AppendLine().Append(GetIndent()).Append("}");

            return this;
        }

        public MappingBuilder KeyValueWithType(string key, Action<MappingBuilder, Type> value, Type type)
        {
            builder.Append(GetIndent()).Append("\"").Append(key).AppendLine("\": {");
            PushIndent();

            value(this, type);

            PopIndent();
            builder.AppendLine().Append(GetIndent()).Append("}");

            return this;
        }

        public MappingBuilder LeftBracket()
        {
            builder.AppendLine("{");
            this.PushIndent();

            return this;
        }

        public MappingBuilder RightBracket()
        {
            builder.AppendLine().AppendLine("}");
            this.PopIndent();

            return this;
        }
    }
}
