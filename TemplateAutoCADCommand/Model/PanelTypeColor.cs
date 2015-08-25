using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoCAD.Architect.TileColor
{
   // Тип покраски панели
   public class PanelTypeColor
   {
      public List<Panel> Panels;
      public List<Zone> Zones;
      public int Number; // уникальный номер типа покраски для типа панелей (записывается в параметр блока панели).
      // нужно как-то определять уникальное сочетание зон (_zones).
      public Image Preview;

      public PanelTypeColor (int number)
      {
         Number = number;
         Panels = new List<Panel>();           
      }

      private PanelTypeColor()
      { }

      // Сравнение двух типов покраски панели по списку зон.
      public bool TypeCompare(PanelTypeColor ptc)
      {
         // Нужно найти другой способ. Т.к. для SequenceEqual имеет значение положение объектов в списке.(или делать сортировку).
         return Zones.SequenceEqual(ptc.Zones);
      }

      /// <summary>
      /// Попытка определения типа покраски из блока панели
      /// </summary>      
      public static PanelTypeColor GetTypeColor(BlockReference blRef)
      {
         PanelTypeColor ptc = new PanelTypeColor();
         ptc.Zones = Zone.GetZones(blRef);
         ptc.Number = GetDynParamTypeColor(blRef);
         return ptc;
      }

      private static int GetDynParamTypeColor(BlockReference blRef)
      {
         int res = 0;
         var dynParams = blRef.DynamicBlockReferencePropertyCollection;
         foreach (DynamicBlockReferenceProperty dynProp in dynParams)
         {
            if (dynProp.PropertyName == Options.DynPropTypeColor)
            {
               try
               {
                  res = int.Parse(dynProp.Value.ToString());
               }
               catch
               {                  
               }               
               break;
            }
         }
         return res;
      }

      public void GetColor(Dictionary<string, TileColor> colors)
      {
         //TODO определение цветов для типов цветов по цветам плитки, используя первую панель в списке Panels.
         var typeColorInPanel = Zones.GroupBy(zone => zone.TypeColor).Select(z => z.Key);
                  
      }
   }
}
