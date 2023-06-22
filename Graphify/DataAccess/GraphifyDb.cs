using Entity.DBM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class GraphifyDb : DbContext
    {
        public GraphifyDb()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=ELIZZ;database=GraphifyDB;trusted_connection=true;TrustServerCertificate=True");
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcedureResult>().HasNoKey();
        }

        public DbSet<ProcedureResult> ProcedureResults { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<WorkpageTemplate> WorkpageTemplates { get; set; }
    }
}
