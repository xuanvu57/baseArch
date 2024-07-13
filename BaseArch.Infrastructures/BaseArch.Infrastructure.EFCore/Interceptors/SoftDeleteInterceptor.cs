using BaseArch.Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BaseArch.Infrastructure.EFCore.Interceptors
{
    /// <summary>
    /// Interceptor to handle soft delete
    /// </summary>
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        /// <inheritdoc/>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            ChangeStateForSoftDeletableEntities(eventData);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        /// <inheritdoc/>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is null)
            {
                return base.SavingChanges(eventData, result);
            }

            ChangeStateForSoftDeletableEntities(eventData);

            return base.SavingChanges(eventData, result);
        }

        /// <summary>
        /// Change state to Modified for soft deletable entities
        /// </summary>
        /// <param name="eventData"><see cref="DbContextEventData"/></param>
        private static void ChangeStateForSoftDeletableEntities(DbContextEventData eventData)
        {
            if (eventData.Context is null)
                return;

            var entries = eventData.Context
                .ChangeTracker
                .Entries<ISoftDeletable>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
            }
        }
    }
}
