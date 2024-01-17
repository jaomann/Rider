namespace ride_Core.Contracts.Repositories
{
    public interface IBaseRepository<T>
    {
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(Guid id);
        IEnumerable<T> GetAll();
        T Get(Guid id);
    }
}
