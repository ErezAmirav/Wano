using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;//new include
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WanoSivuv3.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage ="Please enter a name.")]
        [StringLength(100)]
        [RegularExpression("^[A-Z]+[a-zA-Z]*$", ErrorMessage = "Invalid name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Price.")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please enter a Description.")]
        [Display(Name = "Description")]
        public string Desc { get; set; }

        [Required(ErrorMessage = "Please insert a Image.")]
        [Display(Name ="Images")]
        //[DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        public Category Category { get; set; }
    }
}
