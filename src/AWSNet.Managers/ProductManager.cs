using AutoMapper;
using AWSNet.Dtos;
using AWSNet.Managers.Core;
using AWSNet.Model;
using AWSNet.Repositories;
using AWSNet.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AWSNet.Managers.MapperProfiles;
using AWSNet.Utils.Solr;
using AWSNet.Utils.Configuration;
using Newtonsoft.Json;

namespace AWSNet.Managers
{
    public interface IProductManager : IManager<ProductDto>
    {
        Task<ICollection<ProductDto>> GetAll(int? skip, int? take, bool includeMedia = false);
        Task<ProductDto> GetById(int id, bool includeMedia = false);
    }

    public class ProductManager : IProductManager
    {
        private readonly IProductRepository _repository;
        private IUnitOfWork _unitOfWork;

        public ProductManager(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ProductProfile>();

            });
        }

        public async Task<ICollection<ProductDto>> GetAll(int? skip, int? take, bool includeMedia = false)
        {
            var Products = new List<ProductDto>();

            var ProductSet = await _repository.GetAll();

            if (skip.HasValue)
                ProductSet = ProductSet.OrderBy(c => c.ID).Skip(skip.Value);

            if (take.HasValue)
                ProductSet = ProductSet.Take(take.Value);

            foreach (var Product in ProductSet)
                Products.Add(MapToDto(Product, includeMedia));

            return Products;
        }

        public async Task<ProductDto> GetById(int id, bool includeMedia = false)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");

            var Product = await _repository.GetById(id);
            return Product != null ? MapToDto(Product, includeMedia) : null;
        }

        public async Task<ICollection<ProductDto>> GetAll(int? skip = null, int? take = null)
        {
            return await GetAll(skip, take, false);
        }

        public async Task<ProductDto> GetById(int id)
        {
            return await GetById(id, false);
        }

        public async Task<ProductDto> Add(ProductDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            if ((await _repository.Get(c => c.Name.Equals(dto.Name))).Any())
                throw new ArgumentException("Product already exists"); //We do not allow two Product with the same name.

            var Product = await _repository.Add(MapFromDto(dto));
            await SolrHelper.DataImport(SolrCore.PRODUCT);

            return MapToDto(Product);
        }

        public async Task Update(ProductDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            var Product = await _repository.GetById(dto.Id);

            if (Product == null)
                throw new ArgumentNullException("Product");

            if ((await _repository.Get(c => c.Name.Equals(dto.Name) && c.ID != dto.Id)).Any())
                throw new ArgumentException("Product already exists");

            //await _repository.Update(MapFromDto(dto, Product));
            //await SolrHelper.DataImport(SolrCore.PRODUCT);
        }

        public async Task Delete(ProductDto dto)
        {
            var Product = await _repository.GetById(dto.Id);

            if (Product == null)
                throw new ArgumentNullException("Product");

            //await _repository.Delete(Product);
            //await SolrHelper.DeleteDocumentById(SolrCore.PRODUCT, Product.Id);
        }

        public async Task<int> Count()
        {
            return await _repository.Count();
        }

        private ProductDto MapToDto(Product Product, bool includeMedia = false)
        {
            var dto = Mapper.Map<ProductDto>(Product);

            return dto;
        }

        private Product MapFromDto(ProductDto ProductDto, Product Product = null)
        {
            Product ret;

            if (Product == null)
            {
                var newProduct = Mapper.Map<Product>(ProductDto);
                newProduct.CreationUser = newProduct.CreationUser;

                ret = newProduct;
            }
            else
            {
                Mapper.Map<ProductDto, Product>(ProductDto, Product);
                Product.LastModificationDate = DateTime.UtcNow;
                Product.LastModificationUser = ProductDto.LastModificationUser;

                ret = Product;
            }

            return ret;
        }
    }
}
