using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Castle.Windsor;
using System.Web.Http.Dispatcher;
using AWSNet.DependencyInjection;
using System.Web.Http.Cors;

namespace AWSNet.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IWindsorContainer container)
        {
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            MapRoutes(config);
            RegisterControllerActivator(container);
        }

        private static void MapRoutes(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new AWSNet.WebAPI.Extensions.AWSNetAuthorizeActionFilter());

            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors();
        }

        private static void RegisterControllerActivator(IWindsorContainer container)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(container));
        }
    }
}
