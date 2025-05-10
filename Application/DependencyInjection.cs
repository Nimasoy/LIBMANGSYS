using MediatR;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Commands;
using Application.Services;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            services.AddScoped<UserService>();
            services.AddScoped<BookService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<TagService>();

            return services;
        }
    }
}
       