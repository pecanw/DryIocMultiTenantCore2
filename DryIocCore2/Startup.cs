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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            var container = new Container()
                .WithDependencyInjectionAdapter(services,
                    throwIfUnresolved: type => type.Name.EndsWith("Controller"));

            // Otpion 1 - register custom scope factory
            //container.Register<IServiceScopeFactory, MultiTenantDryIocServiceScopeFactory>(Reuse.Singleton, ifAlreadyRegistered: IfAlreadyRegistered.Replace);

            // Your registrations are defined in CompositionRoot class
            var serviceProvider = container.ConfigureServiceProvider<CompositionRoot>();

            return serviceProvider;
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseTenantSupport();

            // Otpion 2 - replace HttpContext.RequestServices
            app.UseMiddleware<DryIocTenantSupportMiddleware>();

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
