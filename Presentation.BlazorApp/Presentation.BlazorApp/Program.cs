using Azure.Messaging.ServiceBus;
using Infrastructure.Data.Contexts;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Address;
using Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.BlazorApp;
using Presentation.BlazorApp.Components;
using Presentation.BlazorApp.Configurations;


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



builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<UserAddressRepository>();
builder.Services.AddScoped<OptionalAddressRepository>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton(new ServiceBusClient(builder.Configuration.GetConnectionString("ServiceBusConnection")));


builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped<DarkModeService>();




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

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Presentation.BlazorApp.Client._Imports).Assembly);
await RolesConfiguration.RegisterRoles(app.Services);

app.Run();
