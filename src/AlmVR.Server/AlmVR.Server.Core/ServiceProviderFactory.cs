using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmVR.Server.Core
{
    public class ServiceProviderFactory
    {
        private static bool _containerCreated;

        public static IServiceProvider GetServiceProvider(IServiceCollection services)
        {
            if (_containerCreated)
                throw new NotSupportedException("GetServiceProvider can only be called once.");

            var builder = new ContainerBuilder();
            //builder.RegisterType<PostRepository>().As<IPostRepository>();
            //builder.RegisterType<SiteAnalyticsServices>();
            builder.Populate(services);
            var container = builder.Build();
            //Create the IServiceProvider based on the container.

            _containerCreated = true;

            return new AutofacServiceProvider(container);
        }
    }
}
