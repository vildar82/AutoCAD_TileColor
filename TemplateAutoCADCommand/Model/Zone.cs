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
      public string Name;
      public string TypeColor;

      public override bool Equals(object obj)
      {
         Zone zone = obj as Zone;
         return Name.Equals(zone.Name) && TypeColor.Equals(zone.TypeColor);
      }

      public static List<Zone> GetZones(BlockReference blRef)
      {
         List<Zone> zones = new List<Zone>();

         Database db = HostApplicationServices.WorkingDatabase;

         using (var t = db.TransactionManager.StartTransaction () )
         {            
            foreach (ObjectId id in blRef.AttributeCollection)
            {
               var attRef = t.GetObject(id, OpenMode.ForRead) as AttributeReference;
               // Если тег атрибута начинается с з, то это номер зоны, значение атрибута это тип цвета
               if (attRef.Tag.StartsWith ("з") )
               {
                  Zone zone = new Zone();
                  zone.Name = attRef.Tag.Substring(1);
                  zone.TypeColor = attRef.TextString;
                  TileColor.AddTypeColor(zone.TypeColor);
                  zones.Add(zone);
               }
            }
            t.Commit(); 
         } 
         return zones;
      }      
   }
}
