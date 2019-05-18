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
   public delegate void ProgressRunningHandle(int value,int? maxValue,string message);

    public interface IPlan
    {
        void Start();

        void Pause();

        void Stop();

        int ExecutedTimes { get; }

        PlanState PlanState { get; }

        IEnumerable<ITask> Tasks { get; }

        event ProgressRunningHandle  ProgressRunning;

    }

    public enum PlanState
    {
        Running,
        Paused,
        Stoped
    }

    /// <summary>
    /// 次数计划。
    /// </summary>
    public class TimesPlan:IPlan
    {
        /// <summary>
        /// 重复执行的次数，为<see cref="null"/>时无限重复。
        /// </summary>
        int? Times { get; set; }

        /// <summary>
        /// 执行间隔。
        /// </summary>
        TimeSpan Interval { get; set; }

        #region IPlan
        public int ExecutedTimes { get; private set; }

        public PlanState PlanState { get; private set; }

        public IEnumerable<ITask> Tasks { get; }

        public event ProgressRunningHandle ProgressRunning;

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
        #endregion


    }

    /// <summary>
    /// 时间周期计划。
    /// </summary>
    public class CyclePlan:IPlan
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

        #region IPlan
        public int ExecutedTimes { get; private set; }

        public PlanState PlanState { get; private set; }

        public IEnumerable<ITask> Tasks { get; }

        public event ProgressRunningHandle ProgressRunning;

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
        #endregion

    }

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
        /// 序号，优先级（越小越优先）。
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// 执行。
        /// </summary>
        void Execute();

    }



    public class MethodTask : ITask
    {
        private readonly string methodBase64;
        private readonly object[] args;

        public MethodTask(string methodBase64, object[] args)
        {

            this.methodBase64 = methodBase64;
            this.args = args;
        }

        public MethodTask(MethodInfo method, object[] args)
        {
            methodBase64 = ToBase64(method);
            this.args = args;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Index { get; set; }

        public void Execute()
        {
          Task.Run(() =>
                 {
                     MethodInfo method = null;
                     try
                     {
                         method = ToMethodInfo(methodBase64);
                     }
                     catch (Exception ex)
                     {
                         throw new Exception($"反序列化MethodInfo失败：\n{ex.Message}");
                     }
                     method.Invoke(null, args);
                 });
        }



        public static string ToBase64(MethodInfo method) => Converter.SerializeBase64(method);

        public static MethodInfo ToMethodInfo(string base64) => Converter.DeserializeBase64<MethodInfo>(base64);

    }

   

}



