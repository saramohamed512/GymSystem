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
    internal class MembershipConfigration : IEntityTypeConfiguration<Membership>
    {

        public void Configure(EntityTypeBuilder<Membership> builder)
        {

            builder.Property(X => X.CreatedAt)
            .HasColumnType("StartDate")
            .HasDefaultValueSql("GETDATE()");


            builder.HasKey(X => new { X.MemberId, X.PlanId });

            builder.Ignore(X => X.Id);
        }
    }
}
