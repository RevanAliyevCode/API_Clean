using System.Reflection;

namespace API.Presentation.Reflection
{
    public static class ServiceRegestrationReflection
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var infrastructureService = Assembly.Load("API.Infrastructure");
            var persistanceService = Assembly.Load("API.Persistance");

            services.RegisterServicesFromAssembly(persistanceService, configuration);
            services.RegisterServicesFromAssembly(infrastructureService);
        }

        private static void RegisterServicesFromAssembly (this IServiceCollection services, Assembly assembly, IConfiguration? configuration = null)
        {
            var registrationClasses = assembly.GetTypes()
           .Where(t => t.IsClass && t.IsAbstract && t.IsSealed)
           .Where(t => t.Name.EndsWith("ServiceRegistration"));

            foreach (var registrationClass in registrationClasses)
            {
                var methods = registrationClass.GetMethods(BindingFlags.Public | BindingFlags.Static);

                foreach (var method in methods)
                {
                    var parameters = method.GetParameters();

                    if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IServiceCollection))
                    {
                        method.Invoke(null, new object[] { services });
                    }
                    else if (parameters.Length == 2 && parameters[0].ParameterType == typeof(IServiceCollection) &&
                             parameters[1].ParameterType == typeof(string) && configuration != null)
                    {
                        var connectionString = configuration.GetConnectionString("DefaultConnection");
                        method.Invoke(null, new object[] { services, connectionString });
                    }
                }
            }
        }
    }
}
