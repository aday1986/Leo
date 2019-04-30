using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Data
{
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表描述信息
        /// </summary>
        public string Description { get; set; }

        public static bool TryGetTableAttribute<T>(out TableAttribute tableAttribute)
        {
            return typeof(T).TryGetTableAttribute(out tableAttribute);
        }

    }
}
