﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace SolarSystemWeb.Models.Application
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        public CustomControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                if (controllerType == null)
                    throw new ArgumentNullException($"Parameter \"{nameof(controllerType)}\" is null");

                if (!typeof(IController).IsAssignableFrom(controllerType))
                    throw new ArgumentException($"Type \"{nameof(controllerType)}\" is not controller");

                return _container.Resolve(controllerType) as IController;
            }
            catch
            {
                return null;
            }

        }

        public override void ReleaseController(IController controller)
        {            
            var disposable = controller as IDisposable;
            disposable?.Dispose();
        }
    }
}