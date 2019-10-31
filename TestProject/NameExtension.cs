using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    static class NameExtension
    {
        /// <summary>
        /// 小写的名称，由Name计算而来
        /// 示例：Student -> STUDENT
        /// 示例：StudentScore -> STUDENT_SCORE
        /// </summary>
        public static string ToUpperCaseUnderLine(this string name)
        {
            return ToLowerCaseUnderLine(name).ToUpper();
        }

        /// <summary>
        /// 小写的名称，由Name计算而来
        /// 示例：Student -> student
        /// 示例：StudentScore -> student_score
        /// </summary>
        public static string ToLowerCaseUnderLine(this string name)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                var ch = name[i];

                if (ch >= 'A' && ch <= 'Z')
                {
                    if (i > 0)
                    {
                        builder.Append("_");
                    }
                    builder.Append((char)(ch + 32));
                }
                else
                {
                    builder.Append(name[i]);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 由Name计算而来
        /// 示例：name, studentScore
        /// </summary>
        public static string ToLowerCamelCase(this string name)
        {
            return name[0].ToString().ToLower() + name.Substring(1);
        }
    }
}
