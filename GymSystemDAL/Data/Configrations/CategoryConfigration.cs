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
    internal class CategoryConfigration : IEntityTypeConfiguration<Entities.Category>
    {

        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.Property(X => X.CategoryName)
            .HasColumnType("varchar")
            .HasMaxLength(20);


        }
    }
}
