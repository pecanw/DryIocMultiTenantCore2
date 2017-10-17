using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using DryIocCore2.TenantSupport.AspNetCore;
using DryIocCore2.TenantSupport.AspNetCore.TenantResolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DryIocCore2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddControllersAsServices();

            var resolver = new FallbackTenantResolver(new HeaderTenantResolver(), "t1");
            services.AddTenantSupport(resolver);

            var container = new Container(rules => rules.With(propertiesAndFields: PropertiesAndFields.Of))
                // optional: support for MEF service discovery
                //.WithMef()
                // setup DI adapter
                .WithDependencyInjectionAdapter(services,
                    // optional: get original DryIoc.ContainerException if specified type is not resolved, 
                    // and prevent fallback to default resolution by infrastructure
                    throwIfUnresolved: type => type.Name.EndsWith("Controller"));
/*
 ,

                    // optional: You may Log or Customize the infrastructure components registrations
                    registerDescriptor: (registrator, descriptor) =>
                    {
#if DEBUG
                        if (descriptor.ServiceType == typeof(ILoggerFactory))
                            Console.WriteLine($"Logger factory is registered as instance: {descriptor.ImplementationInstance != null}");
#endif
                        return false; // fallback to default registration logic
                    });
*/

            // Your registrations are defined in CompositionRoot class
            var serviceProvider = container.ConfigureServiceProvider<CompositionRoot>();

            /*
            var r = container.Resolve<TenantContainerManager>();

            var t1Container = r.GetTenantContainer("t1");
            var t2Container = r.GetTenantContainer("t2");
            var t3Container = r.GetTenantContainer("t3");
             */

            return serviceProvider;
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseTenantSupport();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

        }

        private static IEnumerable<PropertyOrFieldServiceInfo> DeclaredPublicProperties(Request request)
        {
            return (request.ImplementationType ?? request.ServiceType).GetTypeInfo()
                .DeclaredProperties.Where(p => p.IsInjectable())
                .Select(PropertyOrFieldServiceInfo.Of);
        }

    }
}
