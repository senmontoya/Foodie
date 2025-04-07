﻿using Foodie.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Foodie.Models
{
    public class FoodieContext : DbContext
    {
        public FoodieContext(DbContextOptions<FoodieContext> options) : base(options)
        {

        }

        public DbSet<Login_Cliente> Login_Cliente { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Platos> Platos { get; set; }

    }
}
