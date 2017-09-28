using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AWSNet.Model
{
    public partial class User : IUser<int>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here

            return userIdentity;
        }

        partial void OnCreated()
        {
            CreationDate = DateTime.UtcNow;
        }
    }
}
