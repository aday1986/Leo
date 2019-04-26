using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Logging
{
    public interface ILogService
    {
        void AddAsync(LogInfo log);
    }
}
