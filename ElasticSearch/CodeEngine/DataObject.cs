using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.CodeDom.Js
{
    public class DataObject: DataValueOrObject
    {
        /// <summary>
        /// 每一个Value是一个值
        /// </summary>
        public Dictionary<string, DataValue> DataValueMap { get; set; }

        /// <summary>
        /// 每个Value是一个Object
        /// </summary>
        public Dictionary<string, DataObject> DataObjectMap { get; set; }

        /// <summary>
        /// 每个元素是一个数组
        /// </summary>
        public Dictionary<string, DataArray> DataArrayMap { get; set; }
    }
}
