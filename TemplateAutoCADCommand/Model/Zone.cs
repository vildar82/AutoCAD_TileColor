using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoCAD.Architect.TileColor
{
   public class Zone
   {      
      /// <summary>
      /// Имя зоны - тег зоны, без "з".
      /// </summary>
      public string Name;      
      /// <summary>
      /// Назначенный цвет зоне для покраски.
      /// </summary>
      public TileColor ZoneColor;      

      public override bool Equals(object obj)
      {
         Zone zone = obj as Zone;
         // ???
         return Name.Equals(zone.Name) && ZoneColor.Equals(zone.ZoneColor);
      }
      public override int GetHashCode()
      {
         return Name.GetHashCode();
      }

      // Сортировка списка зон
      private static void Sort(List<Zone> zones)
      {
         zones.Sort((x, y) => string.Compare(x.Name, y.Name));
      }

      public static List<Zone> GetZones(ObjectId idBlRef, bool defineColor)
      {
         List<Zone> zones = new List<Zone>();
         Database db = HostApplicationServices.WorkingDatabase;

         using (var t = db.TransactionManager.StartTransaction () )
         {
            var blRef = t.GetObject(idBlRef, OpenMode.ForRead) as BlockReference;
            foreach (ObjectId id in blRef.AttributeCollection)
            {
               var attRef = t.GetObject(id, OpenMode.ForRead) as AttributeReference;
               // Если тег атрибута начинается с з, то это номер зоны, значение атрибута это тип цвета
               if (attRef.Tag.StartsWith ("з") )
               {
                  Zone zone = new Zone();
                  zone.Name = attRef.Tag.Substring(1);
                  if (defineColor)
                  {
                     // Определение покраски плитки ZoneColor?
                     // ?      
                     throw new NotImplementedException();      
                  }
                  zones.Add(zone);
               }
            }
            t.Commit(); 
         }
         Sort(zones);
         return zones;
      }      
   }
}
