using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Context
{
    public class GymSystemDBContext : DbContext
    {
        public GymSystemDBContext(DbContextOptions<GymSystemDBContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=.;Database=GymSystem;Trusted_Connection=True;TrustServerCertificate=True;");
        //    }
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Entities.Category> Categories { get; set; }
        public DbSet<Entities.Member> Members { get; set; }
        public DbSet<Entities.Plan> Plans { get; set; }
        public DbSet<Entities.Membership> Memberships { get; set; }
        public DbSet<Entities.Trainer> Trainers { get; set; }
        public DbSet<Entities.Session> Sessions { get; set; }
        public DbSet<Entities.MemberSession> MemberSessions { get; set; }
        public DbSet<Entities.HealthRecord> HealthRecords { get; set; }
        //dowlnload pacakgae of entityFramwork.Tools in the Pl Project
        //make sure the startup project is PL
        //make defoult project from package mangaer console is DAL
        //Add-Migration "IntialCreate" -OutputDir "Data/Migrations" -Project GymSystemDAL -StartupProject GymSystemPL
    }
}
