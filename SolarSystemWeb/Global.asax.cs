using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DataLayer;
using DataLayer.Repositories;
using Microsoft.Practices.Unity;
using SolarSystemWeb.Models.Application;
using SolarSystemWeb.Models.Entities;
using SolarSystemWeb.Models.Repositories;

namespace SolarSystemWeb
{
    public class MvcApplication : HttpApplication
    {
        private readonly UnityContainer _container = new UnityContainer();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           
            RegisterTypes();            
            ControllerBuilder.Current.SetControllerFactory(new CustomControllerFactory(_container));
        }

        protected void RegisterTypes()
        {
            _container.RegisterType<ICrudRepository<SpaceObjectDto, SpaceObject>, SpaceObjectRepository> (new ContainerControlledLifetimeManager());
            _container.RegisterType<ICrudRepository<SpaceObjectTypeDto, SpaceObjectType>, SpaceObjectTypeRepository> (new ContainerControlledLifetimeManager());            
        }
    }
}
