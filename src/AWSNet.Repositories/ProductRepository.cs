using AWSNet.Model;
using AWSNet.Repositories.Core;

namespace AWSNet.Repositories
{
    public interface IProductRepository : IAsyncRepository<Product>
    { }

    public class ProductRepository : BaseAsyncRepository<Product>, IProductRepository
    {
        public ProductRepository(IUnitOfWork unitOfWorkInterface)
        {
            UnitOfWork = unitOfWorkInterface;
            Set = UnitOfWork.Context.Products;
        }
    }
}
