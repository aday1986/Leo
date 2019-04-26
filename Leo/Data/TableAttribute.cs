using System;
using System.Collections.Generic;
using System.Linq;
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

        private static Type tableAttributeType = typeof(TableAttribute);
        public static bool TryGetTableAttribute<T>(out TableAttribute tableAttribute)
        {
            var type = typeof(T);
            var atts = type.GetCustomAttributes(tableAttributeType, false);
            if (atts.Any())
            {
                tableAttribute = (TableAttribute)atts[0];
                return true;
            }
            else
            {
                tableAttribute = null;
                return false;
            }
        }

    }
}
