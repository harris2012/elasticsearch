using System;

namespace Infrastructure
{
    /// <summary>
    /// 索引
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TypeAttribute : Attribute
    {
        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="name">类型名称</param>
        public TypeAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 是否
        /// </summary>
        public Dynamic Dynamic { get; set; } = Dynamic.True;
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
}
