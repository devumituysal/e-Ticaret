using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Configuration
{
    internal class ContactFormEntityConfiguration : IEntityTypeConfiguration<ContactFormEntity>
    {
        public void Configure(EntityTypeBuilder<ContactFormEntity> builder)
        {
            builder.HasKey(cf => cf.Id);
            builder.Property(cf => cf.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(cf => cf.Email) 
                .IsRequired()
                .HasMaxLength(256);
            builder.Property(cf => cf.Message)
                .IsRequired() 
                .HasMaxLength(1000);
            builder.Property(cf => cf.CreatedAt)
                .IsRequired();
        }
    }
}
