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
      public List<Zone> Zones;      

      public int Number { get; set; }

      private PanelTypeColor ()
      {                 
      }
      
      // Сравнение двух типов покраски панели по списку зон.
      public bool TypeCompare(PanelTypeColor ptc)
      {
         // Нужно найти другой способ. Т.к. для SequenceEqual имеет значение положение объектов в списке.(или делать сортировку).
         return Zones.SequenceEqual(ptc.Zones);
      }

      // Дефолтный тип окраски
      public static PanelTypeColor Default(List<Zone> zonesTemplate, int number, TileColor color)
      {
         PanelTypeColor ptcDef = new PanelTypeColor();
         ptcDef.Number = number;
         // Копирование зон из шаблона
         ptcDef.Zones = new List<Zone>();
         foreach (Zone zone in zonesTemplate)
         {
            Zone zoneCopy = new Zone();
            zoneCopy.Name = zone.Name;
            zoneCopy.ZoneColor = color;            
            ptcDef.Zones.Add(zoneCopy);
         }         
         return ptcDef; 
      }

      /// <summary>
      /// Попытка определения типа покраски из блока панели
      /// </summary>      
      public static PanelTypeColor GetTypeColor(BlockReference blRef)
      {
         throw new NotImplementedException();
         //PanelTypeColor ptc = new PanelTypeColor();
         //ptc.Zones = Zone.GetZones(blRef);
         //ptc.Number = GetDynParamTypeColor(blRef);
         //return ptc;
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
         throw new NotImplementedException();
      }

      // Определение зоны по номеру зоны
      public Zone GetZone(string zoneNumber)
      {
         var zone = Zones.Find(z => z.Name == zoneNumber);
         return zone;
      }
   }
}
