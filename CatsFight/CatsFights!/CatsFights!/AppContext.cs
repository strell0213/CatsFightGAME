﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CatsFights_
{
    internal class AppContext : DbContext
    {
      
        public DbSet<User> Users { get; set; }

        public DbSet<Enemy> Enemies { get; set; }
        public AppContext() : base("DefaultConnection") { }
    }
}
