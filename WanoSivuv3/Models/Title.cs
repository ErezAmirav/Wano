﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WanoSivuv3.Models
{
    public class Title
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> myProducts { get; set; }
    }
}
