using ComplaintSystem.Application.Interfaces;
using ComplaintSystem.Domain.Entities;
using ComplaintSystem.Domain.Interfaces;
using ComplaintSystem.Infrastructure.Files;
using ComplaintSystem.Infrastructure.Identity;
using ComplaintSystem.Infrastructure.Persistence;
using ComplaintSystem.Infrastructure.Rag;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ComplaintSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.Configure<OpenAiSettings>(configuration.GetSection("OpenAI"));

        services.AddHttpContextAccessor();
        services.AddHttpClient<IEmbeddingProvider, OpenAiEmbeddingProvider>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        return services;
    }
}
