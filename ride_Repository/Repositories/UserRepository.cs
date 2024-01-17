using ride_Core.Contracts.Repositories;
using ride_Core.Entities;

namespace ride_Repository.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(Context context) : base(context)
        {
        }
    }
}
