using ComplaintSystem.Application.Interfaces;
using ComplaintSystem.Application.Mappings;
using ComplaintSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ComplaintSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IComplaintService, ComplaintService>();
        services.AddScoped<ILookupService, LookupService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IRagDuplicateCheckService, RagDuplicateCheckService>();

        return services;
    }
}
