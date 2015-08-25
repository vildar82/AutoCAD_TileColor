using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutoCAD.Architect.TileColor
{
   public static class Select
   {
      // Выбор блоков панелей в чертеже.
      public static List<ObjectId> GetBlockRefPanels(Document doc)
      {         
         List<ObjectId> ids = new List<ObjectId>();

         Editor ed = doc.Editor;
         Database db = doc.Database;
         var prOpt = new PromptSelectionOptions();
         prOpt.MessageForAdding = "Выбор блоков панелей";         
         var selRes = ed.GetSelection(prOpt);
         if (selRes.Status != PromptStatus.OK)
            return ids;

         // Фильтр блоков панелей (по имени блоков).
         using (var t = db.TransactionManager.StartTransaction())
         {
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
         return ids;
      }
      
      // Определение имени блока (для дин блоков неанонимное имя)
      public static string GetEffectiveBlockName (BlockReference blref)
      {
         string res = string.Empty; 
         if (blref.IsDynamicBlock)
         {
            using (var btr = blref.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord)
            {
               res = btr.Name;               
            }
         }
         else
         {
            res = blref.Name; 
         }
         return res;
      }
   }
}
