using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MyStarterTemplate.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    private const string adminId = "2301D884-221A-4E7D-B509-0113DCC043E1";
    private const string utilizadorId = "7D9B7113-A8F8-4035-99A7-A20DD400F6A3";
    private const string consultorId = "78A7570F-3CE5-48BA-9461-80283ED1D94D";

    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {

        builder.HasData(
               new IdentityRole
               {
                   Id = adminId,
                   Name = "Admin",
                   NormalizedName = "ADMIN"
               },
               new IdentityRole
               {
                   Id = utilizadorId,
                   Name = "Utilizador",
                   NormalizedName = "UTILIZADOR"
               },
               new IdentityRole
               {
                   Id = consultorId,
                   Name = "Consultor",
                   NormalizedName = "CONSULTOR"
               }
           );
    }
}
