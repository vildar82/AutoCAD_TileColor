using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCAD.Architect.TileColor
{
   // Тип покраски панели
   class PanelTypeColor
   {
      List<Zone> _zones;
      int _number; // уникальный номер типа покраски для типа панелей (записывается в параметр блока панели).
      // нужно как-то определять уникальное сочетание зон (_zones).

      // Сравнение двух типов покраски панели по списку зон.
      public bool TypeCompare(PanelTypeColor ptc)
      {
         // Нужно найти другой способ. Т.к. для SequenceEqual имеет значение положение объектов в списке.
         return _zones.SequenceEqual(ptc._zones);
      }
   }
}
