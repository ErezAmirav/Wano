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
      
        public string Name { get; set; }
        
        public double Price { get; set; }
        
        public string Desc { get; set; }
        
        public string Image { get; set; }
        
        public Category Category { get; set; }
    }
}
