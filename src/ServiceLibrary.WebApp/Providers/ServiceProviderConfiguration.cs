using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceLibrary.Model.Extensions;

namespace ServiceLibrary.WebApp.Providers
{
   public static class ServiceProviderConfiguration
   {
      public static IServiceCollection ConfigureService(this IServiceCollection services, 
         IConfiguration configuration)
      {
         Console.WriteLine("Configuring services...");

         services.AddServicesWithReflection()
             .ConfigureOptions(configuration)
             .AddDbContextWithReflection(configuration)
             .BuildServiceProvider();

         Console.WriteLine("Done.");

         return services;
      }
   }
}
