using System;
using Business.Services.Product;
using Microsoft.Extensions.DependencyInjection;

namespace Business;

public static class BusinessServiceRegister
{
    public static void RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
    }
}
