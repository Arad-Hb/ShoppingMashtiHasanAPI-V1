
using Application.Implementations;
using ApplicationServiceContract.Services;
using Catalogue.DomainServiceContract.Services;
using Infrastructure.Catalogue.EF.Persistence;
using Infrastructure.Catalogue.EF.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ShoppingMashtiHasan
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            string CatalogueConnectionString = builder.Configuration["CatalogueConnectionString"].ToString();
            builder.Services.AddDbContext<CatalogueContext>(optionAction =>
            {
                optionAction.UseSqlServer(CatalogueConnectionString);
            }
            , ServiceLifetime.Scoped);

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryApplicationServiceContract, CategoryApplication>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
