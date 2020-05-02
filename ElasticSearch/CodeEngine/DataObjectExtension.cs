using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.CodeDom.Js
{
    public static class DataObjectExtension
    {
        public static DataObject AddDataValue(this DataObject dataObject, string key, DataValue dataValue)
        {
            if (dataObject.DataValueMap == null)
            {
                dataObject.DataValueMap = new Dictionary<string, DataValue>();
            }

            dataObject.DataValueMap.Add(key, dataValue);

            return dataObject;
        }

        public static DataValue AddDataValue(this DataObject dataObject, string key)
        {
            if (dataObject.DataValueMap == null)
            {
                dataObject.DataValueMap = new Dictionary<string, DataValue>();
            }

            DataValue dataValue = new DataValue();

            dataObject.DataValueMap.Add(key, dataValue);

            return dataValue;
        }

        public static void AddDataObject(this DataObject dataObject, string key, DataObject subDataObject)
        {
            if (dataObject.DataObjectMap == null)
            {
                dataObject.DataObjectMap = new Dictionary<string, DataObject>();
            }

            dataObject.DataObjectMap.Add(key, subDataObject);
        }

        public static DataObject AddDataObject(this DataObject dataObject, string key)
        {
            if (dataObject.DataObjectMap == null)
            {
                dataObject.DataObjectMap = new Dictionary<string, DataObject>();
            }

            DataObject subDataObject = new DataObject();

            dataObject.DataObjectMap.Add(key, subDataObject);

            return subDataObject;
        }

        public static void AddDataArray(this DataObject dataObject, string key, DataArray dataArray)
        {
            if (dataObject.DataArrayMap == null)
            {
                dataObject.DataArrayMap = new Dictionary<string, DataArray>();
            }

            dataObject.DataArrayMap.Add(key, dataArray);
        }

        public static DataArray AddDataArray(this DataObject dataObject, string key)
        {
            if (dataObject.DataArrayMap == null)
            {
                dataObject.DataArrayMap = new Dictionary<string, DataArray>();
            }

            DataArray dataArray = new DataArray();

            dataObject.DataArrayMap.Add(key, dataArray);

            return dataArray;
        }
    }
}
