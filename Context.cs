using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject_02_EF_AssetTracking
{
    class Context : DbContext
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Office> Offices { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EF_AssetTracking;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
            optionsBuilder.UseSqlServer(conn);
        }
    }
}
