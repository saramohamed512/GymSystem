using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GymSystemDAL.Data.Context
{
    public class GymSystemDBContextFactory : IDesignTimeDbContextFactory<GymSystemDBContext>
    {
        public GymSystemDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GymSystemDBContext>();
            
            // Use the same connection string as in appsettings.json
            optionsBuilder.UseSqlServer("Server=.;Database=GymSystem;Trusted_Connection=True;TrustServerCertificate=True;");
            
            return new GymSystemDBContext(optionsBuilder.Options);
        }
    }
}
