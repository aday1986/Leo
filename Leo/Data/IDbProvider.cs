using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Leo.Data
{
    /// <summary>
    /// 表示一组数据库连接工厂。
    /// </summary>
    public interface IDbProvider
    {
        IDbConnection CreateConnection();
        IDbDataAdapter CreateAdapter();
        IDbCommand CreateCommand();
        IDbDataParameter CreateDataParameter(string key, object value);
    }
}
