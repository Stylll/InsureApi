using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Insure.Core.Models;
using Insure.Data.Configurations;

namespace Insure.Data
{
    public class InsureDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }

        public InsureDbContext(DbContextOptions<InsureDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ItemConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
        }

    }
}
