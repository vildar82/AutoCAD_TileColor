using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoCAD.Architect.TileColor
{
   public class TileColor
   {
      public Color Color;
      public string TypeColor { get; set; }      

      public static void AddTypeColor(string typeColor)
      {
         if (!Contains(typeColor))
         {
            TileColor tileColor = new TileColor();
            tileColor.TypeColor = typeColor;
            TileColors.Add(tileColor);
         }         
      }
      private static bool Contains (string typeColor)
      {         
         foreach (var item in TileColors)
         {
            if (item.TypeColor == typeColor)
            {
               return true;
            }
         }
         return false;
      }      
   }
}
