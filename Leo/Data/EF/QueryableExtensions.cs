using System.Collections.Generic;
using System.Linq;

namespace Leo.Data.EF
{
    public static partial class QueryableExtensions
    {
        /// <summary>
        /// 添加查询条件。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> query, IEnumerable<Condition> conditions)
        {
            var parser = new ExpressionParser<T>();
            var filter = parser.ParserConditions(conditions);
            query = query.Where(filter);
            return query;
        }

        /// <summary>
        /// 分页。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public static IQueryable<T> Pager<T>(this IQueryable<T> query, int pageindex, int pagesize)
        {
            return query.Skip((pageindex - 1) * pagesize).Take(pagesize);
        }
    }
}
