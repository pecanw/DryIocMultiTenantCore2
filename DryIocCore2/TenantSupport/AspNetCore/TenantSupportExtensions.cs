using System;
using DryIocCore2.TenantSupport.AspNetCore.TenantResolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DryIocCore2.TenantSupport.AspNetCore
{
    /// <summary>
    /// Extension methods for setting up tenant support />.
    /// </summary>
    public static class TenantSupportExtensions
    {
        /// <summary>
        /// Adds Tenant support services to the specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// <remarks>If the <see cref="ITenantResolver"/> service is not registered, the <see cref="HeaderTenantResolver"/> is used</remarks>
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        public static void AddTenantSupport(this IServiceCollection services)
        {
            services.TryAddSingleton<ITenantResolver, HeaderTenantResolver>();
            services.AddSingleton<ITenantProvider, TenantProvider>();
            services.AddSingleton<TenantSupportMarker>();
        }

        /// <summary>
        /// Adds Tenant support services to the specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TResolver">The <see cref="ITenantResolver"/> implementation</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        public static void AddTenantSupport<TResolver>(this IServiceCollection services)
            where TResolver : class, ITenantResolver
        {
            services.AddSingleton<ITenantResolver, TResolver>();
            services.AddSingleton<ITenantProvider, TenantProvider>();
            services.AddSingleton<TenantSupportMarker>();
        }

        /// <summary>
        /// Adds Tenant support services to the specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        /// <param name="resolver">An <see cref="ITenantResolver"/> instance to be used as tenant resolver</param>
        public static void AddTenantSupport(this IServiceCollection services, ITenantResolver resolver)
        {
            services.AddSingleton<ITenantResolver>(resolver);
            services.AddSingleton<ITenantProvider, TenantProvider>();
            services.AddSingleton<TenantSupportMarker>();
        }

        /// <summary>
        /// Adds tenant support to the <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseTenantSupport(this IApplicationBuilder app)
        {
            if (app.ApplicationServices.GetService(typeof(TenantSupportMarker)) == null)
            {
                throw new InvalidOperationException($"Tenant support services not registered. Did you forget to call services.{nameof(AddTenantSupport)}?");
            }

            return app.UseMiddleware<TenantSupportMiddleware>();
        }
    }
}