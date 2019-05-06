using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Leo.Util
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 判断数组是否为null或count为0。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpry<T>(this IEnumerable<T> value)
        {
            return (value == null || value.Count() == 0);
        }

        public static IEnumerable<Assembly> GetAllAssemblies(this Assembly assembly)
        {
            var result = new List<Assembly>();
            result.Add(assembly);
            result.AddRange(assembly.GetReferencedAssemblies().Select(Assembly.Load).Distinct());
            return result;
        }

        public static IEnumerable<Type> GetAllDefinedTypes(this Assembly assembly)
        {
            var asses = assembly.GetAllAssemblies();
            var types = asses.SelectMany(y => y.GetTypes())
                .Distinct(new TypeEqualityComparer())
                .Where (t=>t.IsPublic)
                .ToList();
            if (types.Contains(null)) types.Remove(null);
            return types;
        }

        private class TypeEqualityComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                return GetHashCode(x) == GetHashCode(y);
            }

            public int GetHashCode(Type obj)
            {
                if (obj == null) return -1;
              return  obj.MetadataToken;
            }
        }
    }
}
