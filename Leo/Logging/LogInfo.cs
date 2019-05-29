using Leo.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leo.Logging
{   //独立的日志数据库，不继承IEntity
    public class LogInfo
    {
        [Key]
        [Column(IsPrimaryKey =true, IsIdentity =true)]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
    }

  

}
