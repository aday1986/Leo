using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leo.Util
{
  public static  class Extensions
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
    }
}
