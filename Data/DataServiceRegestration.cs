using System;
using Data.Repositeries.Product;
using U = Data.UnitOfWork;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DataServiceRegestration
{
    public static void RegisterDataServices(this IServiceCollection services, string conectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(conectionString);
        });

        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
        services.AddScoped<U.IUnitOfWork, U.UnitOfWork>();
    }
}
