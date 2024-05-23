using Azure.Messaging.ServiceBus;

using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Infrastructure.Data.Contexts;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Address;
using Infrastructure.Repositories.Course;
using Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Presentation.BlazorApp;
using Presentation.BlazorApp.Components;
using Presentation.BlazorApp.Configurations;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);




        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();


        builder.Services.AddAuthentication()
        .AddGoogle(GoogleOptions =>
        {
            GoogleOptions.ClientId = "893316874513-a6a0bnpavt6tj1262db69jp4rpkasq4l.apps.googleusercontent.com";
            GoogleOptions.ClientSecret = "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";
        })
        .AddFacebook(FacebookOptions =>
        {
            FacebookOptions.AppId = "276230208847426";
            FacebookOptions.AppSecret = "fa8c8957ec6c242f81323a3453e31455";

        });


        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AzureDb")), ServiceLifetime.Singleton);
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;

        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();


        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/account/signin";
            options.LogoutPath = "/signout";
            options.AccessDeniedPath = "/denied";

            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.SlidingExpiration = true;

        });






        builder.Services.RegisterServices();
        builder.Services.RegisterRepositories();

        //builder.Services.AddScoped<UserService>();
        //builder.Services.AddScoped<AddressService>();
        //builder.Services.AddScoped<AddressRepository>();
        //builder.Services.AddScoped<UserAddressRepository>();
        //builder.Services.AddScoped<OptionalAddressRepository>();

        //builder.Services.AddScoped<CourseService>();
        //builder.Services.AddScoped<SavedCourseRepository>();

        builder.Services.AddHttpClient();
        builder.Services.AddSingleton(new ServiceBusClient(builder.Configuration.GetConnectionString("ServiceBusConnection")));

      

      



        //builder.Services.AddScoped<DarkModeService>();



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", "?statusCode={0}");

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Presentation.BlazorApp.Client._Imports).Assembly);
        await app.Services.RegisterRoles();

        app.MapAdditionalIdentityEndpoints();

        app.Run();
    }
}