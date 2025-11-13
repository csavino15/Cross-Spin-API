namespace Domain.Abstractions;

public interface IRepository<T>
    where T : Entity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(T entity);
    void Delete(T entity);
}
