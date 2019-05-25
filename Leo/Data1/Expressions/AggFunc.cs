using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leo.Data1.Expressions
{
    /// <summary>
    /// 聚合函数。
    /// </summary>
   public abstract class AggFunc
    {
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
