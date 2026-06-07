
using Microsoft.EntityFrameworkCore;
using NeromylosSuites.Security;
using Serilog;

namespace NeromylosSuites
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((hostingContext, configuration) =>
            {
                configuration.ReadFrom.Configuration(hostingContext.Configuration);
            });

            var connString = builder.Configuration.GetConnectionString("DevConnection");

            builder.Services.AddDbContext<Data.NeromylosSuitesMvcContext>(options =>
                    options.UseSqlServer(connString));

            // Add services to the container.

            builder.Services.AddSingleton<IEncryptionUtil, EncryptionUtil>();

            // Add repositories

            // Add Automapper

            // JWT

            // Cors

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            // AddEndpointsApiExplorer

            // Swagger

            // AddExceptionHandler

            // AddAuthorization

            var app = builder.Build();

            // app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            //app.UseCors("AllowClient");
            //app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
