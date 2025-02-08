using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace API.Presentation.Reflection
{
    public static class DbInitializerReflection
    {
        public static async Task InitializeIdentityDataAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var serviceProviderScoped = scope.ServiceProvider;

            var userManager = serviceProviderScoped.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProviderScoped.GetRequiredService<RoleManager<IdentityRole>>();

            var dbInitializerType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == "DbInitializer");

            if (dbInitializerType != null)
            {
                var seedMethod = dbInitializerType.GetMethod("SeedAsync", BindingFlags.Public | BindingFlags.Static);
                if (seedMethod != null)
                {
                    var task = (Task)seedMethod.Invoke(null, new object[] { userManager, roleManager });
                    await task;
                }
            }
        }
    }
}
