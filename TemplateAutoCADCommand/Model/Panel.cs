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
      public List<Zone> Zones;
      public ObjectId IdBlRef;       

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
   }
}
