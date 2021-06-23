using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WanoSivuv3.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Number { get; set; }
        [Display(Name = "Username")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
