using ATABit.Model.Data.Contracts;
using ATABit.Model.Entities.Contracts;
using Bit.Core.Exceptions;
using Template.Application.Common.Contracts;

namespace Template.Application.Common.Implementations;

public abstract class EntityService<TEntity> : ReadOnlyEntityService<TEntity>, IEntityService<TEntity> where TEntity : class, IATAMiniEntity, new()
{
    public IATARepository<TEntity> Repository { get; set; } = default!; //Property Injection

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await OnAdding(entity, cancellationToken);

        TEntity addedEntity = await Repository.AddAsync(entity, cancellationToken);

        await OnAdded(addedEntity, cancellationToken);

        return addedEntity;
    }

    public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entitiesToAdd,
        CancellationToken cancellationToken)
    {
        var addedEntities = new List<TEntity>();

        foreach (var entityToAdd in entitiesToAdd)
        {
            var addedEntity = await AddAsync(entityToAdd, cancellationToken);

            addedEntities.Add(addedEntity);
        }

        return addedEntities;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await OnUpdating(entity, cancellationToken);

        TEntity updatedEntity = await Repository.UpdateAsync(entity, cancellationToken);

        await OnUpdated(updatedEntity, cancellationToken);

        return updatedEntity;
    }

    public virtual async Task DeleteAsync(int key, CancellationToken cancellationToken)
    {
        TEntity? entity = await Repository.GetByIdAsync(cancellationToken, key);

        if (entity == null)
            throw new ResourceNotFoundException();

        await DeleteAsync(entity: entity, cancellationToken);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await OnDeleting(entity, cancellationToken);

        TEntity deletedEntity = await Repository.DeleteAsync(entity, cancellationToken);

        await OnDeleted(deletedEntity, cancellationToken);
    }

    #region CRUD Events

    /// <summary>
    /// Gets called right before Add
    /// </summary>
    public virtual Task OnAdding(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets called right after Add
    /// </summary>
    public virtual Task OnAdded(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets called right before delete
    /// </summary>
    public virtual Task OnDeleting(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets called right after delete
    /// </summary>
    public virtual Task OnDeleted(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets called right before update
    /// </summary>
    public virtual Task OnUpdating(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets called right after update
    /// </summary>
    public virtual Task OnUpdated(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    #endregion CRUD Events
}

public enum OperationKind
{
    Get,
    Add,
    Delete,
    Update
}