using FluentValidation;
using GymManagement.Application.Common.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
            options.AddOpenBehavior(typeof(ValdiationBehaviour<,>));
        });
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return services;
    }
}