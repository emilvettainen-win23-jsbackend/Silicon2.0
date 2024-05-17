namespace Infrastructure.Dtos.Address;

public class AddressDto
{
    public string StreetName { get; set; } = null!;
    public string? OptionalAddress { get; set; }
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
}
