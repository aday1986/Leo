using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leo.Data
{
    /// <summary>
    /// 聚合函数。
    /// </summary>
    public static class AggFunc
    {
        public static bool In<T>(this T column,params T[] values)
        {
            return true;
        }

        public static bool Like(this string column,string value)
        {
            return true;
        }

        public static int Count(object column)
        {
            return 0;
        }

        public static decimal Sum(object column)
        {
            return 0;
        }

        public static decimal Min(object column)
        {
            return 0;
        }

        public static decimal Max(object column)
        {
            return 0;
        }

        public static decimal Avg(object column)
        {
            return 0;
        }
    }
}
