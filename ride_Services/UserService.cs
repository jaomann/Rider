using ride_Core.Contracts;
using ride_Core.Contracts.Repositories;
using ride_Core.Entities;

namespace ride_Services
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(IBaseRepository<User> repository) : base(repository)
        {
        }
    }
}
