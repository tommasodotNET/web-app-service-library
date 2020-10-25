using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebAppServiceLibrary.Extensions
{
   public static class ReflectionExtensions
   {
      public static IEnumerable<Type> GetAllTypesImplementing(Type implementedInterface)
      {
         var types = GetAllAssemblyTypes()
             .Where
             (t =>
                 !t.GetTypeInfo().IsInterface &&
                 !t.GetTypeInfo().IsAbstract &&
                 t.GetTypeInfo().ImplementedInterfaces.Contains(implementedInterface)
             )
             .ToList();

         return types;
      }

      public static Type[] GetAllAssemblyTypes()
      {
         return Assembly.GetEntryAssembly().GetTypes();
      }

      public static List<Type> GetTypesWithHelpAttribute(Type customAttributeType)
      {
         List<Type> result = new List<Type>();

         foreach (Type type in GetAllAssemblyTypes())
         {
            if (type.GetTypeInfo().GetCustomAttribute(customAttributeType, true) != null)
            {
               result.Add(type);
            }
         }

         return result;
      }

      /// <summary>
      /// Get property value for an object given the property name
      /// </summary>
      /// <param name="obj">Object to retrieve the property value from</param>
      /// <param name="propName">Property name</param>
      /// <returns>Property value</returns>
      public static object GetProp(this object obj, string propName)
      {
         return obj.GetType().GetProperty(propName).GetValue(obj);
      }

      /// <summary>
      /// Set property value for an object given the property name
      /// </summary>
      /// <param name="obj">Object to set the property value of</param>
      /// <param name="propName">Property name</param>
      /// <param name="value">Property value</param>
      public static void SetProp(this object obj, string propName, object value)
      {
         obj.GetType().GetProperty(propName).SetValue(obj, value, null);
      }

      /// <summary>
      /// Assign an object's property to another object
      /// solamente se è cambiata
      /// </summary>
      /// <param name="oldObj">Object with the property</param>
      /// <param name="newObj">Object to assign the property to</param>
      /// <param name="prop">Property name</param>
      public static void AssignIfChanged(this object oldObj, object newObj, string prop)
      {
         object oldProp = oldObj.GetProp(prop);
         object newProp = newObj.GetProp(prop);

         if ((oldProp == null && newProp != null) || (oldProp != null && !oldProp.Equals(newProp)))
         {
            oldObj.SetProp(prop, newProp);
         }
      }
   }
}
