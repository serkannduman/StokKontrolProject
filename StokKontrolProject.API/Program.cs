using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Repository.Abstract;
using StokKontrolProject.Repository.Concrete;
using StokKontrolProject.Repository.Context;
using StokKontrolProject.Service.Abstract;
using StokKontrolProject.Service.Concrete;
using Newtonsoft.Json;

namespace StokKontrolProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StokKontrolContext>(option =>
            {
                option.UseSqlServer("server=DESKTOP-PMQ8OOE\\MSSQLSERVER2019;database=StockDB;Trusted_Connection=True;");
            });

            builder.Services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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