using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;

namespace WebApp.Data;

public class WebAppContext : IdentityDbContext<WebAppUser>
{
    public WebAppContext(DbContextOptions<WebAppContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        this.SeedRoles(builder);        // Seed roles first
        this.SeedUsers(builder);        // Then seed users
        this.SeedUserRoles(builder);    // Finally seed user roles
    }


    private void SeedUsers(ModelBuilder builder)
    {
        WebAppUser user = new WebAppUser()
        {
            Id = "b74ddd14-6340-4840-95c2-db12554843e5", // Fix the UserId value here
            UserName = "admin@gmail.com",
            NormalizedUserName = "ADMIN@GMAIL.COM",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            EmailConfirmed = true,
            LockoutEnabled = false,
            PhoneNumber = "1234567890"
        };

        PasswordHasher<WebAppUser> ph = new PasswordHasher<WebAppUser>();
        user.PasswordHash = ph.HashPassword(user, "Bingusbangus123!");

        builder.Entity<WebAppUser>().HasData(user);
    }


    private void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
            new IdentityRole() { Id = "c7b013f0-5201-4317-abd8-c211f91b7330", Name = "SpecialUser", ConcurrencyStamp = "2", NormalizedName = "SPECIALUSER" }
            );
    }

    private void SeedUserRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>() { RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "b74ddd14-6340-4840-95c2-db12554843e5" }
            );
    }
}