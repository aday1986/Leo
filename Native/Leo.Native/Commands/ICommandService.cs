using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Native.Commands
{
    public interface ICommandService
    {
        
        /// <summary>
        /// 获取所有命令权限。
        /// </summary>
        /// <returns></returns>
        IEnumerable<CommandPower> GetAll();
        /// <summary>
        /// 添加命令权限。
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="qqId"></param>
        /// <returns></returns>
        IEnumerable<CommandPower> GetCommandPowers(long groupId, long qqId);
        bool AddCommandPower(CommandPower command);
        /// <summary>
        /// 移除命令权限。
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool RemoveCommandPower(CommandPower command);
        /// <summary>
        /// 是否拥有权限。
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool HasPower(Command command);

        /// <summary>
        /// 执行命令。
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool Excuter(Command command, out string message);

        bool IsCommand(string message,out string commandName);

   
    }
}
