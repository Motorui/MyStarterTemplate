using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyStarterTemplate.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    private const string adminId = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7";

    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        var admin = new IdentityUser
        {
            Id = adminId,
            UserName = "rapereira",
            NormalizedUserName = "RAPEREIRA",
            Email = "rui.santos@portway.pt",
            NormalizedEmail = "RUI.SANTOS@PORTWAY.PT",
            PhoneNumber = "XXXXXXXXXXXXX",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = new Guid().ToString("D")
        };

        admin.PasswordHash = PassGenerate(admin);

        builder.HasData(admin);
    }

    public string PassGenerate(IdentityUser user)
    {
        var passHash = new PasswordHasher<IdentityUser>();
        return passHash.HashPassword(user, "rui0504");
    }
}

