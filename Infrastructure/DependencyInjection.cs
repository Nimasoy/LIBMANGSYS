using Infrastructure.Data;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Infrastructure.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Polly;
using Polly.Retry;
using Polly.RateLimiting;
using Microsoft.Data.SqlClient;
using Polly.Timeout;
using System.Threading.RateLimiting;


namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ILendingRepository, LendingRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddResiliencePipeline("read-pipeline", builder =>
            {
                builder
                    .AddTimeout(TimeSpan.FromSeconds(3))
                    .AddRetry(new RetryStrategyOptions
                    {
                    ShouldHandle = new PredicateBuilder()
                        .Handle<SqlException>()
                        .Handle<DbUpdateException>()
                        .Handle<TimeoutRejectedException>(),
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Exponential,
                    Delay = TimeSpan.FromSeconds(2),
                    UseJitter = true
                });
            });

            services.AddResiliencePipeline("write-pipeline", builder =>
            {
                builder
                .AddTimeout(TimeSpan.FromSeconds(5))
                .AddRetry(new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder()
                        .Handle<SqlException>()
                        .Handle<DbUpdateException>()
                        .Handle<TimeoutRejectedException>(),
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Exponential,
                    Delay = TimeSpan.FromSeconds(3),
                    UseJitter = true,
                    OnRetry = args =>
                    {
                        Console.WriteLine($"[WRITE RETRY] Attempt #{args.AttemptNumber}");
                        return default;
                    }
                });
            });

            services.AddResiliencePipeline("reserve-pipeline", builder =>
            {
                builder
                    .AddRateLimiter(new SlidingWindowRateLimiter(
                        new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            SegmentsPerWindow = 4,
                            Window = TimeSpan.FromMinutes(1)
                        })
                );
            });
            return services;
        }
    }
} 