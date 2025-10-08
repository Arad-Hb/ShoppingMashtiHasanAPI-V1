using Application.Implementations;
using ApplicationServiceContract.Services;
using Catalogue.DomainServiceContract.Services;
using Infrastructure.Catalogue.EF.Persistence.Repositories;
using Infrastructure.Catalogue.EF.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.BootStrap
{
    public static class BootStrap
    {
        public static void WireUpCatalogue(this IServiceCollection services,string ConnectionString)
        {
            services.AddDbContext<CatalogueContext>(optionAction => {
                optionAction.UseSqlServer(ConnectionString);
            }
            , ServiceLifetime.Scoped);

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryApplicationServiceContract, CategoryApplication>();
        }
    }
}
