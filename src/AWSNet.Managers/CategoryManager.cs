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
    public interface ICategoryManager : IManager<CategoryDto>
    {
        Task<ICollection<CategoryDto>> GetAll(int? skip, int? take, bool includeMedia = false);
        Task<CategoryDto> GetById(int id, bool includeMedia = false);
    }

    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository _repository;
        private IUnitOfWork _unitOfWork;

        public CategoryManager(ICategoryRepository repository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<CategoryProfile>();
                //cfg.AddProfile<CreateMediaProfile>();
                //cfg.AddProfile<KeywordProfile>();
            });
        }

        public async Task<ICollection<CategoryDto>> GetAll(int? skip, int? take, bool includeMedia = false)
        {
            var categories = new List<CategoryDto>();

            var categorySet = await _repository.GetAll();

            if (skip.HasValue)
                categorySet = categorySet.OrderBy(c => c.Id).Skip(skip.Value);

            if (take.HasValue)
                categorySet = categorySet.Take(take.Value);

            foreach (var category in categorySet)
                categories.Add(MapToDto(category, includeMedia));

            return categories;
        }

        public async Task<CategoryDto> GetById(int id, bool includeMedia = false)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");

            var category = await _repository.GetById(id);
            return category != null ? MapToDto(category, includeMedia) : null;
        }

        public async Task<ICollection<CategoryDto>> GetAll(int? skip = null, int? take = null)
        {
            return await GetAll(skip, take, false);
        }

        public async Task<CategoryDto> GetById(int id)
        {
            return await GetById(id, false);
        }

        public async Task<CategoryDto> Add(CategoryDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            if ((await _repository.Get(c => c.Name.Equals(dto.Name))).Any())
                throw new ArgumentException("category already exists"); //We do not allow two Category with the same name.

            var category = await _repository.Add(MapFromDto(dto));
            await SolrHelper.DataImport(SolrCore.CATEGORY);

            return MapToDto(category);
        }

        public async Task Update(CategoryDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            var category = await _repository.GetById(dto.Id);

            if (category == null)
                throw new ArgumentNullException("category");

            if ((await _repository.Get(c => c.Name.Equals(dto.Name) && c.Id != dto.Id)).Any())
                throw new ArgumentException("category already exists");

            await _repository.Update(MapFromDto(dto, category));
            await SolrHelper.DataImport(SolrCore.CATEGORY);
        }

        public async Task Delete(CategoryDto dto)
        {
            var category = await _repository.GetById(dto.Id);

            if (category == null)
                throw new ArgumentNullException("category");

            await _repository.Delete(category);
            await SolrHelper.DeleteDocumentById(SolrCore.CATEGORY, category.Id);
        }

        public async Task<int> Count()
        {
            return await _repository.Count();
        }

        private CategoryDto MapToDto(Category category, bool includeMedia = false)
        {
            var dto = Mapper.Map<CategoryDto>(category);
            return dto;
        }

        private Category MapFromDto(CategoryDto categoryDto, Category category = null)
        {
            Category ret;

            if (category == null)
            {
                var newCategory = Mapper.Map<Category>(categoryDto);
                newCategory.CreationUser = newCategory.CreationUser;

                ret = newCategory;
            }
            else
            {
                Mapper.Map<CategoryDto, Category>(categoryDto, category);
                category.LastModificationDate = DateTime.UtcNow;
                category.LastModificationUser = categoryDto.LastModificationUser;

                ret = category;
            }

            return ret;
        }
    }
}
