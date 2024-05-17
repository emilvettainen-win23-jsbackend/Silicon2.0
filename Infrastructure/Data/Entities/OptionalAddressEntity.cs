using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data.Entities;

public class OptionalAddressEntity
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string OptionalAddress { get; set; } = null!;

    public ICollection<UserAddressEntity> UserAddresses { get; set; } = new List<UserAddressEntity>();
}
