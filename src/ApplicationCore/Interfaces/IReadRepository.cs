using Ardalis.Specification;

namespace ApplicationCore.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : BaseEntity, IAggregateRoot
{
}
