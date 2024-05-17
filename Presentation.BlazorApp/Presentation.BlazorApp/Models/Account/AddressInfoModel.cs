using System.ComponentModel.DataAnnotations;

namespace Presentation.BlazorApp.Models.Account;

public class AddressInfoModel
{
    [Required(ErrorMessage = "Address is required")]
    public string Addressline_1 { get; set; } = null!;

    public string? Addressline_2 { get; set; }

    [DataType(DataType.PostalCode)]
    [Required(ErrorMessage = "Postalcode is required")]
    public string PostalCode { get; set; } = null!;


    [Required(ErrorMessage = "City is required")]
    public string City { get; set; } = null!;
}