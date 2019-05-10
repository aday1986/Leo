using System.Collections.Generic;

namespace Leo.Native.Commands
{
    public class CommandActionList : Dictionary<string, CommandAction>, ICommandActionList
    {
        public CommandActionList()
        {
         
                this.Add("测试",
                    new CommandAction()
                    {
                        CommandName = "测试",
                        Func = (cmd) => { return $"[{cmd.QQId}]拥有在群[{cmd.GroupId}]发送命令[{cmd.CommandName}]的权限。"; }
                    });

               this.Add("测试1",
                   new CommandAction()
                   {
                       CommandName = "测试1",
                       Func = (cmd) => { return $"[{cmd.QQId}]拥有在群[{cmd.GroupId}]发送命令[{cmd.CommandName}]的权限。"; }
                   });

            this.Add("测试2",
                 new CommandAction()
                 {
                     CommandName = "测试2",
                     Func = (cmd) => { return $"{cmd.CommandName}命令不需要权限就可以被调用。"; },
                     NeedValidation = false
                     
                 });


        }
    }
}
