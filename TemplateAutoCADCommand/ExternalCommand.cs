using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(AutoCAD.Architect.TileColor.ExternalCommand))]

namespace AutoCAD.Architect.TileColor
{
   public class ExternalCommand
   {
      [CommandMethod("TileColor")]
      public void TileColorCommand()
      {
         var doc = Application.DocumentManager.MdiActiveDocument;
         var db = doc.Database;
         var ed = doc.Editor;

         ed.WriteMessage("\nПробная версия покраски панели. Панель это блок с именем НС4_72-75, и с атрибутами зон с тегом начинающимся с 'з'. В блоке панели разложены блоки плитки с именем 'плитка' и с атрибутом зоны.");

         using (var t = db.TransactionManager.StartTransaction())
         {
            //get blockTable
            var bt = t.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            //get ModelSpace BlockTableRecord
            var ms = t.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
            if (ms == null) return;

            //var blockRefereneceList = (from ObjectId objectEntry in ms
            //                           where objectEntry.ObjectClass.Name.Equals("AcDbBlockReference")
            //                           let blockReference = t.GetObject(objectEntry, OpenMode.ForWrite) as BlockReference
            //                           select blockReference).ToList();
            //foreach (var blockRefereneceEntry in blockRefereneceList)
            //{
            //    var entry = blockRefereneceEntry.DynamicBlockTableRecord.GetObject(OpenMode.ForRead, false);
            //}

            // Выбор блоков панелей в чертеже.
            List<ObjectId> idBlRefsPanel = Select.GetBlockRefPanels(doc);
            if (idBlRefsPanel.Count == 0)
            {
               ed.WriteMessage("\nНе найдены блоки панелей. Блок панели с именем НС4_72-75");
               return;
            }

            // Обработка блоков. Получение колекции блоков, имен зон и типов цветов.
            List<Panel> panels = Panel.GetPanels(idBlRefsPanel);
            if (panels.Count == 0)
            {
               ed.WriteMessage("\nНе определены панели для раскраски.");
               return;
            }

            // Задание цветов для типов цветов
            FormColor formColor = new FormColor(TileColor.TileColors);
            var resDlg = formColor.ShowDialog();
            if (resDlg == System.Windows.Forms.DialogResult.OK)
            {
               // Покраска блоков
               foreach (var panel in panels)
               {
                  panel.Paint();
               }
            }
            t.Commit();
         }
      }      
   }
}
