﻿using GymSystemDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Configrations
{
    internal class HealthRecordConfigration : IEntityTypeConfiguration<Entities.HealthRecord>
    {

        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {

            builder.ToTable("Member");
            builder.HasOne<Member>()
                .WithOne(X => X.HealthRecord)
                .HasForeignKey<HealthRecord>(X => X.Id);
        }
    }
}
