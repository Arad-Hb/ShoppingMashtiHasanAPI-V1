using Catalogue.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Catalogue.EF.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.CategoryID);
            builder.Property(x => x.PageTitle).HasMaxLength(120).IsRequired(false);
            builder.Property(x => x.Lineage).HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.CategoryDescription).HasMaxLength(4000);
            builder.Property(x => x.BodyExtraData).HasMaxLength(4000).IsRequired(false);
            builder.Property(x => x.Depth).IsRequired(false).HasDefaultValue(0);
            builder.Property(x => x.HeadExtraData).IsRequired(false);
            builder.Property(x => x.MetaDescription).IsRequired(false);
            builder.Property(x => x.MetaTag).IsRequired(false);
            builder.Property(x => x.Slug).IsRequired(false);
            
            builder.HasMany(x=>x.Products).WithOne(x => x.Category)
                .HasForeignKey(x=>x.CategoryID).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x=>x.Children).WithOne(x=>x.Parent)
                .HasForeignKey(x=>x.ParentID).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.CategoryFeatures)
                .WithOne(x => x.Category).HasForeignKey(x => x.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
