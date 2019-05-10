using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leo.Data;

namespace Native.Csharp.App.Model
{
    public class GroupMessage
    {
        public DateTime MsgDate { get; set; }
        /// <summary>
		/// 来源QQ
		/// </summary>
		public long FromQQ { get; set; }

        /// <summary>
		/// 消息Id
		/// </summary>
        [Column(IsPrimaryKey = true)]
        public int MsgId { get; set; }

        /// <summary>
        /// 来源群号
        /// </summary>
        public long FromGroup { get; set; }

        /// <summary>
        /// 是否是匿名消息
        /// </summary>
        public bool IsAnonymousMsg { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg { get; set; }


    }
}
