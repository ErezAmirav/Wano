using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WanoSivuv3.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }
        public Title Title { get; set; }
    }
}
