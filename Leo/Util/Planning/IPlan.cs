namespace Leo.Util.Planning
{
    /// <summary>
    /// 计划类接口。
    /// </summary>
    public interface IPlan
    {
        string Name { get; set; }

        string Description { get; set; }

        /// <summary>
        /// 计划已执行的进度。
        /// </summary>
        int ExecutedTimes { get; }

        PlanState PlanState { get; }

        void Start();

        void Pause();

        void Stop();

        /// <summary>
        /// 计划进度进行之前，用于注册进度中执行的任务。
        /// </summary>
        event EventHandler<ProgressStep> BeforeProgressRun;

        event EventHandler<PlanState> PlanStateChanged;

    }



}



