﻿using Azure.Messaging.ServiceBus;
using Infrastructure.Services;
using Presentation.BlazorApp.GraphQL;
using Presentation.BlazorApp.Helper;



namespace Presentation.BlazorApp.Configurations;

public static class ServicesConfiguration
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration )
    {
        services.AddScoped<UserService>();
        services.AddScoped<AddressService>();
        services.AddScoped<CourseService>();
        services.AddScoped<ToastService>();
        services.AddSingleton<GraphQLService>();


        services.AddSingleton(new ServiceBusClient(configuration.GetValue<string>("ServiceBusConnection")));
        services.AddHttpClient();


        services.AddAuthentication()
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

    }
}
