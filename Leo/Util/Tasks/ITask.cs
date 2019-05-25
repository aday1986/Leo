namespace Leo.Util.Tasks
{
    /// <summary>
    /// 表示一组任务。
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 唯一名称，主键。
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// 执行。
        /// </summary>
        bool Execute();

    }



}



