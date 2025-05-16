using Microsoft.IdentityModel.Tokens;
using Softy_Pinko.Services.Abstraction.Storage;
using Softy_Pinko.Services.Concreate.Storage;
using Softy_Pinko.Services.Concreate.Storage.Local;

namespace Softy_Pinko.Services
{
    public static class ServiceRegistrations
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
            services.AddStorage<LocalStorage>();
        }
        private static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
        {
            services.AddScoped<IStorage, T>();
        }
    }
}
