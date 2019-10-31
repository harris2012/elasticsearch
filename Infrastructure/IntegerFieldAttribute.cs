using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 长整型
    /// </summary>
    public class IntegerFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Integer;
    }
}
