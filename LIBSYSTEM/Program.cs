using LIBSYSTEM.Endpoints;
using Serilog;
using Infrastructure;
using Application;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
namespace LIBSYSTEM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOpenApi();
            builder.Services.AddIdentityCore<User>().AddEntityFrameworkStores<LibraryDbContext>();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


            builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();  
            }

            app.MapAllEndpoints();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}
