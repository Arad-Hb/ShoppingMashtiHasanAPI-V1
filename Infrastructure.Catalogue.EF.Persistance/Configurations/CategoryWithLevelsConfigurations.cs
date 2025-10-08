using Catalogue.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Catalogue.EF.Persistence.Configurations
{
    public class CategoryWithLevelsConfigurations : IEntityTypeConfiguration<CategoryWithLevels>
    {
        public void Configure(EntityTypeBuilder<CategoryWithLevels> builder)
        {
            builder.HasNoKey();
        }
    }
}
