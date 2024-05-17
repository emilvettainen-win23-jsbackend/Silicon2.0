using Infrastructure.Services;

namespace Presentation.BlazorApp.Configurations;

public static class ServicesConfiguration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddScoped<AddressService>();
        services.AddScoped<CourseService>();
        services.AddScoped<DarkModeService>();
    }
}
