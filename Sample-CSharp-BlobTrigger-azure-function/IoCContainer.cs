using Microsoft.Extensions.DependencyInjection;
using Sample.CSharp.Common;
using Sample.CSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.CSharp.BlobTrigger.AzureFunctions
{
    /// <summary>
    /// Inversion of Control (IoC) Container
    /// </summary>
    public class IoCContainer
    {
        private static IServiceProvider _provider;


        public static IServiceProvider Create()
        {
            return _provider ?? (_provider = ConfigureServices());
        }

        private static IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IBlobParser<Employee>, BlobParser>();
            return services.BuildServiceProvider();
        }
    }
}
