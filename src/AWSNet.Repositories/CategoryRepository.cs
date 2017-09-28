using AWSNet.Model;
using AWSNet.Repositories.Core;

namespace AWSNet.Repositories
{
    public interface ICategoryRepository : IAsyncRepository<Category>
    { }

    public class CategoryRepository : BaseAsyncRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork unitOfWorkInterface)
        {
            UnitOfWork = unitOfWorkInterface;
            Set = UnitOfWork.Context.Category;
        }
    }
}
