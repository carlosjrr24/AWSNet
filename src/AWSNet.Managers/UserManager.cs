using AWSNet.Dtos;
using AWSNet.Managers.Core;
using AWSNet.Model;
using AWSNet.Repositories;
using AWSNet.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using AWSNet.Managers.MapperProfiles;
using AWSNet.Utils.Solr;
using AWSNet.Utils.Configuration;

namespace AWSNet.Managers
{
    public interface IUserManager : IManager<UserDto>
    {
        Task Enable(int id);
        Task Disable(int id);
        Task SetProfileImagePath(UserDto dto);
    }

    public class UserManager : IUserManager
    {
        private readonly IUserRepository _repository;
        private readonly IRoleRepository _roleRepository;
        private IUnitOfWork _unitOfWork;

        public UserManager(IUserRepository repository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _roleRepository = roleRepository;

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });
        }

        public async Task<ICollection<UserDto>> GetAll(int? skip = null, int? take = null)
        {
            var users = new List<UserDto>();

            var userSet = await Task.FromResult(_repository.GetAll().Where(u => !u.IsDeleted));

            if (skip.HasValue)
                userSet = userSet.OrderBy(u => u.Id).Skip(skip.Value);

            if (take.HasValue)
                userSet = userSet.Take(take.Value);

            foreach (var user in userSet)
                users.Add(MapToDto(user));

            return users;
        }

        public async Task<UserDto> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");

            var user = await Task.FromResult(_repository.GetById(id));
            return user != null && !user.IsDeleted ? MapToDto(user) : null;
        }

        public async Task<UserDto> Add(UserDto dto)
        {
            await Task.FromResult(0);
            throw new System.NotImplementedException();
        }

        public async Task Update(UserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            var user = _repository.GetById(dto.Id);

            if (user == null || user.IsDeleted)
                throw new ArgumentNullException("user");

            _repository.Update(MapFromDto(dto, user));
            await SolrHelper.DataImport(SolrCore.USER);
        }

        public async Task SetProfileImagePath(UserDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ArgumentNullException("dto");

            var user = _repository.GetById(dto.Id);

            if (user == null || user.IsDeleted)
                throw new ArgumentNullException("user");

            user.ProfileImagePath = dto.ProfileImagePath;

            _repository.Update(user);
            await SolrHelper.DataImport(SolrCore.USER);
        }

        public async Task Enable(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");

            var user = _repository.GetById(id);

            if (user == null || user.IsDeleted)
                throw new ArgumentNullException("user");

            user.IsEnabled = true;

            _repository.Update(user);
            await SolrHelper.DataImport(SolrCore.USER);
        }

        public async Task Disable(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");

            var user = _repository.GetById(id);

            if (user == null || user.IsDeleted)
                throw new ArgumentNullException("user");

            user.IsEnabled = false;

            _repository.Update(user);
            await SolrHelper.DataImport(SolrCore.USER);
        }

        public async Task Delete(UserDto dto)
        {
            var user = _repository.GetById(dto.Id);

            if (user == null || user.IsDeleted)
                throw new ArgumentNullException("user");

            user.IsEnabled = false;
            user.IsDeleted = true;

            _repository.Update(user);
            await SolrHelper.DataImport(SolrCore.USER);
        }

        public async Task<int> Count()
        {
            return await Task.FromResult(_repository.Count());
        }

        private UserDto MapToDto(User user)
        {
            var dto = Mapper.Map<UserDto>(user);

            if (user.Roles.Any())
                dto.Roles.AddRange(user.Roles.Select(r => r.Name));

            return dto;
        }

        private User MapFromDto(UserDto userDto, User user)
        {
            Mapper.Map<UserDto, User>(userDto, user); ;

            user.LastModificationDate = DateTime.UtcNow;
            user.LastModificationUser = userDto.LastModificationUser;

            var rolesToRemove = user.Roles.Where(role => !userDto.Roles.Any(r => r.Equals(role.Name, StringComparison.InvariantCultureIgnoreCase))).ToArray();
            foreach (var role in rolesToRemove)
                user.Roles.Remove(role);

            var rolesToAdd = userDto.Roles.Where(r => !user.Roles.Any(role => role.Name.Equals(r, StringComparison.InvariantCultureIgnoreCase))).ToArray();
            foreach (var roleName in rolesToAdd)
            {
                var r = _roleRepository.Get(ro => ro.Name.Equals(roleName));

                if (r == null)
                    throw new ArgumentException("roleName does not correspond to a Role entity", "roleName");

                user.Roles.Add(r);
            }

            return user;
        }
    }
}
