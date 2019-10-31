using Infrastructure;
using System;

namespace Example
{
    /// <summary>
    /// 产品
    /// </summary>
    [Index("product")]
    public class Product
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [TextField]
        public string Name { get; set; }
    }
}
