using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leo.Data
{
    /// <summary>
    /// 字段特性
    /// </summary>
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int ColumnOrder { set; get; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string ColumnName { set; get; }

        /// <summary>
        /// 长度
        /// </summary>
        public int? MaxLength { set; get; }

        /// <summary>
        /// 精度
        /// </summary>
        public int Precision { set; get; }

        /// <summary>
        /// 小数位
        /// </summary>
        public int Scale { set; get; }

        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPrimaryKey { set; get; }

        public bool Unique { get; set; }

        /// <summary>
        /// 是否是标识列
        /// </summary>
        public bool IsIdentity { set; get; }

        /// <summary>
        /// 是否允许空
        /// </summary>
        public bool Nullable { set; get; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// 不可被更新。
        /// </summary>
        public bool NoUpdate { get; set; }


        private static readonly Type columnAttributeType = typeof(ColumnAttribute);
        private static Dictionary<Type, Dictionary<string, ColumnAttribute>> columnAttributesCaches 
            = new Dictionary<Type, Dictionary<string, ColumnAttribute>>();
        public static bool TryGetColumnAttributes<T>(out Dictionary<string, ColumnAttribute> columnAttributes)
        {
            var type = typeof(T);
            if (!columnAttributesCaches.TryGetValue(type, out columnAttributes))
            {
                var infos = type.GetProperties();
                var dic = new Dictionary<string, ColumnAttribute>();
                foreach (var info in infos)
                {
                    var atts = info.GetCustomAttributes(columnAttributeType, false);
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

        public static bool TryGetColumnAttribute(System.Reflection.PropertyInfo propertyInfo, out ColumnAttribute columnAttribute)
        {
           var atts= (ColumnAttribute[])propertyInfo.GetCustomAttributes(columnAttributeType, false);
            columnAttribute = atts.FirstOrDefault();
            return atts.Any();
        }

        public static bool TryGetKeyColumns<T>(out Dictionary<string, ColumnAttribute> keyColumns)
        {
            var type = typeof(T);
            keyColumns = new Dictionary<string, ColumnAttribute>();
            if (TryGetColumnAttributes<T>(out Dictionary<string, ColumnAttribute> columns))
            {
               var keys= columns.Where(key => key.Value.IsPrimaryKey);
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
