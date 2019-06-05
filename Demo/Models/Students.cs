using Leo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
   public class Students
    {
        [Column(IsPrimaryKey =true,IsIdentity =true)]
        public int Id { get; set; }
        [Column(ColumnName ="Name")]
        public string StudentName { get; set; }
        public DateTime CreateDate { get; set; }
        public StudentEnum StudentEnum { get; set; }
    }

    public enum StudentEnum
    {
        e1,
        e2,
        e3
    }
}
