using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TechPodcastNightDemoBot.Modules;

namespace TechPodcastNightDemoBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new DialogModule());
            builder.RegisterModule(new MainModule());

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();

            var config = GlobalConfiguration.Configuration;
            builder.RegisterWebApiFilterProvider(config);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
