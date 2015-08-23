using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoCAD.Architect.TileColor
{
   public class Panel
   {
      // Panel - объект панели для каждого блока(ref) панели в чертеже (с уникальным objectId).

      List<Zone> _zones;
      ObjectId _idBlRef;       

      public static List<Panel> GetPanels(List<ObjectId> idBlRefPanels)
      {
         List<Panel> panels = new List<Panel>();

         Database db = HostApplicationServices.WorkingDatabase;
         using (var t = db.TransactionManager.StartTransaction())
         {
            foreach (var id in idBlRefPanels)
            {
               var blRefPanel = t.GetObject(id, OpenMode.ForRead) as BlockReference;
               Panel panel = new Panel();
               // Получение списка зон из атрибута блока панели.
               panel.Zones = Zone.GetZones(blRefPanel);
               panel.IdBlRef = id;
               panels.Add(panel);
            }            
            t.Commit();
         } 
         return panels;
      }

      public void Paint()
      {
         Database db = HostApplicationServices.WorkingDatabase;

         using (var t = db.TransactionManager.StartTransaction())
         {
            var blRefPanel = t.GetObject(IdBlRef, OpenMode.ForRead) as BlockReference;
            var btr = t.GetObject(blRefPanel.BlockTableRecord, OpenMode.ForWrite) as BlockTableRecord;
            foreach (ObjectId idEnt in btr)
            {
               if (idEnt.ObjectClass.Name == "AcDbBlockReference")
               {
                  var blRef = t.GetObject(idEnt, OpenMode.ForWrite) as BlockReference;
                  if (Select.GetEffectiveBlockName(blRef) == "плитка")
                  {
                     PaintTile(blRef);
                  }
               }
            }             
            t.Commit();
         }
      }

      private void PaintTile(BlockReference blRefTile)
      {
         string zoneTileAtr = GetTileZone(blRefTile);
         // Получение цвета по типу зоны
         string zoneType = GetTypeColor(zoneTileAtr);
         // Получение цвета по типу цвета
         Color color = TileColor.GetColor(zoneType);
         if (color == null)
            return;
         blRefTile.Color = color;
      }

      private string GetTileZone(BlockReference blRefTile)
      {
         foreach (ObjectId idAtr in blRefTile.AttributeCollection)
         {
            var atrRef = idAtr.GetObject(OpenMode.ForRead) as AttributeReference;
            string textAtr = atrRef.TextString;
            string tagAtr = atrRef.Tag;
            atrRef.Close();
            if (tagAtr == "ЗОНА")
            {
               return textAtr;
            }
         }
         return "";
      }

      private string GetTypeColor(string zoneName)
      {
         string res = string.Empty; 
         foreach (var zone in Zones)
         {
            if (zone.Name == zoneName)
            {
               res =zone.TypeColor;  
            }
         }
         return res;
      }

      internal static List<Panel> GetAllPanelInModel()
      {
         //TODO Поиск всех панелей в Модели.
         // Определение всех параметров панели:
         //    Тип панели - по имени блока (вида ПП_4ОК_66-75). Имена блоков заданы. Пока одно имя ПП_4ОК_66-75.
         //    Список зон (номера и типы покраски). Могут быть не заданы, если этот блок раньше еще не красился.
         //    Тип покраски - если для зон заданы типы покраски. (ориентируясь на атрибуты зон, а не на цвета самих плиток в блоке панели).
         //    Определение списка цветов проекта, если он пустой. По цветам плиток для типов зон.

         List<Panel>
         Database db = HostApplicationServices.WorkingDatabase;
         
         using (var t = db.TransactionManager.StartTransaction())
         {
            var bt = t.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            var ma = t.GetObject (SymbolUtilityServices.BlockModelSpaceName)

            foreach (ObjectId idEnt in selRes.Value.GetObjectIds())
            {
               if (idEnt.ObjectClass.Name == "AcDbBlockReference")
               {
                  var blRef = t.GetObject(idEnt, OpenMode.ForRead) as BlockReference;
                  // Пока выбираем только блок с именем "НС4_72-75"
                  if (GetEffectiveBlockName(blRef) == "НС4_72-75")
                  {
                     ids.Add(idEnt);
                  }
               }
            }
            t.Commit();
         }
      }
   }
}
