using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static bool TryGetColumnAttributes<T>(out Dictionary<string, ColumnAttribute> columnAttributes)
        {
            var type = typeof(T);
            return type.TryGetColumnAttributes(out columnAttributes);

        }

        public static bool TryGetColumnAttribute(PropertyInfo propertyInfo, out ColumnAttribute columnAttribute)
        {
            return propertyInfo.TryGetColumnAttribute(out columnAttribute);
        }

        public static bool TryGetKeyColumns<T>(out Dictionary<string, ColumnAttribute> keyColumns)
        {
            var type = typeof(T);
            return type.TryGetKeyColumns(out keyColumns);
        }

    }
}
