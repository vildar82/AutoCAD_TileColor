using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCAD.Architect.TileColor
{
   public static class Options
   {
      // Типы панелей
      public static List<string> TypePanels
      {
         get
         {
            return new List<string>
            {
               "ПП_4ОК_66-75"
            };
         }
      }

      public static string DynPropTypeColor
      {
         get
         {
            return "Окраска";
         }
      }
   }
}
