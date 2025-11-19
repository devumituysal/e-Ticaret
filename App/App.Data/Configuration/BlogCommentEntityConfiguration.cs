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
    internal class BlogCommentEntityConfiguration : IEntityTypeConfiguration<BlogCommentEntity>
    {
        public void Configure(EntityTypeBuilder<BlogCommentEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.BlogId).IsRequired();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Comment).IsRequired();
            builder.Property(e => e.IsApproved).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasOne(e => e.Blog)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.BlogId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
