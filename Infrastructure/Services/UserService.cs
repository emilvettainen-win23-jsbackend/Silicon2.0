using Azure.Messaging.ServiceBus;
using Infrastructure.Data.Contexts;
using Infrastructure.Data.Entities;
using Infrastructure.Dtos.User;
using Infrastructure.Factories;
using Infrastructure.Helpers.Responses;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Infrastructure.Services;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<UserService> _logger;
    private readonly ServiceBusClient _serviceBusClient;
   
  
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<UserService> logger, ServiceBusClient serviceBusClient, AuthenticationStateProvider authenticationStateProvider)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _serviceBusClient = serviceBusClient;
        _authenticationStateProvider = authenticationStateProvider;
 
    }

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
            var isFirstUser = !await _userManager.Users.AnyAsync(x => !x.IsExternalAccount);
            var userExists = await _userManager.Users.AnyAsync(x => x.Email == user.Email);
            if (userExists)
            {
                return ResponseFactory.Exists();
            }

            var createUser = await _userManager.CreateAsync(user, password);
            if (!createUser.Succeeded)
            {
                return ResponseFactory.Error();
            }
            var standardRole = isFirstUser ? "Admin" : "User";
            var roleResult = await _userManager.AddToRoleAsync(user, standardRole);
            return roleResult.Succeeded ? ResponseFactory.Ok() : ResponseFactory.Error();
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
            Debug.WriteLine(ex.Message);
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
        catch (Exception)
        {


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
        catch (Exception)
        {

            return ResponseFactory.ServerError();
        }
    }


    public async Task<ResponseResult> HandleNotificationsFormAsync(string userId, string email, bool newsletter, bool darkMode)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ResponseFactory.NotFound();
            }

            if (email != null)
            {
                await HandleNewsletterAsync(user, newsletter, email);
            }
            
            user.DarkMode = darkMode;
            user.Newsletter = newsletter;
            user.NewsletterEmail = email ?? "";

            var updateResult = await _userManager.UpdateAsync(user);
            return updateResult.Succeeded ? ResponseFactory.Ok() : ResponseFactory.Error();

            

        }
        catch (Exception)
        {

            throw;
        }
    }


    public async Task VerificationRequest(string email)
    {
        try
        {
            var sender = _serviceBusClient.CreateSender("verification_request");
            await sender.SendMessageAsync(new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Email = email }))));
        }
        catch (Exception)
        {

            throw;
        }
    }


    public async Task HandleNewsletterAsync(ApplicationUser user, bool newsletter, string email)
    {
        try
        {
            if (!user.Newsletter && newsletter)
            {
                //skicka servicebus subscribe
            }

            if (user.Newsletter && !newsletter)
            {
                //radera subscribe
            }

        }
        catch (Exception)
        {

            throw;
        }

    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }


}
