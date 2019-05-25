namespace Leo.Util.Planning
{
    /// <summary>
    /// 计划基类。
    /// </summary>
    public abstract class PlanBase : IPlan
    {
        public int ExecutedTimes { get; protected set; }
        public PlanState PlanState { get; protected set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public abstract event EventHandler<ProgressStep> BeforeProgressRun;
        public event EventHandler<PlanState> PlanStateChanged;

        public abstract void Pause();
        public abstract void Start();
        public abstract void Stop();

        protected void SetPlanState(PlanState planState)
        {
            if (PlanState != planState)
            {
                lock (this)
                {
                    PlanState = planState;
                    PlanStateChanged?.Invoke(this, planState);
                }
            }
        }
    }



}



