using AlgimedWPFApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedWPFApp
{
    public class DBContext : DbContext, IDisposable
    {
        public DbSet<Mode> Modes { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AlgimedApp.db");
            Directory.CreateDirectory(Path.GetDirectoryName(databasePath));

            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }

        public override void Dispose() => base.Dispose();
    }
}
