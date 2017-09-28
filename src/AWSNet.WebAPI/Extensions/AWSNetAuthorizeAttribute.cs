using System.Web.Http;
using System.Linq;

namespace AWSNet.WebAPI.Extensions
{
    public class AWSNetAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (AuthorizeRequest(actionContext))
                return;

            HandleUnauthorizedRequest(actionContext);
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }

        private bool AuthorizeRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
               && !actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
                    return false;
            }

            return true;
        }
    }
}