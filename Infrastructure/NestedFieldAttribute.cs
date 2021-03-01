using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 嵌套类型
    /// </summary>
    public class NestedFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.NestedObject;

        public override string Type => "nested";
    }
}
