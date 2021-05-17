using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WanoSivuv3.Models
{
    public enum UserType
    {
        Client,
        Admin
    }
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public UserType Type { get; set; } = UserType.Client;
    }
}