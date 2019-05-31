using System;
using System.Data;

namespace Leo.Data
{
    public class BeforeExecuteEventArgs : EventArgs
    {
        public IDbCommand Command { get; set; }
        public string Message { get; set; }
    }


}
