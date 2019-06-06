using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Leo.Data
{

    class CommandCollections
    {
        public CommandCollections(params IDbCommand[] commands)
        {
            this.Command = commands.FirstOrDefault();
            foreach (var command in commands)
            {
                Dictionary<string, object> keys = new Dictionary<string, object>();
                foreach (IDbDataParameter item in command.Parameters)
                {
                    keys.Add(item.ParameterName, item.Value);
                }
                DataParameters.Add(keys);
            }

        }

        public CommandCollections(IDbCommand command, Dictionary<string, object>[] dataParameters)
        {
            Command = command;
            DataParameters = dataParameters.ToList();
        }

        public IDbCommand Command { get; set; }
        public List<Dictionary<string, object>> DataParameters { get; set; } = new List<Dictionary<string, object>>();
    }

    static class SqlCore
    {
        public static event EventHandler<BeforeExecuteEventArgs> BeforeExecute;
        public static event EventHandler<AfterExecuteEventArgs> AfterExecute;
        public static event EventHandler<ErrorExecuteEventArgs> Error;


        public static int ExecuteNonQuery(object sender, CommandCollections commandCollections)
        {
            var command = commandCollections.Command;
            try
            {
                BeforeExecute?.Invoke(sender, new BeforeExecuteEventArgs() { Command = commandCollections.Command });
                var now = DateTime.Now;
                int result = 0;
                foreach (var param in commandCollections.DataParameters)
                {
                    command.Parameters.Clear();
                    foreach (var item in param)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = item.Key;
                        p.Value = item.Value;
                        command.Parameters.Add(p);
                    }
                    result += command.ExecuteNonQuery();
                }
                string message = $"Sql:{command.CommandText}\n" +
                    $"用时:{(DateTime.Now - now).TotalMilliseconds}毫秒。\n" +
                    $"受影响行数:{result}";
#if DEBUG
                Debug.Print(message);
#endif
                AfterExecute?.Invoke(sender, new AfterExecuteEventArgs() { Command = command, Message = message });
                return result;
            }
            catch (Exception ex)
            {
                Error?.Invoke(sender, new ErrorExecuteEventArgs() { Command = command, Message = ex.Message });
                throw;
            }
        }

        public static int ExecuteNonQuery(object sender, IDbCommand command)
        {
            return ExecuteNonQuery(sender, new CommandCollections(command));

        }

        public static IEnumerable<T> Query<T>(object sender, IDbCommand command)
        {
            try
            {
                BeforeExecute?.Invoke(sender, new BeforeExecuteEventArgs() { Command = command });
                var now = DateTime.Now;
                if (command.Connection.State != ConnectionState.Open) command.Connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var result = reader.ToList_Expression<T>();
                    string message = $"Sql:{command.CommandText}\n" +
                   $"用时:{(DateTime.Now - now).TotalMilliseconds}毫秒。";
#if DEBUG
                    Debug.Print(message);
#endif
                    AfterExecute?.Invoke(sender, new AfterExecuteEventArgs() { Command = command, Message = message });
                    return result;
                }
            }
            catch (Exception ex)
            {
                Error?.Invoke(sender, new ErrorExecuteEventArgs() { Command = command, Message = ex.Message });
                throw;
            }

        }

        public static object ExecuteScalar(object sender, IDbCommand command)
        {
            try
            {
                BeforeExecute?.Invoke(sender, new BeforeExecuteEventArgs() { Command = command });
                var now = DateTime.Now;
                var result = command.ExecuteReader();
                string message = $"Sql:{command.CommandText}\n" +
                    $"用时:{(DateTime.Now - now).TotalMilliseconds}毫秒。\n" +
                    $"返回结果:{result.ToString()}";
#if DEBUG
                Debug.Print(message);
#endif
                AfterExecute?.Invoke(sender, new AfterExecuteEventArgs() { Command = command, Message = message });
                return result;
            }
            catch (Exception ex)
            {
                Error?.Invoke(sender, new ErrorExecuteEventArgs() { Command = command, Message = ex.Message });
                throw;
            }
        }
    }

    class AfterExecuteEventArgs : EventArgs
    {
        public IDbCommand Command { get; set; }
        public string Message { get; set; }
    }

    class BeforeExecuteEventArgs : EventArgs
    {
        public IDbCommand Command { get; set; }
        public string Message { get; set; }
    }

    class ErrorExecuteEventArgs : EventArgs
    {
        public IDbCommand Command { get; set; }
        public string Message { get; set; }
    }


}
