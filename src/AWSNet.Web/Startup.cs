using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AWSNet.Web.Startup))]
namespace AWSNet.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
