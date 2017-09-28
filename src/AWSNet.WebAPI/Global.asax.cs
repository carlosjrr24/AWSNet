using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using AWSNet.DependencyInjection;
using AWSNet.Managers;
using AWSNet.Repositories;
using AWSNet.Utils.Configuration;
using AWSNet.Utils.Logging;
using AWSNet.Utils.Solr;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace AWSNet.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;
        private static System.Timers.Timer SyncLayoutInstancesTimer;

        internal static IWindsorContainer Container
        {
            get
            {
                return _container;
            }
        }

        protected void Application_Start()
        {
            ConfigureWindsor(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configure(c => WebApiConfig.Register(c, _container));
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }

        protected void Application_Error()
        {
            var exc = Server.GetLastError();
            AWSNetLogger.Error(exc.Message);
        }

        protected void Application_End()
        {
            _container.Dispose();
            base.Dispose();
        }

        protected void Application_BeginRequest()
        {
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {
                Response.Flush();
            }
        }

        public static void ConfigureWindsor(HttpConfiguration configuration)
        {
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));
            IDependencyResolver dependencyResolver = new WindsorDependencyResolver(_container);
            configuration.DependencyResolver = dependencyResolver;
        }

    }

}
