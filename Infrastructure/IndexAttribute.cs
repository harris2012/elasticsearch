using System;

namespace Infrastructure
{
    /// <summary>
    /// 索引
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IndexAttribute : Attribute
    {
        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="name">索引名</param>
        public IndexAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 索引名称
        /// </summary>
        public string Name { get; private set; }
    }
}
