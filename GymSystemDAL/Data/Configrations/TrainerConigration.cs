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
    internal class TrainerConigration : GymUserConfigration<Trainer>, IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            //Special Type HireDate
            builder.Property(X => X.CreatedAt)
                .HasColumnName("HireDate")
                .HasDefaultValueSql("GETDATE()");

            base.Configure(builder);

        }
    }
}
