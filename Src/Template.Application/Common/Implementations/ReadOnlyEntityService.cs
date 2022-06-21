using System.Linq.Expressions;
using ATABit.Helper.Extensions;
using ATABit.Model.Data.Contracts;
using ATABit.Model.User.Contract;
using AutoMapper;
using Bit.Model.Contracts;
using Template.Application.Common.Contracts;

namespace Template.Application.Common.Implementations;

public abstract class ReadOnlyEntityService<TEntity> : IReadOnlyEntityService<TEntity> where TEntity : class, IEntity, new()
{
    #region Property Injections

    public IReadOnlyATARepository<TEntity> ReadOnlyATARepository { get; set; } = default!;

    public IMapper Mapper { get; set; } = default!;

    public IUserInfoProvider UserInfoProvider { get; set; } = default!; //Property Injection


    #endregion Property Injections

    public Expression<Func<TEntity, bool>>? CommonWhere { get; set; }
    public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? CommonOrder { get; set; }

    public virtual async Task<TEntity?> GetByIdAsync(int key, CancellationToken cancellationToken)
    {
        TEntity? entity = await ReadOnlyATARepository.GetByIdAsync(cancellationToken, key);

        return entity;
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        IQueryable<TEntity> entities = ReadOnlyATARepository.GetAll();

        return entities;
    }

    private IQueryable<TEntity> GetSorted(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy)
    {
        return orderBy != null ? orderBy(query) : (CommonOrder != null ? CommonOrder(query) : query);
    }

    public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> query = ReadOnlyATARepository.GetAll().AddWhere(CommonWhere).AddWhere(where);

        return GetSorted(query, orderBy).AsEnumerable();
    }

    public async Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
    {
        IQueryable<TEntity> query = (await ReadOnlyATARepository.GetAllAsync(cancellationToken)).AddWhere(CommonWhere).AddWhere(where);

        return GetSorted(query, orderBy).AsEnumerable();
    }

    public async Task<IQueryable<TEntity>> GetAsync(CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = await ReadOnlyATARepository.GetAllAsync(cancellationToken);

        return query;
    }
}