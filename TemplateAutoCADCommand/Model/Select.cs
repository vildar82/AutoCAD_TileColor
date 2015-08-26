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
      public static string GetEffectiveBlockName(BlockReference blref)
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

      public static List<ObjectId> SelectBlocks(string msg, string name)
      {
         List<ObjectId> idBlRef = new List<ObjectId>();
         Document doc = Application.DocumentManager.MdiActiveDocument;
         Editor ed = doc.Editor;

         SelectionFilter filter = new SelectionFilter(CreateFilterListForBlocks(DymBlockAllNames(name)));          
         var prOpt = new PromptSelectionOptions();
         prOpt.MessageForAdding = msg;         

         var selRes = ed.GetSelection(prOpt, filter);
         if (selRes.Status == PromptStatus.OK)
         {
            idBlRef = selRes.Value.GetObjectIds().ToList();
         }
         return idBlRef;
      }

      private static List<string> DymBlockAllNames(string blName)
      {
         List<string> blNames = new List<string>();
         blNames.Add(blName);
         Database db = HostApplicationServices.WorkingDatabase;
         using (var t = db.TransactionManager.StartTransaction())
         {
            var bt = t.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            var btrOrig = t.GetObject(bt[blName], OpenMode.ForRead) as BlockTableRecord;
            if (btrOrig.IsDynamicBlock)
            {
               var anonimsBtr = btrOrig.GetAnonymousBlockIds();
               foreach (ObjectId idBtrAn in anonimsBtr)
               {
                  var btrAn = t.GetObject(idBtrAn, OpenMode.ForRead) as BlockTableRecord;
                  blNames.Add(btrAn.Name);
               }
            }
            t.Commit();
         }
         return blNames;
      }

      private static TypedValue[] CreateFilterListForBlocks(List<string> blkNames)
      {
         if (blkNames.Count == 0)
            return null;

         if (blkNames.Count == 1)
            return new TypedValue[] { new TypedValue((int)DxfCode.BlockName, blkNames[0]) };

         List<TypedValue> tvl = new List<TypedValue>(blkNames.Count + 2);

         tvl.Add(new TypedValue((int)DxfCode.Operator, "<or"));

         foreach (var blkName in blkNames)
         {
            tvl.Add(new TypedValue((int)DxfCode.BlockName, (blkName.StartsWith("*") ? "`" + blkName : blkName)));
         }
         tvl.Add(new TypedValue((int)DxfCode.Operator, "or>"));
         return tvl.ToArray();
      }
   }
}

