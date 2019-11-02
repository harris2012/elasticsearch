using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch.Manager
{
    public class MappingBuilder
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        private Stack<string> indents = new Stack<string>();

        private string indent = "  ";

        //private string BeforeColon = " ";
        private string BeforeColon = string.Empty;

        public MappingBuilder()
        {
            LeftBracket();
        }

        public string Build()
        {
            RightBracket();

            return stringBuilder.ToString();
        }

        #region Public Methods

        /// <summary>
        /// 直接调用底层 Append
        /// </summary>
        public MappingBuilder Append(string text)
        {
            stringBuilder.Append(text);

            return this;
        }

        /// <summary>
        /// 直接调用底层 AppendLine
        /// </summary>
        public MappingBuilder AppendLine(string text)
        {
            stringBuilder.AppendLine(text);

            return this;
        }

        #endregion

        #region KeyValue

        public MappingBuilder KeyValue(string key, object value)
        {
            switch (value.GetType().FullName)
            {
                case "System.Int32":
                case "System.Int64":
                    stringBuilder.Append(GetIndent()).Append("\"").Append(key).Append("\"").Append(BeforeColon).Append(": ").Append(value);
                    break;
                case "System.Boolean":
                    stringBuilder.Append(GetIndent()).Append("\"").Append(key).Append("\"").Append(BeforeColon).Append(": ").Append((bool)value ? "true" : "false");
                    break;
                case "System.String":
                    stringBuilder.Append(GetIndent()).Append("\"").Append(key).Append("\"").Append(BeforeColon).Append(": \"").Append(value).Append("\"");
                    break;
                default:
                    break;
            }

            return this;
        }

        public MappingBuilder KeyValue(string key, Action<MappingBuilder> value)
        {
            StartKey(key);

            value(this);

            EndKey();

            return this;
        }

        public MappingBuilder KeyValue(string key, Dictionary<string, object> items)
        {
            StartKey(key);

            KeyValue(items);

            EndKey();

            return this;
        }

        public MappingBuilder KeyValue(string key, List<string> items)
        {
            StartArray(key);

            for (int i = 0; i < items.Count; i++)
            {
                stringBuilder.Append(GetIndent()).Append("\"").Append(items[i]).Append("\"");
                if (i < items.Count - 1)
                {
                    stringBuilder.AppendLine(",");
                }
            }

            EndArray();

            return this;
        }

        public MappingBuilder KeyValue(Dictionary<string, object> items)
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

            return this;
        }

        public MappingBuilder KeyValue(string key, Type type, Action<MappingBuilder, Type> value)
        {
            StartKey(key);

            value(this, type);

            EndKey();

            return this;
        }

        public MappingBuilder KeyValue(string key, List<CustomTokenizerAttribute> customTokenizerAttributeList, Action<MappingBuilder, List<CustomTokenizerAttribute>> value)
        {
            StartKey(key);

            value(this, customTokenizerAttributeList);

            EndKey();

            return this;
        }

        public MappingBuilder KeyValue(string key, List<CustomAnalyzerAttribute> customAnalyzerAttributeList, List<CustomTokenizerAttribute> customTokenizerAttributeList, Action<MappingBuilder, List<CustomAnalyzerAttribute>, List<CustomTokenizerAttribute>> value)
        {
            StartKey(key);

            value(this, customAnalyzerAttributeList, customTokenizerAttributeList);

            EndKey();

            return this;
        }

        public MappingBuilder KeyValue(string key, CustomTokenizerAttribute customTokenizerAttribute, Action<MappingBuilder, CustomTokenizerAttribute> value)
        {
            StartKey(key);

            value(this, customTokenizerAttribute);

            EndKey();

            return this;
        }

        public MappingBuilder KeyValue(string key, List<CustomAnalyzerAttribute> customAnalyzerAttributeList, Action<MappingBuilder, List<CustomAnalyzerAttribute>> value)
        {
            StartKey(key);

            value(this, customAnalyzerAttributeList);

            EndKey();

            return this;
        }

        public MappingBuilder KeyValue(string key, CustomAnalyzerAttribute customAnalyzerAttribute, Action<MappingBuilder, CustomAnalyzerAttribute> value)
        {
            StartKey(key);

            value(this, customAnalyzerAttribute);

            EndKey();

            return this;
        }

        public MappingBuilder KeyValue(string key, TextFieldAttribute textFieldAttribute, int keywordIgnoreAbove, Action<MappingBuilder, TextFieldAttribute, int> value)
        {
            StartKey(key);

            value(this, textFieldAttribute, keywordIgnoreAbove);

            EndKey();

            return this;
        }

        public MappingBuilder KeyValue(string key, FieldAttribute fieldAttribute, Action<MappingBuilder, FieldAttribute> value)
        {
            StartKey(key);

            value(this, fieldAttribute);

            EndKey();

            return this;
        }

        #endregion

        private void StartKey(string key)
        {
            Start(key, "{");
        }

        private void EndKey()
        {
            End("}");
        }

        private void StartArray(string key)
        {
            Start(key, "[");
        }

        private void EndArray()
        {
            End("]");
        }

        private void Start(string key, string start)
        {
            stringBuilder.Append(GetIndent()).Append("\"").Append(key).Append("\"").Append(BeforeColon).AppendLine($": {start}");
            PushIndent();
        }

        private void End(string end)
        {
            PopIndent();
            stringBuilder.AppendLine().Append(GetIndent()).Append(end);
        }

        private string GetIndent()
        {
            return string.Join(string.Empty, indents);
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

        private MappingBuilder LeftBracket()
        {
            stringBuilder.AppendLine("{");
            this.PushIndent();

            return this;
        }

        private MappingBuilder RightBracket()
        {
            stringBuilder.AppendLine().AppendLine("}");
            this.PopIndent();

            return this;
        }
    }
}
