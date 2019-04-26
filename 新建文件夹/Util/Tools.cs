using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Util
{
    public static class Tools
    {
        /// <summary>
        /// 判断数组是null或者length=0
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool IsNull(object[] items)
        {
            return (items == null || items.Length == 0);
        }

        public static bool TryParse<T>(object obj, out T rsl)
        {
            if (obj != null && obj is T)
            {
                rsl = (T)obj;
                return true;
            }
            else
            {
                rsl = default(T);
                return false;
            }
        }
    }
}
