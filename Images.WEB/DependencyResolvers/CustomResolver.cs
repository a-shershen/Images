using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;

namespace Images.WEB.DependencyResolvers
{
    public class CustomResolver : System.Web.Mvc.IDependencyResolver
    {
        private Ninject.IKernel kernel;

        public CustomResolver(Ninject.IKernel iKernel)
        {
            kernel = iKernel;
            Binds();
        }

        public object GetService(Type serviceType)
        {
            return kernel.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void Binds()
        {
            kernel.Bind<Images.Exif.Lib.Interfaces.IExifService>()
                .To<Images.Exif.Lib.Services.ExifService>();
        }
    }
}