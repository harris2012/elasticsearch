using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 属性类型
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// 【文本型】text。建议直接使用`TextField`
        /// </summary>
        Text,

        /// <summary>
        /// 【文本型】keyword（不会分词）
        /// </summary>
        Keyword,

        /// <summary>
        /// 【数值型】integer
        /// </summary>
        Integer,

        /// <summary>
        /// 【数值型】long
        /// </summary>
        Long,

        /// <summary>
        /// 【数值型】short
        /// </summary>
        Short,

        /// <summary>
        /// 【数值型】byte
        /// </summary>
        Byte,

        /// <summary>
        /// 【数值型】double
        /// </summary>
        Double,

        /// <summary>
        /// 【数值型】float
        /// </summary>
        Float,

        /// <summary>
        /// 【数值型】half_float
        /// </summary>
        HalfFloat,

        /// <summary>
        /// 【日期类型】date
        /// </summary>
        Date,

        /// <summary>
        /// 【布尔类型】boolean
        /// </summary>
        Boolean,

        /// <summary>
        /// 【二进制类型】binary
        /// </summary>
        Binary,

        /// <summary>
        /// 【范围类型】integer_range
        /// </summary>
        IntegerRange,

        /// <summary>
        /// 【范围类型】float_range
        /// </summary>
        FloatRange,

        /// <summary>
        /// 【范围类型】long_range
        /// </summary>
        LongRange,

        /// <summary>
        /// 【范围类型】double_range
        /// </summary>
        DoubleRange,

        /// <summary>
        /// 【范围类型】date_range
        /// </summary>
        DateRange,

        /// <summary>
        /// 【数组类型】array
        /// </summary>
        Array,

        /// <summary>
        /// 【对象类型】object
        /// </summary>
        Object,

        /// <summary>
        /// 【嵌套类型】nested object
        /// </summary>
        NestedObject,

        /// <summary>
        /// 【地理位置数据类型】geo_point
        /// </summary>
        GeoPoint,

        /// <summary>
        /// 【地理位置数据类型】geo_shape
        /// </summary>
        GeoShape,

        /// <summary>
        /// 【专用类型】ip（记录ip地址）
        /// </summary>
        Ip,

        /// <summary>
        /// 【专用类型】completion（实现自动补全）
        /// </summary>
        Completion,

        /// <summary>
        /// 【专用类型】token_count（记录分词数）
        /// </summary>
        TokenCount,

        /// <summary>
        /// 【专用类型】murmur3（记录字符串hash值）
        /// </summary>
        Murmur3
    }
}
