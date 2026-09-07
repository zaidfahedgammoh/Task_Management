using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task_Management.Application.Interfaces;
using Task_Management.Domain;
using Task_Management.Infrastructure;
using Task_Management.Infrastructure.Repositories;
using Task_Management.Infrastructure.Services;


namespace Task_Management.Infrastructure;

public static class DependencyInjection
{
    
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TaskDbContext>(options =>
    options
        .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        .UseSeeding((context, serviceProvider) =>
        {
            var passwordHasher = new PasswordHasher();

            if (!context.Set<User>().Any())
            {
                context.Set<User>().AddRange(
                    new User
                    {
                        username = "ttl",
                        email = "ttl@test.com",
                        password = passwordHasher.Hash("Password123!"),
                        role = UserRoles.TTL
                    },
                    new User
                    {
                        username = "employee",
                        email = "employee@test.com",
                        password = passwordHasher.Hash("Password123!"),
                        role = UserRoles.Employee
                    },
                    new User
                    {
                        username = "manager",
                        email = "manager@test.com",
                        password = passwordHasher.Hash("Password123!"),
                        role = UserRoles.Manager
                    }
                );

                context.SaveChanges();
            }
        })
        .UseAsyncSeeding(async (context, serviceProvider, cancellationToken) =>
        {
            var passwordHasher = new PasswordHasher();

            if (!await context.Set<User>().AnyAsync(cancellationToken))
            {
                context.Set<User>().AddRange(
                    new User
                    {
                        username = "ttl",
                        email = "ttl@test.com",
                        password = passwordHasher.Hash("Password123!"),
                        role = UserRoles.TTL
                    },
                    new User
                    {
                        username = "employee",
                        email = "employee@test.com",
                        password = passwordHasher.Hash("Password123!"),
                        role = UserRoles.Employee
                    },
                    new User
                    {
                        username = "manager",
                        email = "manager@test.com",
                        password = passwordHasher.Hash("Password123!"),
                        role = UserRoles.Manager
                    }
                );

                await context.SaveChangesAsync(cancellationToken);
            }
        }));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }
}