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
            
      ObjectId _idBlRef;// id блока панели (BlockReference).
      string _name; // имя блока панели (тип панели, например ПП_4ОК_66-75).

      public Panel(BlockReference blRef, string blName)
      {
         _idBlRef = blRef.ObjectId;
         Name = blName;     
         // Сбор сведений о панели из блока
         // А что нужно?
         // Пока допустим, что ничего.
         // В идеале, нужно определить тип покраски панели. (зоны и их цвета).
      }

      public string Name
      {
         get { return _name; }
         private set { _name = value; }
      }

      public ObjectId IdBlRef
      {
         get { return _idBlRef; }
      }

      public static List<Panel> GetAllPanelInModel()
      {
         List<Panel> panels = new List<Panel>();
         Database db = HostApplicationServices.WorkingDatabase;
         using (var t = db.TransactionManager.StartTransaction())
         {
            var bt = t.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            var ms = t.GetObject(bt[SymbolUtilityServices.BlockModelSpaceName], OpenMode.ForRead) as BlockTableRecord;

            foreach (ObjectId idEnt in ms)
            {
               if (idEnt.ObjectClass.Name == "AcDbBlockReference")
               {
                  var blRef = t.GetObject(idEnt, OpenMode.ForRead) as BlockReference;                  
                  string blName = Select.GetEffectiveBlockName(blRef);
                  // Только блоки с именами типов панелей
                  if (Options.TypePanels.Contains(blName))
                  {
                     // Создание панели
                     Panel panel = new Panel(blRef, blName);
                     // Добавление в коллекцию.
                     panels.Add(panel);
                  }
               }
            }
            t.Commit();
         }
         return panels;
      }
   }
}
