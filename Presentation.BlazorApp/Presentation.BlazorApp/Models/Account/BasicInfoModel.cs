using System.ComponentModel.DataAnnotations;

namespace Presentation.BlazorApp.Models.Account;

public class BasicInfoModel
{
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = null!;



    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = null!;



    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email address is required")]
    [RegularExpression(@"^(([^<>()\]\\.,;:\s@\""]+(\.[^<>()\]\\.,;:\s@\""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$", ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;



    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }



    [DataType(DataType.MultilineText)]
    public string? Biography { get; set; }
}
