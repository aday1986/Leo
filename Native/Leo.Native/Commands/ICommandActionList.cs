using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Native.Commands
{
    public interface ICommandActionList:IDictionary<string, CommandAction>
    {
    }
}
