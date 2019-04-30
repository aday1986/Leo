using System.Collections.Generic;

namespace Leo.Data
{
    public interface ISqlBulider
    {
        string GetCreateSql<T>();
        string GetDeleteSql<T>();
        string GetDeleteSql<T>(IEnumerable<Condition> conditions, out Dictionary<string, object> parameters);
        string GetInsertSql<T>();
        string GetSelectSql<T>(IEnumerable<Condition> conditions, out Dictionary<string, object> parameters, int? top = null);
        string GetSelectSql<T>(IEnumerable<Condition> conditions, out Dictionary<string, object> parameters, int pageSize, int pageIndex);
        string GetUpdateSql<T>();
        string GetWhere(IEnumerable<Condition> conditions, out Dictionary<string, object> parameters);
    }
}