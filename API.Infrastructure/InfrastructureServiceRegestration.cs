using API.Application.Abstraction.Services.Producer;
using API.Infrastructure.Services.Producer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure
{
    public static class InfrastructureServiceRegister
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IProducerService, ProducerService>();
        }
    }
}
