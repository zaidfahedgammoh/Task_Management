using Microsoft.Extensions.DependencyInjection;
using Task_Management.Application.Services;

namespace Task_Management.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<UserService>();

        return services;
    }
}