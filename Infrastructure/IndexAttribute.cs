using System;

namespace Infrastructure
{
    /// <summary>
    /// 类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IndexAttribute : Attribute
    {
        #region 索引

        /// <summary>
        /// 索引名。如果不显示设置，将使用类名作为索引名
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// 分片数 number_of_shards，默认值是5
        /// </summary>
        public int NumberOfShards { get; set; } = -1;

        /// <summary>
        /// 副本数 number_of_replicas，默认值是1
        /// </summary>
        public int NumberOfReplicas { get; set; } = -1;

        /// <summary>
        /// 最大字段数
        /// </summary>
        public int MappingTotalFieldsLimit { get; set; } = -1;

        #endregion

        #region 类型

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 是否支持动态字段
        /// </summary>
        public Dynamic Dynamic { get; set; } = Dynamic.True;

        /// <summary>
        /// "_all": { "enabled": true }
        /// es内部默认值是 false
        /// </summary>
        [Obsolete("暂不支持设置")]
        public ALLEnabled AllEnabled { get; set; } = ALLEnabled.None;

        #endregion
    }

    /// <summary>
    /// 动态索引相关配置
    /// </summary>
    public enum Dynamic
    {
        /// <summary>
        /// 默认值，表示允许选自动新增字段
        /// </summary>
        True,

        /// <summary>
        /// 不允许自动新增字段，但是文档可以正常写入，但无法对字段进行查询等操作
        /// </summary>
        False,

        /// <summary>
        /// 严格模式，文档不能写入，报错
        /// </summary>
        Strict
    }

    /// <summary>
    /// "_all": { "enabled": true }
    /// es内部默认值是 false
    /// </summary>
    public enum ALLEnabled
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None,

        /// <summary>
        /// 是
        /// </summary>
        True,

        /// <summary>
        /// 否(默认值)
        /// </summary>
        False
    }
}
