using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCAD.Architect.TileColor.Model
{
   // Тип панели (имя блока, список зон, типы покраски
   public class PanelType
   {
      string _blockName; // Имя блока (тип панели).
      List<PanelTypeColor> _typeColor; // Тип покраски панели
      
      public string BlockName
      {
         get { return _blockName; }
      }

      public PanelType(string blockName)
      {
         _blockName = blockName;
      }
   }
}
