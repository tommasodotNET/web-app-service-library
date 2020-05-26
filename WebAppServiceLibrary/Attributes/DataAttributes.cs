using System;
using System.Collections.Generic;
using System.Text;

namespace WebAppServiceLibrary.Attributes
{
   /// <summary>
   /// Attribute used to describe a DbContext derived class.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class)]
   public class DatabaseAttribute : Attribute
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="connectionString">The connection string name in appsettings file</param>
      public DatabaseAttribute(string connectionString = "Database")
      {
         ConnectionString = connectionString;
      }

      public string ConnectionString { get; set; }
   }
}
