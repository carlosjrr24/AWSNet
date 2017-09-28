using AWSNet.Model;
using AWSNet.Repositories.Core;

namespace AWSNet.Repositories
{
    public interface IUserRepository : IRepository<User>
    { }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWorkInterface)
        {
            UnitOfWork = unitOfWorkInterface;
            Set = UnitOfWork.Context.User;
        }
    }
}
