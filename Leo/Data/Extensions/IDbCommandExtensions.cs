using System.Collections.Generic;
using System.Data;

namespace Leo.Data
{
    public static class IDbCommandExtensions
    {
        public static int ExecuteNonQuery(this IDbCommand command,object sender, string sql, Dictionary<string, object> parms)
        {
            command.CommandText = sql;
            foreach (var item in parms)
            {
                var p = command.CreateParameter();
                p.ParameterName = item.Key;
                p.Value = item.Value;
                command.Parameters.Add(p);
            }
            return SqlCore.ExecuteNonQuery(sender, command);
        }

        public static int ExecuteNonQuery(this IDbCommand command, object sender, string sql,List< Dictionary<string, object>> parms)
        {
            command.CommandText = sql;
            return SqlCore.ExecuteNonQuery(sender, new CommandCollections(command,parms));
        }
    }

}
