using System;
using System.Text;

namespace Leo.Data
{
    /// <summary>
    /// 条件实体。
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// 字段名称。
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值。
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 条件类型。
        /// </summary>
        public ConditionEnum ConditionType { get; set; }
    }
    /// <summary>
    /// 条件类型枚举。
    /// </summary>
    public enum ConditionEnum
    {
        /// <summary>
        /// 匹配。
        /// </summary>
        Like,
        /// <summary>
        /// 等于。
        /// </summary>
        Equal,
        /// <summary>
        /// 不等于。
        /// </summary>
        NotEqual,
        /// <summary>
        /// 大于。
        /// </summary>
        Greater,
        /// <summary>
        /// 大于或等于。
        /// </summary>
        GreaterEqual,
        /// <summary>
        /// 小于。
        /// </summary>
        Less,
        /// <summary>
        /// 小于或等于。
        /// </summary>
        LessEqual
    }

 
}
