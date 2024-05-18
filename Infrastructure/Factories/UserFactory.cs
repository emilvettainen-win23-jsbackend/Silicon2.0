using Infrastructure.Data.Entities;
using Infrastructure.Dtos.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Factories;

public class UserFactory
{
    public static UpdateBasicInfoDto UpdateUserDto(string firstName, string lastName, string phoneNumber, string biography)
    {
        try
        {
            return new UpdateBasicInfoDto
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phoneNumber,
                Biography = biography,
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }


    public static UserDto GetUser(ApplicationUser user)
    {
        try
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                Biography = user.Biography,
                ProfileImageUrl = user.ProfileImageUrl,
                IsExternalAccount = user.IsExternalAccount,
                DarkMode = user.DarkMode,
                Newsletter = user.Newsletter,
                NewsletterEmail = user.NewsletterEmail,
            };
        }
        catch (Exception)
        {

            return new UserDto();
        }
    }
}
