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
      public int Name { get; set; }            

      public TileColor (int name, Color color)
      {
         Color = color;
         Name = name;
      }

      public static TileColor Default ()
      {
         return new TileColor(0, Color.FromColorIndex(ColorMethod.ByAci, 1));
      }
   }
}
