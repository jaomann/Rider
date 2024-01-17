using ride_Core.Contracts;
using ride_Core.Contracts.Repositories;
using ride_Core.Entities;

namespace ride_Services
{
    public class BaseService<T> : IBaseService<T> where T : EntityBase
    {
        private readonly IBaseRepository<T> _repository;
        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }
        public void Create(T entity)
        {
           _repository.Create(entity);
        }

        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }

        public T Get(Guid id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
        }
    }
}