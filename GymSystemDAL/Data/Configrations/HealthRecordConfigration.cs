using GymSystemDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Configrations
{
    public class HealthRecordConfigration : IEntityTypeConfiguration<Entities.HealthRecord>
    {

        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            // Configure decimal precision for Height and Weight
            builder.Property(x => x.Height)
                .HasPrecision(5, 2); // 5 total digits, 2 decimal places (e.g., 199.99)
            
            builder.Property(x => x.Weight)
                .HasPrecision(5, 2); // 5 total digits, 2 decimal places (e.g., 199.99)

            builder.ToTable("HealthRecords");
            builder.HasOne<Member>()
                .WithOne(X => X.HealthRecord)
                .HasForeignKey<HealthRecord>(X => X.Id);

            builder.Ignore(X => X.CreatedAt);
            builder.Ignore(X => X.UpdatedAt);
        }
    }
}
