using Leo.Data;

namespace Leo.Native.Commands
{
    /// <summary>
    /// 命令权限。
    /// </summary>
    public class CommandPower
    {
        [Column(IsPrimaryKey =true)]
        public long GroupId { get; set; }

        [Column(IsPrimaryKey = true)]
        public long QQId { get; set; }

        [Column(IsPrimaryKey = true)]
        public string CommandName { get; set; }
    }
}
