using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCAD.Architect.TileColor
{
   public static class Options
   {
      // Список блоков панелей (типов панелей)
      public static List<string> TypePanels
      {
         get { return new List<string> { "ПП_4ОК_66-75" }; }
      }

      // Динамическое свойтво "Окраски" блока панели
      public static string DynPropTypeColor
      {
         get { return "Окраска"; }
      }

      // Имя блока плитки
      public static string TileBlockName
      {
         get { return "плитка"; }
      }

      // Тег атрибута зоны у блока плитки
      public static string TileBlockZoneAttrTag
      {
         get { return "ЗОНА"; }
      }
   }
}
