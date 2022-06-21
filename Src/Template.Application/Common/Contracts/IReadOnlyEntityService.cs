using System.Linq.Expressions;
using Bit.Model.Contracts;

namespace Template.Application.Common.Contracts;

public interface IReadOnlyEntityService<TEntity> where TEntity : class, IEntity, new()
{
    Task<TEntity?> GetByIdAsync(int key, CancellationToken cancellationToken);

    IQueryable<TEntity> GetAll();

    IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

    Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>>? where, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "");

    Task<IQueryable<TEntity>> GetAsync(CancellationToken cancellationToken);
}