﻿using Azure.Messaging.ServiceBus;
using Infrastructure.Data.Entities;
using Infrastructure.Dtos.User;
using Infrastructure.Factories;
using Infrastructure.Helpers.Responses;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;

namespace Infrastructure.Services;

public class UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<UserService> logger, ServiceBusClient serviceBusClient, AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient, IConfiguration config)
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ILogger<UserService> _logger = logger;
    private readonly ServiceBusClient _serviceBusClient = serviceBusClient;
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _config = config;
   
  
    private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

    public async Task<UserDto> GetCurrentUserAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return new UserDto();
        }
        var currentUser = await _userManager.GetUserAsync(user);
        return UserFactory.GetUser(currentUser!) ?? new UserDto();
    }

    public async Task<bool> GetDarkModeSettingAsync()
    {
        var user = await GetCurrentUserAsync();
        return user?.DarkMode ?? false;
    }


    public async Task<ResponseResult> CreateUserAsync(ApplicationUser user, string password)
    {
        try
        {
            var userExists = await _userManager.Users.AnyAsync(x => x.Email == user.Email);
            if (userExists)
            {
                return ResponseFactory.Exists();
            }

            var createUser = await _userManager.CreateAsync(user, password);
            return createUser.Succeeded ? ResponseFactory.Ok() : ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UserService.CreateUserAsync() :: {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> UpdateBasicInfoAsync(string userId, UpdateBasicInfoDto dto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ResponseFactory.NotFound();
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.Phone;
            user.Biography = dto.Biography;
            user.Modified = DateTime.Now;

            var updateResult = await _userManager.UpdateAsync(user);
         
            return updateResult.Succeeded ? ResponseFactory.Ok() : ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UserService.UpdateBasicInfoAsync() :: {ex.Message}");
            return ResponseFactory.ServerError();
        }
    }


    public async Task<BasicInfoDto> PopulateBaseInfoAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null!;
            }

            var model = new BasicInfoDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Biography = user.Biography,
                Email = user.Email!,
                Phone = user.PhoneNumber,
            };

            return model;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UserService.PopulateBaseInfoAsync() :: {ex.Message}");
            return null!;
        }
    }

    public async Task<bool> VerifyCurrentPasswordAsync(string userId, string currentPassword)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, currentPassword);
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UserService.VerifyCurrentPasswordAsync() :: {ex.Message}");
        }
        return false;
    }

    public async Task<ResponseResult> UpdatePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ResponseFactory.Error();
            }

            var updatePassword = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return updatePassword.Succeeded ? ResponseFactory.Ok() : ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UserService.UpdatePasswordAsync() :: {ex.Message}");
            return ResponseFactory.ServerError();
        }
    }


    public async Task<ResponseResult> HandleNotificationsFormAsync(string userId, string? email, bool newsletter, bool darkMode)
    {
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var newsResult = await HandleNewsletterAsync(user, newsletter, email!);
            switch (newsResult.StatusCode)
            {
                case ResultStatus.OK:
                    break;

                case ResultStatus.EXISTS:
                    return ResponseFactory.Exists("An subscription is already exists with the given email");

                default:
                    return ResponseFactory.Error("An error occurred. Your changes could not be saved. Please contact support!");
            }

            user.DarkMode = darkMode;
            user.Newsletter = newsletter;
            user.NewsletterEmail = email ?? "";
            var updateResult = await _userManager.UpdateAsync(user);
            return updateResult.Succeeded ? ResponseFactory.Ok("Changes have been saved!") : ResponseFactory.Error("Failed to update changes. Please try again!");
        }
        return ResponseFactory.Error("Failed to update changes. Please try again!");
    }

    public async Task<ResponseResult> HandleNewsletterAsync(ApplicationUser user, bool newsletter, string email)
    {
        if (!user.Newsletter && newsletter)
        {
            var result = await _httpClient.PostAsJsonAsync($"{_config["CreateSubscription"]}", new { Email = email });
            return result.StatusCode switch
            {
                System.Net.HttpStatusCode.OK => ResponseFactory.Ok(),
                System.Net.HttpStatusCode.Conflict => ResponseFactory.Exists(),
                _ => ResponseFactory.Error()
            };
        }

        if (user.Newsletter && !newsletter)
        {
            var result = await _httpClient.PostAsJsonAsync($"{_config["DeleteSubscription"]}", new { Email = user.NewsletterEmail });
            return result.StatusCode switch 
            { 
                System.Net.HttpStatusCode.OK => ResponseFactory.Ok(),
                _ => ResponseFactory.Error()
            };
        }
        return ResponseFactory.Ok();
    }


    public async Task VerificationRequest(string email)
    {
        try
        {
            var sender = _serviceBusClient.CreateSender("verification_request");
            await sender.SendMessageAsync(new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Email = email }))));
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UserService.VerificationRequest() :: {ex.Message}");
        }
    }


    public async Task<bool> UpdateProfileImageAsync(string filePath)
    {
        try
        {
            var user = await GetCurrentUserAsync();
            var userEntity = await _userManager.FindByIdAsync(user.Id);
            if (userEntity != null)
            {
                userEntity.ProfileImageUrl = filePath;
                var updateResult = await _userManager.UpdateAsync(userEntity);
                return updateResult.Succeeded;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UserService.UpdateProfileImageAsync() :: {ex.Message}");
        }
        return false;
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}