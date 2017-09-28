using AWSNet.Model;
using AWSNet.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AWSNet.Managers
{
    public interface IApplicationUserManager : IUserStore<User, int>, IUserLoginStore<User, int>, IUserPasswordStore<User, int>, IUserEmailStore<User, int>,
                             IUserClaimStore<User, int>, IUserRoleStore<User, int>, IUserSecurityStampStore<User, int>, IUserLockoutStore<User, int>,
                             IUserTwoFactorStore<User, int>, IQueryableUserStore<User, int>
    {
        Task<bool> HasAccessToActionAsync(User user, string controllerName, string actionName);
        Task ValidateRolesAsync(List<string> roles);
    }

    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IRoleRepository _roleRepository;

        public ApplicationUserManager(IUserRepository userRepository, IUserLoginRepository userLoginRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _roleRepository = roleRepository;
        }

        #region IUserStore
        public Task CreateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Add(user);

            return Ok();
        }

        public Task DeleteAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Delete(user);

            return Ok();
        }

        public Task<User> FindByIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentNullException("userId");

            return Task.FromResult<User>(_userRepository.GetById(userId));
        }

        public Task<User> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName");

            return Task.FromResult<User>(_userRepository.Get(u => u.UserName.Equals(userName)));
        }

        public Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Update(user);

            return Ok();
        }
        #endregion

        #region IUserLoginStore
        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (login == null)
                throw new ArgumentNullException("login");

            var l = new UserLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = user
            };

            user.Logins.Add(l);
            _userRepository.Update(user);

            return Ok();
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            var l = _userLoginRepository.Get(ul => ul.ProviderKey.Equals(login.ProviderKey) && ul.LoginProvider.Equals(login.LoginProvider));

            if (l == null)
                return Task.FromResult<User>(null);

            return Task.FromResult<User>(l.User);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<IList<UserLoginInfo>>(user.Logins.Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (login == null)
                throw new ArgumentNullException("login");

            var l = user.Logins.FirstOrDefault(ul => ul.LoginProvider.Equals(login.LoginProvider) && ul.ProviderKey.Equals(login.ProviderKey));

            if (l != null)
            {
                user.Logins.Remove(l);
                _userRepository.Update(user);
            }

            return Ok();
        }
        #endregion

        #region IUserPasswordStore
        public Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<bool>(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Ok();
        }
        #endregion

        #region IUserEmailStore
        public Task<User> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("email");

            var user = _userRepository.Get(u => u.Email.Equals(email));

            return Task.FromResult<User>(user);
        }

        public Task<string> GetEmailAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailAsync(User user, string email)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("email");

            user.Email = email;
            _userRepository.Update(user);

            return Ok();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new InvalidOperationException("Cannot set the confirmation status of the e-mail because user doesn't have an e-mail.");

            user.EmailConfirmed = confirmed;
            _userRepository.Update(user);

            return Ok();
        }
        #endregion

        #region IUserClaimStore
        public Task AddClaimAsync(User user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (claim == null)
                throw new ArgumentNullException("claim");

            var c = new UserClaim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                User = user
            };

            user.Claims.Add(c);
            _userRepository.Update(user);

            return Ok();
        }

        public Task<IList<Claim>> GetClaimsAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<IList<Claim>>(user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(User user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (claim == null)
                throw new ArgumentNullException("claim");

            var c = user.Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

            if (c != null)
            {
                user.Claims.Remove(c);
                _userRepository.Update(user);
            }

            return Ok();
        }
        #endregion

        #region IUserRoleStore
        public Task ValidateRolesAsync(List<string> roles)
        {
            if (roles == null || !roles.Any())
                throw new ArgumentNullException("roles");

            if (!_roleRepository.GetAll().Any(r => roles.Contains(r.Name)))
                throw new ArgumentException("roles");

            return Ok();
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName");

            var r = _roleRepository.Get(ro => ro.Name.Equals(roleName));
            if (r == null)
                throw new ArgumentException("roleName does not correspond to a Role entity", "roleName");

            user.Roles.Add(r);
            _userRepository.Update(user);

            return Ok();
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<IList<string>>(user.Roles.Select(r => r.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role");

            return Task.FromResult<bool>(user.Roles.Any(r => r.Name.Equals(roleName)));
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role");

            var role = user.Roles.FirstOrDefault(r => r.Name.Equals(roleName));

            if (role == null)
                throw new ArgumentException("roleName does not correspond to a Role entity", "roleName");

            user.Roles.Remove(role);
            _userRepository.Update(user);

            return Ok();
        }

        public Task<bool> HasAccessToActionAsync(User user, string controllerName, string actionName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(controllerName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: controller");

            if (string.IsNullOrWhiteSpace(actionName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: action");

            return Task.FromResult<bool>(user.Roles.Any(r => r.Actions.Any(a => a.ControllerName.Equals(controllerName) && a.ActionName.Equals(actionName))));
        }

        #endregion

        #region IUserSecurityStampStore
        public Task<string> GetSecurityStampAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<string>(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(User user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.SecurityStamp = stamp;
            return Ok();
        }
        #endregion

        #region IUserLockoutStore
        public Task<int> GetAccessFailedCountAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(
                user.LockoutEndDateUtc.HasValue ?
                    new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc)) :
                    new DateTimeOffset());
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.AccessFailedCount = 0;
            return Ok();
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.LockoutEnabled = enabled;
            return Ok();
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? null : new DateTime?(lockoutEnd.UtcDateTime);
            return Ok();
        }
        #endregion

        #region IUserTwoFactorStore
        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult<bool>(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.TwoFactorEnabled = enabled;
            return Ok();
        }
        #endregion

        #region IQueryableUserStore
        public IQueryable<User> Users
        {
            get { return _userRepository.GetAll(); }
        }
        #endregion

        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }

        private Task Ok()
        {
            return Task.FromResult(0);
        }
    }
}
