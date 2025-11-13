using Application.Abstractions.Behaviors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class ApplicationDependencyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddMediatR(services);
    }

    private static void AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(ApplicationDependencyInjection).Assembly);

            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));

            //configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));

            configuration.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
        });
    }
}
