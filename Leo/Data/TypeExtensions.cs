using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Leo.Data
{
    public static partial class TypeExtensions
    {
        public static bool TryGetTableAttribute(this Type type, out TableAttribute tableAttribute)
        {
            var atts = type.GetCustomAttributes(typeof(TableAttribute), false);
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

        /// <summary>
        /// 字段特性缓存。
        /// </summary>
        private static Dictionary<Type, Dictionary<string, ColumnAttribute>> columnAttributesCaches { get; }
            = new Dictionary<Type, Dictionary<string, ColumnAttribute>>();

        public static bool TryGetColumnAttributes(this Type type, out Dictionary<string, ColumnAttribute> columnAttributes)
        {
            if (!columnAttributesCaches.TryGetValue(type, out columnAttributes))
            {
                var infos = type.GetProperties();
                var dic = new Dictionary<string, ColumnAttribute>();
                foreach (var info in infos)
                {
                    var atts = info.GetCustomAttributes(typeof(ColumnAttribute), false);
                    if (atts.Any())
                    {
                        dic.Add(info.Name, (ColumnAttribute)atts[0]);
                    }
                }
                columnAttributes = dic;
                columnAttributesCaches[type] = dic;
            }
            return columnAttributes.Any();

        }

        public static bool TryGetColumnAttribute(this PropertyInfo propertyInfo, out ColumnAttribute columnAttribute)
        {
            var atts = (ColumnAttribute[])propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), false);
            columnAttribute = atts.FirstOrDefault();
            return atts.Any();
        }

        public static bool TryGetKeyColumns(this Type type, out Dictionary<string, ColumnAttribute> keyColumns)
        {
            keyColumns = new Dictionary<string, ColumnAttribute>();
            if (TryGetColumnAttributes(type, out Dictionary<string, ColumnAttribute> columns))
            {
                var keys = columns.Where(key => key.Value.IsPrimaryKey);
                if (keys.Any())
                {
                    foreach (var item in keys)
                    {
                        keyColumns.Add(item.Key, item.Value);
                    }
                    return true;
                }
            }
            return false;
        }

       
    }
}
