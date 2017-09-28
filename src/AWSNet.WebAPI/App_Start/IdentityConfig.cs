using Microsoft.AspNet.Identity;
using AWSNet.Model;
using AWSNet.Managers;

namespace AWSNet.WebAPI
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<User, int>
    {
        public new IApplicationUserManager Store
        {
            get
            {
                return base.Store as IApplicationUserManager;
            }
        }

        public ApplicationUserManager(IApplicationUserManager applicationUserManager)
            : base(applicationUserManager)
        {
            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<User, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            var dataProtectionProvider = Startup.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                this.UserTokenProvider =
                    new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }

        public static ApplicationUserManager GetInstance()
        {
            return WebApiApplication.Container.Resolve<ApplicationUserManager>();
        }
    }
}
