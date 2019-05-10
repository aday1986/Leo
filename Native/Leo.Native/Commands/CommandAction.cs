using System;

namespace Leo.Native.Commands
{
    /// <summary>
    /// 命令的处理类。
    /// </summary>
    public class CommandAction
    {
        public string CommandName { get; set; }
        /// <summary>
        /// 是否需要验证权限。
        /// </summary>
        public bool NeedValidation { get; set; } = true;

        public Func<Command, string> Func { get; set; }
    }
}
