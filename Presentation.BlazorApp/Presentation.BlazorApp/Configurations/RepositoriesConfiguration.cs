﻿using Infrastructure.Repositories;
using Infrastructure.Repositories.Address;
using Infrastructure.Repositories.Course;

namespace Presentation.BlazorApp.Configurations;

public static class RepositoriesConfiguration
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(BaseRepository<,>), typeof(BaseRepository<,>));
        services.AddScoped<AddressRepository>();
        services.AddScoped<OptionalAddressRepository>();
        services.AddScoped<UserAddressRepository>();
        services.AddScoped<SavedCourseRepository>();
    }
}

