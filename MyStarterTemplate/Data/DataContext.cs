using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyStarterTemplate.Models;
using System.Collections;
using System.Reflection;

namespace MyStarterTemplate.Data;

public class DataContext : IdentityDbContext<IdentityUser, IdentityRole, string, IdentityUserClaim<string>,
     IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    #region set DbSet

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges()
    {
        OnBeforeSaving();
        return base.SaveChanges();
    }

    private void OnBeforeSaving()
    {
        IEnumerable entries = ChangeTracker.Entries();
        foreach (EntityEntry entry in entries)
        {
            if (entry.Entity is IBaseEntity baseEntity)
            {
                DateTime now = DateTime.UtcNow;
                string user;
                try
                {
                    user = _httpContextAccessor.HttpContext!.User.Identity!.Name!;
                }
                catch (NullReferenceException)
                {
                    user = "SISTEMA";
                }

                switch (entry.State)
                {
                    case EntityState.Modified:
                        baseEntity.LastUpdatedAt = now;
                        baseEntity.LastUpdatedBy = user;
                        break;

                    case EntityState.Added:
                        baseEntity.CreatedAt = now;
                        baseEntity.CreatedBy = user;
                        baseEntity.LastUpdatedAt = null;
                        baseEntity.LastUpdatedBy = string.Empty;
                        break;
                }
            }
        }
    }
}
