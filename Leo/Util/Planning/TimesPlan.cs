using System;
using System.Threading;
using System.Threading.Tasks;

namespace Leo.Util.Planning
{
    /// <summary>
    /// 次数计划。
    /// </summary>
    public class TimesPlan : PlanBase, IPlan
    {
        public TimesPlan(int? times, TimeSpan interval)
        {
            Times = times;
            Interval = interval;
        }

        /// <summary>
        /// 重复执行的次数，为<see cref="null"/>时无限重复。
        /// </summary>
        int? Times { get; set; }

        /// <summary>
        /// 执行间隔。
        /// </summary>
        TimeSpan Interval { get; set; }

        public override event EventHandler<ProgressStep> BeforeProgressRun;

        public override void Pause()
        {
            lock (this)
            {
                switch (PlanState)
                {
                    case PlanState.None:
                        break;
                    case PlanState.Running:
                        SetPlanState(PlanState.Paused);
                        break;
                    case PlanState.Paused:
                        SetPlanState(PlanState.Running);
                        break;
                    case PlanState.Stoped:
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Start()
        {
            Task.Run(() =>
            {
                SetPlanState(PlanState.Running);
                while (true)
                {
                    switch (PlanState)//用switch逻辑条理清晰。
                    {
                        case PlanState.Running:
                            if (Times.HasValue && ExecutedTimes >= Times)
                                goto STOP;
                            var e = new ProgressStep()
                            {
                                MaxValue = Times,
                                Value = ExecutedTimes,
                                Message = string.Empty
                            };
                            Task.Run(() => { BeforeProgressRun?.Invoke(this, e); });
                            ExecutedTimes++;
                            break;
                        case PlanState.Paused:
                            break;
                        case PlanState.Stoped:
                            goto STOP;
                        default:
                            break;
                    }
                    Thread.Sleep(Interval);
                }
                STOP:
                SetPlanState(PlanState.Stoped);
            });
        }

        public override void Stop()
        {
            switch (PlanState)
            {
                case PlanState.None:
                    break;
                case PlanState.Running:
                    SetPlanState(PlanState.Stoped);
                    break;
                case PlanState.Paused:
                    SetPlanState(PlanState.Stoped);
                    break;
                case PlanState.Stoped:
                    break;
                default:
                    break;
            }
        }
    }



}



