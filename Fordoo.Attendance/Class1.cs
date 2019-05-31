using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fordoo.Attendance
{
   

    #region 输入

    /// <summary>
    /// 考勤记录。
    /// </summary>
    public class AttendanceRecord
    {
        /// <summary>
        /// 出勤人工号。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 出勤人姓名。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 出勤时间。
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 出勤类型。
        /// </summary>
        public AttendanceEnum AttendanceType { get; set; } = AttendanceEnum.Normal;
    }

    /// <summary>
    /// 考勤规则。
    /// </summary>
    public class AttendanceRule
    {
        /// <summary>
        /// 规则名称。
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// 每周每天应打卡规定时间。
        /// </summary>
        public Dictionary<DayOfWeek, List<DateTime>> TimesOfWeeks { get; set; }

        /// <summary>
        /// 例外时间，如节日等。
        /// </summary>
        public Dictionary<string, DateTime> ExceptionTimes { get; set; }

        /// <summary>
        /// 规定时间之前的有效范围。
        /// </summary>
        public TimeSpan BeforRange { get; set; }

        /// <summary>
        /// 规定时间之后的有效范围。
        /// </summary>
        public TimeSpan AfterRange { get; set; }
    }

    public class AttendanceContext
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IEnumerable< AttendanceRecord> Records { get; set; }

        public AttendanceRule Rule { get; set; }
    }

    #endregion

    #region 输出

    /// <summary>
    /// 出勤信息。
    /// </summary>
    public class AttendanceInfo
    {
        /// <summary>
        /// 出勤计划时间和实际时间。
        /// </summary>
        public List<Tuple<DateTime, DateTime>> AttendanceTimes { get; set; }
    }

    #endregion


    public interface IAttendanceService
    {
        int AddRange(params AttendanceRecord[] records);

        IEnumerable<AttendanceRecord> GetRecords(Func<AttendanceRecord, bool> conditions);

        AttendanceInfo GetAttendanceInfo(AttendanceContext context);
    }

    public enum AttendanceEnum
    {
        /// <summary>
        /// 正常打卡。
        /// </summary>
        Normal,
        /// <summary>
        /// 请假。
        /// </summary>
        Leave,
        /// <summary>
        /// 外派，公出。
        /// </summary>
        Expatriate

    }

   
}
