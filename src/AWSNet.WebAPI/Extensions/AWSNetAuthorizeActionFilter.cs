using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Http;

namespace AWSNet.WebAPI.Extensions
{
    public class AWSNetAuthorizeActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
               && !actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
                {
                    var userManager = ApplicationUserManager.GetInstance();
                    var user = userManager.FindByIdAsync(actionContext.RequestContext.Principal.Identity.GetUserId<int>()).Result;

                    if (user == null || !user.IsEnabled || !userManager.Store.HasAccessToActionAsync(user,
                                                           actionContext.ControllerContext.ControllerDescriptor.ControllerName,
                                                           actionContext.ActionDescriptor.ActionName).Result)
                    {
                        actionContext.Response = CreateUnauthorizedResponse(actionContext);
                    }
                }
                else
                {
                    actionContext.Response = CreateUnauthorizedResponse(actionContext);
                }
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

        private HttpResponseMessage CreateUnauthorizedResponse(HttpActionContext actionContext)
        {
            return actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authorization has been denied for this request.");
        }
    }
}