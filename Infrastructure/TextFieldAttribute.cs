﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// 文本
    /// </summary>
    public class TextFieldAttribute : FieldAttribute
    {
        public override FieldType FieldType => FieldType.Text;
    }
}
