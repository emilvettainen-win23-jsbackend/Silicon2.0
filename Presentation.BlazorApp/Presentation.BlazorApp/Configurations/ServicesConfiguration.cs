
using CurrieTechnologies.Razor.SweetAlert2;

using Infrastructure.Services;
using Presentation.BlazorApp.GraphQL;



namespace Presentation.BlazorApp.Configurations;

public static class ServicesConfiguration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        services.AddScoped<AddressService>();
        services.AddScoped<CourseService>();
        services.AddScoped<DarkModeService>();

        services.AddSingleton<GraphQLService>();
 

        services.AddSweetAlert2();
        services.AddBlazorBootstrap();


    }
}
