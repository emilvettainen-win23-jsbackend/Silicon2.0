using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<OptionalAddressEntity> OptionalAddresses { get; set; }
    public DbSet<UserAddressEntity> UserAddresses { get; set; }
    public DbSet<SavedCourseEntity> SavedCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserAddressEntity>()
        .HasKey(x => new { x.UserId, x.AddressId });

        builder.Entity<UserAddressEntity>()
            .HasOne(x => x.User)
            .WithMany(x => x.UserAddresses)
            .HasForeignKey(x => x.UserId);

        builder.Entity<UserAddressEntity>()
            .HasOne(x => x.Address)
            .WithMany(x => x.UserAddresses)
            .HasForeignKey(x => x.AddressId);

        builder.Entity<UserAddressEntity>()
            .HasOne(x => x.OptionalAddress)
            .WithMany(x => x.UserAddresses)
            .HasForeignKey(x => x.OptionalAddressId)
            .IsRequired(false);

        builder.Entity<SavedCourseEntity>()
            .HasKey(x => new { x.UserId, x.CourseId });

        builder.Entity<SavedCourseEntity>()
            .HasOne(x => x.User)
            .WithMany(x => x.SavedCourses)
            .HasForeignKey(x => x.UserId);
    }
}
