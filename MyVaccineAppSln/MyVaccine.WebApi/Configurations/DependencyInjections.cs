using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Repositories.Implementations;

namespace MyVaccine.WebApi.Configurations;

public static class DependencyInjections
{
    public static IServiceCollection SetDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository> ();
        return services;
    }
}
