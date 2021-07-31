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
                    tokenizerDataObject.AddDataObject(DataKey.DoubleQuotationString(customTokenizerAttribute.Name), tokenizerBody);
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
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(abstractNGramTokenizerAttribute.Type));

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
                    tokenCharsArray.AddDataValue(DataValue.DoubleQuotationString(item));
                }
            }

            return dataObject;
        }

        private static DataObject BuildPatternTokenier(PatternTokenizerAttribute patternTokenizerAttribute)
        {
            DataObject dataObject = new DataObject();
            dataObject.AddDataValue(DataKey.DoubleQuotationString("type"), DataValue.DoubleQuotationString(patternTokenizerAttribute.Type));

            if (!string.IsNullOrEmpty(patternTokenizerAttribute.Pattern))
            {
                dataObject.AddDataValue(DataKey.DoubleQuotationString("pattern"), DataValue.DoubleQuotationString(patternTokenizerAttribute.Pattern));
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

        private static DataObject BuildAnalyzerProperties(CustomAnalyzerAttribute customAnalyzerAttribute)
        {
            DataObject dataObject = new DataObject();

            dataObject.AddDataValue(DataKey.DoubleQuotationString("tokenizer"), DataValue.DoubleQuotationString(customAnalyzerAttribute.Tokenizer));

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
                    var dataArray = dataObject.AddDataArray(DataKey.DoubleQuotationString("filter"));
                    foreach (var item in tokenFilters)
                    {
                        dataArray.AddDataValue(DataValue.DoubleQuotationString(item));
                    }
                }
            }

            return dataObject;
        }
    }
}
