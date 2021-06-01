using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WanoSivuv3.Models;

namespace WanoSivuv3.Data
{
    public class WanoSivuv3Context : DbContext
    {
        public WanoSivuv3Context (DbContextOptions<WanoSivuv3Context> options)
            : base(options)
        {
        }

        public DbSet<WanoSivuv3.Models.Product> Product { get; set; }

        public DbSet<WanoSivuv3.Models.User> User { get; set; }

        public DbSet<WanoSivuv3.Models.Category> Category { get; set; }
    }
}
