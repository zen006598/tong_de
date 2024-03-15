using System.Linq.Expressions;

namespace tongDe.Data.Repository;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task SaveAsync();
    void Update(T entity);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
}
