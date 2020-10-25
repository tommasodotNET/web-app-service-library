using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WebAppServiceLibrary.Attributes;

namespace WebAppServiceLibrary.Extensions
{
   internal static class ServiceCollectionExtensions
   {
      internal static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
      {
         List<Type> optionTypes = ReflectionExtensions.GetTypesWithHelpAttribute(typeof(OptionAttribute));

         foreach (Type optionType in optionTypes)
         {
            OptionAttribute optionAttribute = optionType.GetTypeInfo().GetCustomAttribute<OptionAttribute>();

            var method = typeof(ServiceCollectionExtensions).GetRuntimeMethods().Where(m => m.Name == "ConfigureOptionsFromSection").SingleOrDefault();
            method = method.MakeGenericMethod(optionType);
            object[] parameters = new object[] { services, configuration, optionAttribute.SectionName };
            method.Invoke(null, parameters);
         }

         return services;
      }

      private static IServiceCollection ConfigureOptionsFromSection<T>(IServiceCollection services, IConfiguration configuration, string sectionName)
          where T : class
      {
         services.Configure<T>(options => configuration.GetSection(sectionName).Bind(options));
         Console.WriteLine($"Added option class bonded to section {sectionName}.");

         return services;
      }

      internal static IServiceCollection AddServicesWithReflection(this IServiceCollection services)
      {
         List<Type> serviceTypes = ReflectionExtensions.GetTypesWithHelpAttribute(typeof(ServiceAttribute));

         foreach (Type serviceType in serviceTypes)
         {
            ServiceAttribute serviceAttribute = serviceType.GetTypeInfo().GetCustomAttribute<ServiceAttribute>();

            ServiceDescriptor serviceDescriptor;
            if (serviceAttribute.InterfaceType != null)
            {
               serviceDescriptor = new ServiceDescriptor(serviceAttribute.InterfaceType, serviceType, serviceAttribute.ServiceLifetime);
               Console.WriteLine($"Added service {serviceAttribute.InterfaceType.ToString()} implemented in {serviceType}.");
            }
            else
            {
               serviceDescriptor = new ServiceDescriptor(serviceType, serviceType, serviceAttribute.ServiceLifetime);
               Console.WriteLine($"Added service {serviceType.ToString()}.");
            }


            services.Add(serviceDescriptor);
         }

         return services;
      }

      internal static IServiceCollection AddDbContextWithReflection(this IServiceCollection services, IConfiguration configuration)
      {
         List<Type> databaseTypes = ReflectionExtensions.GetTypesWithHelpAttribute(typeof(DatabaseAttribute));

         foreach (var databaseType in databaseTypes)
         {
            DatabaseAttribute databaseAttribute = databaseType.GetTypeInfo().GetCustomAttribute<DatabaseAttribute>();

            var method = typeof(ServiceCollectionExtensions).GetRuntimeMethods().Where(m => m.Name == "AddDbCustom").SingleOrDefault();
            method = method.MakeGenericMethod(databaseType);
            object[] parameters = new object[] { services, configuration, databaseAttribute.ConnectionString };
            method.Invoke(null, parameters);
         }

         return services;
      }

      private static IServiceCollection AddDbCustom<T>(IServiceCollection services, IConfiguration configuration, string connectionString)
          where T : DbContext
      {
         services.AddDbContext<T>(options => options.UseSqlServer(configuration.GetConnectionString(connectionString)));
         Console.WriteLine($"Added database with connection string {configuration.GetConnectionString(connectionString)}.");

         return services;
      }
   }
}
