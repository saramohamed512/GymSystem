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
    internal class MemberSessionConfigration : IEntityTypeConfiguration<Entities.MemberSession>
    {

        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {

            builder.Ignore(X => X.Id);
            builder.HasKey(X => new { X.MemberId, X.SessionId });
            builder.Property(X => X.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GETDATE()");


        }
    }
}
