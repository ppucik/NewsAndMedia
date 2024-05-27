using ASPNETCoreWebAPI.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ASPNETCoreWebAPI.Repositories.Interceptors;

public sealed class UpdateAuditableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditableEntities(DbContext context)
    {
        DateTime utcNow = DateTime.UtcNow;
        IEnumerable<EntityEntry<IAuditableEntity>> entities = context.ChangeTracker.Entries<IAuditableEntity>().ToList();

        foreach (EntityEntry<IAuditableEntity> entry in entities)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    SetCurrentPropertyValue(entry, nameof(IAuditableEntity.Created), utcNow);
                    break;
                case EntityState.Modified:
                    SetCurrentPropertyValue(entry, nameof(IAuditableEntity.Modified), utcNow);
                    break;
                default:
                    break;
            }
        }

        static void SetCurrentPropertyValue(
            EntityEntry entry,
            string propertyName,
            DateTime utcNow) =>
            entry.Property(propertyName).CurrentValue = DateTime.UtcNow;
    }
}
