using ATABit.Model.Data.Contracts;
using ATABit.Model.Entities.Contracts;
using ATABit.Model.User.Contract;
using Bit.Data.EntityFrameworkCore.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Template.Infrastructure.Persistence.EF;

namespace ATA.Food.Server.Data.Repository;

public class ATARepository<TEntity> : EfCoreRepository<ATADbContext, TEntity>, IATARepository<TEntity>
    where TEntity : class, IATAMiniEntity, new()
{
    public override Task<TEntity> AddAsync(TEntity entityToAdd, CancellationToken cancellationToken)
    {
        //// Moved to Interceptors
        //entityToAdd.CreatedSources = "{created json}";
        //entityToAdd.ModifiedSources = "{modified json}";

        return base.AddAsync(entityToAdd, cancellationToken);
    }

    public override Task<TEntity> UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken)
    {
        //// Moved to Interceptors
        //entityToAdd.ModifiedSources = "{modified json}";

        return base.UpdateAsync(entityToUpdate, cancellationToken);
    }

    public IQueryable<TEntity> GetAllWithoutQueryFilter()
    {
        return Set.IgnoreQueryFilters();
    }

    public override IQueryable<TEntity> GetAll()
    {
        Expression<Func<TEntity, bool>>? getFilter = CreateGetFilter();

        IQueryable<TEntity> queryable = base.GetAll();

        if (getFilter != null)
        {
            queryable = queryable.Where(getFilter);
        }

        return queryable;
    }

    public override async Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        Expression<Func<TEntity, bool>>? getFilter = CreateGetFilter();

        IQueryable<TEntity> queryable = await base.GetAllAsync(cancellationToken);

        if (getFilter != null)
        {
            queryable = queryable.Where(getFilter);
        }

        return queryable;
    }

    public Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken,
        params Expression<Func<TEntity, object?>>[]? includes)
    {
        if (includes != null)
        {
            includes = includes.Where(i => i != null).ToArray();
            if (includes.Length == 0)
            {
                includes = null;
            }
        }

        if (includes == null)
        {
            return GetAll()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return includes
            .Aggregate(GetAll(), (query, inc) => query.Include(inc))
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TEntity> GetByIdWithoutQueryFilterAsync(int id, CancellationToken cancellationToken,
        params Expression<Func<TEntity, object?>>[]? includes)
    {
        if (includes != null)
        {
            includes = includes.Where(i => i != null).ToArray();
            if (includes.Length == 0)
            {
                includes = null;
            }
        }

        if (includes == null)
        {
            return GetAllWithoutQueryFilter()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return includes
            .Aggregate(GetAllWithoutQueryFilter(), (query, inc) => query.Include(inc))
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken)
    {
        DbContext.Set<TEntity>().Remove(new TEntity { Id = id });
        int rowsAffected = await DbContext.SaveChangesAsync(cancellationToken);
        return rowsAffected == 1;
    }

    public async Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));

        int counter = 0;
        foreach (TEntity entity in entities)
        {
            await base.DeleteAsync(entity, cancellationToken);
            counter++;
        }

        return counter;
    }

    protected virtual Expression<Func<TEntity, bool>>? CreateGetFilter()
    {
        Expression<Func<TEntity, bool>>? criteria = null;

        return criteria;
    }

    private static Expression<Func<TEntity, bool>> AndAlsoNullable(
        Expression left,
        string entityMemberName,
        string userInfoProviderMemberName,
        IUserInfoProvider userInfoProvider
    )
    {
        ParameterExpression entityExp = Expression.Parameter(typeof(TEntity), "entity");

        MemberExpression entityMemberAccessExp = Expression.MakeMemberAccess(
            entityExp,
            typeof(TEntity).GetMember(entityMemberName).First()
        );

        MemberExpression userInfoProviderExp = Expression.MakeMemberAccess(
            Expression.Constant(userInfoProvider),
            typeof(IUserInfoProvider).GetMember(userInfoProviderMemberName).First()
        );

        BinaryExpression newBody = Expression.Equal(
            entityMemberAccessExp,
            userInfoProviderExp
        );

        if (left == null)
        {
            return Expression.Lambda<Func<TEntity, bool>>(newBody, entityExp);
        }

        Expression body = ((LambdaExpression)left).Body;

        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(
                body,
                newBody
            ),
            entityExp
        );
    }
}