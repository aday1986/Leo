using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Leo.Data.Extensions
{
    public static class IDataReaderExtensions
    {
        /// <summary>
        /// 属性反射信息缓存 key:类型的hashCode,value属性信息
        /// </summary>
        private static Dictionary<int, Dictionary<string, PropertyInfo>> propInfoCache = new Dictionary<int, Dictionary<string, PropertyInfo>>();

        /// <summary>
        /// 将SqlDataReader转成T类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T ToModel<T>(this IDataReader reader)
        {
            if (reader == null) return default(T);

            var res = Activator.CreateInstance<T>(); ;
            var propInfos = GetFieldnameFromCache<T>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var n = reader.GetName(i);
                if (propInfos.ContainsKey(n))
                {
                    PropertyInfo prop = propInfos[n];
                    var IsValueType = prop.PropertyType.IsValueType;
                    object defaultValue = null;//引用类型或可空值类型的默认值
                    if (IsValueType)
                    {
                        if ((!prop.PropertyType.IsGenericType)
                            || (prop.PropertyType.IsGenericType && !prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))))
                        {
                            defaultValue = 0;//非空值类型的默认值
                        }
                    }
                    var v = reader.GetValue(i);
                    if (prop.PropertyType.IsEnum || prop.PropertyType==typeof(Int32))
                    {
                        prop.SetValue(res,Convert.ToInt32( v), null);
                    }
                    else if (Leo.Util.Converter.TryParse(v, prop.PropertyType, out object result))
                    {
                        prop.SetValue(res, result, null);
                    }
                    else
                    {
                        throw new Exception($"{v.GetType().Name}类型无法被解析为{prop.Name}。");
                    }

                }
            }

            return res;
        }

        /// <summary>
        /// 将SqlDataReader转成List<T>类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IDataReader reader)
        {
            if (reader == null) return null;
            var res = new List<T>();
            while (reader.Read())
            {
                res.Add(reader.ToModel<T>());
            }
            return res;
        }

        private static Dictionary<string, PropertyInfo> GetFieldnameFromCache<T>()
        {
            Dictionary<string, PropertyInfo> res = null;
            var hashCode = typeof(T).GetHashCode();
            if (!propInfoCache.ContainsKey(hashCode))
            {
                propInfoCache.Add(hashCode, GetFieldname<T>());
            }
            res = propInfoCache[hashCode];
            return res;
        }

        /// <summary>
        /// 获取一个类型的对应数据表的字段信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Dictionary<string, PropertyInfo> GetFieldname<T>()
        {
            var res = new Dictionary<string, PropertyInfo>();
            var props = typeof(T).GetProperties();
            foreach (PropertyInfo item in props)
            {
                res.Add(ColumnAttribute.GetColumnName(item), item);
            }
            return res;
        }

    }
}
