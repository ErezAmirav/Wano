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
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression("^[A-Z]+[a-zA-Z]*$", ErrorMessage = "Invalid name")]
        public string Username { get; set; }
        //[Required]
        //[DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email address")]

        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public UserType Type { get; set; } = UserType.Client;
    }
}