using Blazored.Toast;
using CurrieTechnologies.Razor.SweetAlert2;

using Infrastructure.Services;
using Presentation.BlazorApp.GraphQL;
using Presentation.BlazorApp.Helper;


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
        services.AddSingleton<MessageService>();

        services.AddSweetAlert2();
       


    }
}
