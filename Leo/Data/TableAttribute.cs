using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leo.Data
{
    [AttributeUsage( AttributeTargets.Class)]
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

       public static string GetTableName<T>()
        {
            return GetTableName(typeof(T));
        }

       public static string GetTableName(Type type)
        {
            var table = type.GetCustomAttributes(type, false).OfType<TableAttribute>().FirstOrDefault();
            return table?.TableName ?? type.Name;
        }

    }
}
