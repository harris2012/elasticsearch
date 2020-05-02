using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.CodeDom.Js
{
    public static class DataArrayExtension
    {
        public static DataArray AddDataValue(this DataArray dataArray, DataValue dataValue)
        {
            if (dataArray.Items == null)
            {
                dataArray.Items = new List<DataValueOrObject>();
            }

            dataArray.Items.Add(dataValue);

            return dataArray;
        }

        public static DataArray AddDataObject(this DataArray dataArray, DataObject dataObject)
        {
            if (dataArray.Items == null)
            {
                dataArray.Items = new List<DataValueOrObject>();
            }

            dataArray.Items.Add(dataObject);

            return dataArray;
        }

        public static DataObject AddDataObject(this DataArray dataArray)
        {
            if (dataArray.Items == null)
            {
                dataArray.Items = new List<DataValueOrObject>();
            }

            DataObject dataObject = new DataObject();

            dataArray.Items.Add(dataObject);

            return dataObject;
        }
    }
}
