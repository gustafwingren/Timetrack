using ApplicationCore;
using ApplicationCore.Interfaces;
using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure.Data;

public class EfRepository<T>: RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T: BaseEntity, IAggregateRoot
{
    public EfRepository(AppDbC appDbC) : base(appDbC)
    {
    }
}
