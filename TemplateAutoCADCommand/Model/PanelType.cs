using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoCAD.Architect.TileColor
{
   public class PanelType
   {
      // Типы покраски панелей
      public List<PanelTypeColor> PanelTypeColors;
      public string Name; // тип панели (по имени блока)

      public PanelType (string name)
      {
         Name = name;
         PanelTypeColors = new List<PanelTypeColor>();
      }                 
   }
}
