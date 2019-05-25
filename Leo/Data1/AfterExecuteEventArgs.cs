using System;
using System.Data;

namespace Leo.Data1
{
    public class AfterExecuteEventArgs : EventArgs
    {
        public IDbCommand Command { get; set; }
        public string Message { get; set; }
    }


}
