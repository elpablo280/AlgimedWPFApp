using AlgimedWPFApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedWPFApp
{
    public class DBContext : DbContext
    {
        public DbSet<Mode> Modes { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=AlgimedApp.db");
        }
    }
}
