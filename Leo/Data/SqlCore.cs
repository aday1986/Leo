using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Leo.Data
{

    static class SqlCore 
    {
        public static event EventHandler<BeforeExecuteEventArgs> BeforeExecute;
        public static event EventHandler<AfterExecuteEventArgs> AfterExecute;
        public static event EventHandler<ErrorExecuteEventArgs> Error;

        public static int ExecuteNonQuery(object sender, IDbCommand command)
        {
            try
            {
                BeforeExecute(sender, new BeforeExecuteEventArgs() { Command = command });
                var now = DateTime.Now;
                var result = command.ExecuteNonQuery();
                string message = $"Sql:{command.CommandText}\n" +
                    $"用时:{(DateTime.Now - now).TotalMilliseconds}毫秒。\n" +
                    $"受影响行数:{result}";
                AfterExecute(sender, new AfterExecuteEventArgs() { Command = command, Message = message });
                return result;
            }
            catch (Exception ex)
            {
                Error(sender, new ErrorExecuteEventArgs() { Command = command, Message = ex.Message });
                throw;
            }

        }

        public static IDataReader ExecuteReader(object sender, IDbCommand command)
        {
            try
            {
                BeforeExecute?.Invoke(sender, new BeforeExecuteEventArgs() { Command = command });
                var now = DateTime.Now;
                if (command.Connection.State!= ConnectionState.Open) command.Connection.Open();
                var result = command.ExecuteReader();
                string message = $"Sql:{command.CommandText}\n" +
                    $"用时:{(DateTime.Now - now).TotalMilliseconds}毫秒。";
                AfterExecute?.Invoke(sender, new AfterExecuteEventArgs() { Command = command, Message = message });
                return result;
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
                BeforeExecute(sender, new BeforeExecuteEventArgs() { Command = command });
                var now = DateTime.Now;
                var result = command.ExecuteReader();
                string message = $"Sql:{command.CommandText}\n" +
                    $"用时:{(DateTime.Now - now).TotalMilliseconds}毫秒。\n" +
                    $"返回结果:{result.ToString()}";
                AfterExecute(sender, new AfterExecuteEventArgs() { Command = command, Message = message });
                return result;
            }
            catch (Exception ex)
            {
                Error(sender, new ErrorExecuteEventArgs() { Command = command, Message = ex.Message });
                throw;
            }  
        }
    }

    public class AfterExecuteEventArgs : EventArgs
    {
        public IDbCommand Command { get; set; }
        public string Message { get; set; }
    }

    public class BeforeExecuteEventArgs : EventArgs
    {
        public IDbCommand Command { get; set; }
        public string Message { get; set; }
    }

    public class ErrorExecuteEventArgs : EventArgs
    {
        public IDbCommand Command { get; set; }
        public string Message { get; set; }
    }


}
