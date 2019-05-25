using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Leo.Util.Planning
{

    /// <summary>
    /// 时间周期计划。
    /// </summary>
    public class CyclePlan : PlanBase, IPlan
    {
        /// <summary>
        /// 开始日期。
        /// </summary>
        DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期。
        /// </summary>
        DateTime EndDate { get; set; }

        /// <summary>
        /// 开始时间。
        /// </summary>
        DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间。
        /// </summary>
        DateTime EndTime { get; set; }

        public override event EventHandler<ProgressStep> BeforeProgressRun;

        public override void Pause()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }



}



