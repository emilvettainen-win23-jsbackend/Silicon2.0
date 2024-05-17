namespace Infrastructure.Data.Entities;

public class UserAddressEntity
{
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public int AddressId { get; set; }
    public AddressEntity Address { get; set; } = null!;

    public int? OptionalAddressId { get; set; }
    public OptionalAddressEntity? OptionalAddress { get; set; }
}
