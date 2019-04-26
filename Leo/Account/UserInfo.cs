using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leo.Account
{
   public class UserInfo
    {
        [Key]
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set;}
    }
}
