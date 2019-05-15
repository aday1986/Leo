using Leo.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leo.Tests.NET461
{
   public class TestEntity
    {
        [Key]
        [Column(IsPrimaryKey =true)]
        public string Guid { get; set; }

        public DateTime CreateDate { get; set; }

        public int Num { get; set; }

        [Column(NoUpdate =true)]
        public string NoUpdate { get; set; }
    }
}
