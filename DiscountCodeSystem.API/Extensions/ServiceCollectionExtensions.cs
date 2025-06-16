using DiscountCodeSystem.Application.interfaces;
using DiscountCodeSystem.Application.services;
using DiscountCodeSystem.Infrastructure.Data;
using DiscountCodeSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DiscountCodeSystem.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register gRPC service
            services.AddGrpc();

            // Register DbContext
            services.AddDbContext<DiscountDbContext>(opt =>
                opt.UseSqlite("Data Source=discounts.db"));

            // Scoped dependencies
            services.AddScoped<IDiscountCodeDbContext>(provider =>
                provider.GetRequiredService<DiscountDbContext>());

            services.AddScoped<IDiscountCodeRepository, DiscountCodeRepository>();
            services.AddScoped<IDiscountCodeService, DiscountCodeService>();

            return services;
        }
    }
}
