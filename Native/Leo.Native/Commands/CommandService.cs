using Leo.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leo.Native.Commands
{

    public class CommandService : ICommandService
    {
      
        private readonly IRepository<CommandPower> repository;
        private readonly ICommandActionList commandActions;

        public CommandService(IRepository<CommandPower> repository,ICommandActionList commandActions)
        {
            this.repository = repository;
            this.commandActions = commandActions;
        }

        public bool AddCommandPower(CommandPower commandPower)
        {
            repository.Add(commandPower);
            return repository.SaveChanges() > 0;
        }

        public bool HasPower(Command command)
        {
            var conditions = new List<Condition>();
            conditions.Add(new Condition() { Key = nameof(CommandPower.GroupId), Value = command.GroupId, ConditionType = ConditionEnum.Equal });
            conditions.Add(new Condition() { Key = nameof(CommandPower.QQId), Value = command.QQId, ConditionType = ConditionEnum.Equal });
            conditions.Add(new Condition() { Key = nameof(CommandPower.CommandName), Value = command.CommandName, ConditionType = ConditionEnum.Equal });
            return repository.Query(conditions).Any();
        }

        public IEnumerable<CommandPower> GetAll()
        {
            return repository.Query(null);
        }

        public IEnumerable<CommandPower> GetCommandPowers(long groupId, long qqId)
        {
            var conditions = new List<Condition>();
            conditions.Add(new Condition() { Key = nameof(CommandPower.GroupId), Value = groupId, ConditionType = ConditionEnum.Equal });
            conditions.Add(new Condition() { Key = nameof(CommandPower.QQId), Value = qqId, ConditionType = ConditionEnum.Equal });
            return repository.Query(conditions);
        }

        public bool RemoveCommandPower(CommandPower commandPower)
        {
            repository.Remove(commandPower);
            return repository.SaveChanges() > 0;
        }



        public bool Excuter(Command command, out string message)
        {
            bool result = false;
            if (commandActions.TryGetValue(command.CommandName, out CommandAction action))
            {
                if (!action.NeedValidation || HasPower(command))
                {
                    try
                    {
                        message = action.Func?.Invoke(command);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        result = false;
                    }
                }
                else
                {
                    message = $"您没有调用[{command.CommandName}]的权限。";
                    result = false;
                }
            }
            else
            {
                message = $"没有[{command.CommandName}]这个命令。";
            }
            return result;
        }

        public bool IsCommand(string message, out string commandName)
        {
            commandName = string.Empty;
            if (message.StartsWith(Command.Separator.ToString()))
            {
                commandName = message.Remove(0, 1).Split(Command.Separator).FirstOrDefault().Trim();
            }
            return !string.IsNullOrEmpty(commandName);
        }
    }
}
