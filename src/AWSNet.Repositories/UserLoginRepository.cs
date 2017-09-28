using AWSNet.Model;
using AWSNet.Repositories.Core;

namespace AWSNet.Repositories
{
    public interface IUserLoginRepository : IRepository<UserLogin>
    { }

    public class UserLoginRepository : BaseRepository<UserLogin>, IUserLoginRepository
    {
        public UserLoginRepository(IUnitOfWork unitOfWorkInterface)
        {
            UnitOfWork = unitOfWorkInterface;
            Set = UnitOfWork.Context.UserLogin;
        }
    }
}
