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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x=>x.ProductName).IsRequired().HasMaxLength(200);
            builder.Property(x=>x.PageTitle).HasMaxLength(120);
            builder.HasOne(x => x.Supplier).WithMany(x => x.Products).HasForeignKey(x => x.SupplierID).OnDelete(DeleteBehavior.NoAction);
            
            builder.Property(x => x.Slug).IsRequired(false);
            builder.Property(x => x.BodyExtraData).IsRequired(false);
            builder.Property(x => x.PageTitle).IsRequired(false);
            builder.Property(x => x.MetaTag).IsRequired(false);
            builder.Property(x => x.MetaDescription).IsRequired(false);
            builder.Property(x => x.HeadExtraData).IsRequired(false);
            

        //     public int ProductID { get; set; }
        //public string Slug { get; set; }
        //public string PageTitle { get; set; }
        //public string MetaTag { get; set; }
        //public string MetaDescription { get; set; }
        //public string HeadExtraData { get; set; }
        //public string BodyExtraData { get; set; }


    }
    }
}
