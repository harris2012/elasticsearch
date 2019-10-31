using Infrastructure;
using System;

namespace Example
{
    /// <summary>
    /// 产品
    /// </summary>
    [Type("product", Dynamic = Dynamic.False)]
    public class Product
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        [IntegerField]
        public int ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [TextField]
        public string Name { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [TextField]
        public string Ename { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [LongField]
        public long Age { get; set; }
    }
}
