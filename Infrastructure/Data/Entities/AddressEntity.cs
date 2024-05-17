using System.ComponentModel.DataAnnotations.Schema;


namespace Infrastructure.Data.Entities;

public class AddressEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string StreetName { get; set; } = null!;

    [Column(TypeName = "nvarchar(50)")]
    public string? SecondStreetName { get; set; }

    [Column(TypeName = "char(6)")]
    public string PostalCode { get; set; } = null!;

    [Column(TypeName = "nvarchar(50)")]
    public string City { get; set; } = null!;

    public ICollection<UserAddressEntity> UserAddresses { get; set; } = new List<UserAddressEntity>();
}
