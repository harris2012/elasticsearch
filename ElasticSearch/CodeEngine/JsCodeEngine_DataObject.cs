using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.CodeDom.Js.Engine
{
    partial class JsCodeEngine
    {
        public void GenerateDataObject(DataObject dataObject, CodeWriter codeWriter, GenerateOptions options = null, bool endWithLine = true)
        {
            if (dataObject == null) { return; }
            if (codeWriter == null) { return; }
            options = options ?? new GenerateOptions();

            var hasDataValueMap = dataObject.DataValueMap != null && dataObject.DataValueMap.Count > 0;
            var hasDataArrayMap = dataObject.DataArrayMap != null && dataObject.DataArrayMap.Count > 0;
            var hasDataObjectMap = dataObject.DataObjectMap != null && dataObject.DataObjectMap.Count > 0;

            codeWriter.Write(Marks.LEFT_BRACE);

            if (hasDataValueMap || hasDataArrayMap || hasDataObjectMap)
            {
                codeWriter.WriteLine();

                options.PushIndent();

                BuildDataValueMap(dataObject.DataValueMap, codeWriter, options, endWithComma: hasDataArrayMap || hasDataObjectMap);

                BuildDataArrayMap(dataObject.DataArrayMap, codeWriter, options, endWithComma: hasDataObjectMap);

                BuildDataObjectMap(dataObject.DataObjectMap, codeWriter, options);

                options.PopIndent();

                codeWriter.Write(options.IndentString);
            }

            codeWriter.Write(Marks.RIGHT_BRACE);

            if (endWithLine)
            {
                codeWriter.WriteLine();
            }
        }

        private void BuildDataValueMap(Dictionary<string, DataValue> dataValueMap, CodeWriter codeWriter, GenerateOptions options = null, bool endWithComma = false)
        {
            if (dataValueMap == null || dataValueMap.Count == 0) { return; }
            if (codeWriter == null) { return; }
            options = options ?? new GenerateOptions();

            var enumerator = dataValueMap.GetEnumerator();
            var moveNext = enumerator.MoveNext();
            while (moveNext)
            {
                var item = enumerator.Current;
                codeWriter.Write(options.IndentString).Write(item.Key).Write(": ");
                if (item.Value == null)
                {
                    codeWriter.Write("null");
                }
                else
                {
                    codeWriter.Write(item.Value.Value ?? "null");
                }

                moveNext = enumerator.MoveNext();
                if (moveNext || endWithComma)
                {
                    codeWriter.Write(",");
                }
                codeWriter.WriteLine();
            }
        }

        private void BuildDataArrayMap(Dictionary<string, DataArray> dataArrayMap, CodeWriter codeWriter, GenerateOptions options = null, bool endWithComma = false)
        {
            if (dataArrayMap == null) { return; }
            if (codeWriter == null) { return; }
            options = options ?? new GenerateOptions();

            var enumerator = dataArrayMap.GetEnumerator();
            var moveNext = enumerator.MoveNext();
            while (moveNext)
            {
                var dataArrayItem = enumerator.Current;

                codeWriter.Write(options.IndentString).Write($"{dataArrayItem.Key}: ");

                BuildDataArray(dataArrayItem.Value, codeWriter, options);

                moveNext = enumerator.MoveNext();
                if (moveNext || endWithComma)
                {
                    codeWriter.Write(",");
                }
                codeWriter.WriteLine();
            }
        }

        private void BuildDataArray(DataArray dataArray, CodeWriter codeWriter, GenerateOptions options)
        {
            if (dataArray.Items == null || dataArray.Items.Count == 0)
            {
                codeWriter.Write("[]");
            }
            else
            {
                codeWriter.Write("[");

                var enumerator = dataArray.Items.GetEnumerator();
                var first = true;
                var moveNext = enumerator.MoveNext();
                while (moveNext)
                {
                    var current = enumerator.Current;

                    if (current is DataObject)
                    {
                        GenerateDataObject(current as DataObject, codeWriter, options, false);

                        moveNext = enumerator.MoveNext();
                        if (moveNext)
                        {
                            codeWriter.Write(", ");
                        }
                    }
                    else if (current is DataValue)
                    {
                        if (first)
                        {
                            codeWriter.WriteLine();
                        }
                        options.PushIndent();
                        codeWriter.Write(options.IndentString).Write((current as DataValue).Value);
                        options.PopIndent();

                        moveNext = enumerator.MoveNext();
                        if (moveNext)
                        {
                            codeWriter.Write(",");
                        }
                        codeWriter.WriteLine();
                        if (!moveNext)
                        {
                            codeWriter.Write(options.IndentString);
                        }
                    }
                    else
                    {
                        moveNext = enumerator.MoveNext();
                    }

                    first = false;
                }

                codeWriter.Write("]");
            }
        }

        private void BuildDataObjectMap(Dictionary<string, DataObject> dataObjectMap, CodeWriter codeWriter, GenerateOptions options = null)
        {
            if (dataObjectMap == null) { return; }
            if (codeWriter == null) { return; }
            options = options ?? new GenerateOptions();

            var enumerator = dataObjectMap.GetEnumerator();
            var moveNext = enumerator.MoveNext();
            while (moveNext)
            {
                var item = enumerator.Current;

                codeWriter.Write(options.IndentString).Write($"{item.Key}: ");

                GenerateDataObject(item.Value, codeWriter, options, false);

                moveNext = enumerator.MoveNext();
                if (moveNext)
                {
                    codeWriter.WriteLine(",");
                }
                else
                {
                    codeWriter.WriteLine();
                }
            }
        }
    }
}
