using ApplicationCore;
using ApplicationCore.Interfaces;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Test.Shared;

public class FakeRepository<T> :IReadRepository<T>, IRepository<T> where T: BaseEntity, IAggregateRoot
{
    private ICollection<T> _items;

    private readonly ISpecificationEvaluator _specificationEvaluator;

    public FakeRepository(List<T> items)
    {
        _items = items;
        _specificationEvaluator = SpecificationEvaluator.Default;
    }

    public async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = new CancellationToken()) where TId : notnull
    {
        return await Task.FromResult(_items.FirstOrDefault(x => x.Id.Equals(id)));
    }

    public Task<T?> GetBySpecAsync<TSpec>(TSpec specification, CancellationToken cancellationToken = new CancellationToken()) where TSpec : ISingleResultSpecification, ISpecification<T>
    {
        return ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<List<T>> ListAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(_items.ToList());
    }

    public async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = new CancellationToken())
    {
        var queryResult = await ApplySpecification(specification).ToListAsync(cancellationToken);

        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();    }

    public async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = new CancellationToken())
    {
        var queryResult = await ApplySpecification(specification).ToListAsync(cancellationToken);

        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();    }

    public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = new CancellationToken())
    {
        return await ApplySpecification(specification, true).CountAsync(cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(_items.Count());
    }

    public Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(ApplySpecification(specification, true).Any());
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(_items.Any());
    }

    public Task<T> AddAsync(T entity, CancellationToken cancellationToken = new CancellationToken())
    {
        entity.Id = Guid.NewGuid();
        _items.Add(entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Filters the entities  of <typeparamref name="T"/>, to those that match the encapsulated query logic of the
    /// <paramref name="specification"/>.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>The filtered entities as an <see cref="IQueryable{T}"/>.</returns>
    protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
    {
        return _specificationEvaluator.GetQuery(_items.AsQueryable(), specification, evaluateCriteriaOnly);
    }
    /// <summary>
    /// Filters all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// <para>
    /// Projects each entity into a new form, being <typeparamref name="TResult" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>The filtered projected entities as an <see cref="IQueryable{T}"/>.</returns>
    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return _specificationEvaluator.GetQuery(_items.AsQueryable(), specification);
    }
}
