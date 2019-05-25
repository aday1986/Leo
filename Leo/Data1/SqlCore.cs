using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Leo.Data1
{

    public class SqlCore 
    {
        public event EventHandler<BeforeExecuteEventArgs> BeforeExecute;
        public event EventHandler<AfterExecuteEventArgs> AfterExecute;
        public event EventHandler<ErrorExecuteEventArgs> Error;

        public int ExecuteNonQuery(IDbCommand command)
        {
            try
            {
                BeforeExecute(this, new BeforeExecuteEventArgs() { Command = command });
                var now = DateTime.Now;
                var result = command.ExecuteNonQuery();
                string message = $"Sql:{command.CommandText}\n" +
                    $"用时:{(DateTime.Now - now).TotalMilliseconds}毫秒。\n" +
                    $"受影响行数:{result}";
                AfterExecute(this, new AfterExecuteEventArgs() { Command = command, Message = message });
                return result;
            }
            catch (Exception ex)
            {
                Error(this, new ErrorExecuteEventArgs() { Command = command, Message = ex.Message });
                throw;
            }

        }

        public IDataReader ExecuteReader(IDbCommand command)
        {
            try
            {
                BeforeExecute(this, new BeforeExecuteEventArgs() { Command = command });
                var now = DateTime.Now;
                var result = command.ExecuteReader();
                string message = $"Sql:{command.CommandText}\n" +
                    $"用时:{(DateTime.Now - now).TotalMilliseconds}毫秒。";
                AfterExecute(this, new AfterExecuteEventArgs() { Command = command, Message = message });
                return result;
            }
            catch (Exception ex)
            {
                Error(this, new ErrorExecuteEventArgs() { Command = command, Message = ex.Message });
                throw;
            }
           
        }

        public object ExecuteScalar(IDbCommand command)
        {
            try
            {
                BeforeExecute(this, new BeforeExecuteEventArgs() { Command = command });
                var now = DateTime.Now;
                var result = command.ExecuteReader();
                string message = $"Sql:{command.CommandText}\n" +
                    $"用时:{(DateTime.Now - now).TotalMilliseconds}毫秒。\n" +
                    $"返回结果:{result.ToString()}";
                AfterExecute(this, new AfterExecuteEventArgs() { Command = command, Message = message });
                return result;
            }
            catch (Exception ex)
            {
                Error(this, new ErrorExecuteEventArgs() { Command = command, Message = ex.Message });
                throw;
            }  
        }
    }

}
