using AWSNet.Model;
using AWSNet.Repositories.Core;

namespace AWSNet.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    { }

    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWorkInterface)
        {
            UnitOfWork = unitOfWorkInterface;
            Set = UnitOfWork.Context.Role;
        }
    }
}
