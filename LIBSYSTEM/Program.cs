using Application;
using Application.Services;
using Application.Interfaces;
using Application.Validators.Books;
using Infrastructure;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using LIBSYSTEM.Middleware;
using FluentValidation;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace LIBSYSTEM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOpenApi();
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddValidatorsFromAssemblyContaining<AddBookCommandValidator>();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["Redis:Connection"];
            });
            builder.Services.AddScoped<ICacheService, RedisCacheService>();

            builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "LIBSYSTEM API", Version = "v1" });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Enter you token please: ",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                { jwtSecurityScheme, Array.Empty<string>() }
                });
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Librarian", policy =>
                    policy.RequireRole("Librarian"));
            });
                

            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();  
            }

            app.MapAllEndpoints();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.Run();
        }
    }
}
