// Покраска блоков панелей на чертеже.
// Считаем, что в чертеже(документе) панели только одного проекта, и для этого чертежа определен один набор цветов для всех панелей (_tileColors)

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

[assembly: CommandClass(typeof(AutoCAD.Architect.TileColor.Commands))]

namespace AutoCAD.Architect.TileColor
{
   public class Commands
   {
      // Для кажого документа свой объект Commands, с уникальным для этого чертежа набором цветов и т.д.
      // Статические данные будут одинаковыми для всех чертежей в текущем сеансе автокада.      

      Project project;

      // Редактор для покраски панелей
      [CommandMethod("PanelColorEditor", "PIK", CommandFlags.Modal | CommandFlags.NoBlockEditor)]
      public void PanelColorEditorCommand()
      {
         if (project == null)
         {
            project = new Project();
            project.CurrentPanels(); 
         }

         // Форма для покраски панелей в форме по зонам (по типу панели, по типу покраски). Выбор блоков в модели для покраски.
         FormPanelColorEditor formeditor = new FormPanelColorEditor(project);
      }
   }
}
