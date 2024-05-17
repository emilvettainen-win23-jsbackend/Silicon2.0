using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Entities;

public class ApplicationUser : IdentityUser
{
    [ProtectedPersonalData]
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    public string? Biography { get; set; }
    public string? ProfileImageUrl { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime Created { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? Modified { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? Expired { get; set; }

    public bool PendingDelete { get; set; } = false;

    public bool IsExternalAccount { get; set; } = false;

    public bool DarkMode { get; set; } = false;
    public bool Newsletter { get; set; } = false;

    public string? NewsletterEmail { get; set; }

    public ICollection<UserAddressEntity> UserAddresses { get; set; } = new List<UserAddressEntity>();

    public ICollection<SavedCourseEntity> SavedCourses { get; set; } = new List<SavedCourseEntity>();
}
