using System;
using System.Reflection;
using Business.Services.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace Business;

public static class BusinessServiceRegister
{
    public static void RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddScoped<IProducerService, ProducerService>();
    }
}
