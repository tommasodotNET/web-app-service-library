using System;
using System.Collections.Generic;
using System.Text;

namespace WebAppServiceLibrary.Attributes
{
   /// <summary>
   /// Attribute used to describe an option class.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class)]
   public class OptionAttribute : Attribute
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="sectionName">The section name to be binded</param>
      public OptionAttribute(string sectionName = "")
      {
         SectionName = sectionName;
      }

      public string SectionName { get; set; }
   }
}
