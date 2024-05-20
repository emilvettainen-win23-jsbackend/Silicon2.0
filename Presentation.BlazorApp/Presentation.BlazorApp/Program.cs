using Azure.Messaging.ServiceBus;
using CurrieTechnologies.Razor.SweetAlert2;
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
        builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();


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
            options.LoginPath = "/signin";
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



        builder.Services.AddSingleton<GraphQLHttpClient>(s =>
        {
            var options = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost:7153/api/graphql"),
                // Add more options as needed
            };
            return new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(), s.GetRequiredService<HttpClient>());
        });




        builder.Services.AddBlazorBootstrap();
        //builder.Services.AddScoped<DarkModeService>();

        builder.Services.AddSweetAlert2();


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

        app.Run();
    }
}