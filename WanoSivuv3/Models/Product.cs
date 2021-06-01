using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;//new include
using System.Linq;
using System.Threading.Tasks;

namespace WanoSivuv3.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage ="Please enter a name.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Price.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please enter a Description.")]
        [Display(Name = "Description")]
        public string Desc { get; set; }

        [Required(ErrorMessage = "Please insert a Image.")]
        [Display(Name ="Images")]
        public string Image { get; set; }
        
        public Category Category { get; set; }
    }
}
