using ATABit.Model.Entities.Contracts;

namespace Template.Application.Common.Contracts;

public interface IEntityService<TEntity> : IReadOnlyEntityService<TEntity> where TEntity : class, IATAMiniEntity, new()
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entitiesToAdd,
        CancellationToken cancellationToken);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(int key, CancellationToken cancellationToken);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Gets called right before Add
    /// </summary>
    Task OnAdding(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Gets called right after Add
    /// </summary>
    Task OnAdded(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Gets called right before delete
    /// </summary>
    Task OnDeleting(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Gets called right after delete
    /// </summary>
    Task OnDeleted(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Gets called right before update
    /// </summary>
    Task OnUpdating(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Gets called right after update
    /// </summary>
    Task OnUpdated(TEntity entity, CancellationToken cancellationToken);
}