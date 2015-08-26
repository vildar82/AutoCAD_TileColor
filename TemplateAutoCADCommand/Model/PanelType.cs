using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoCAD.Architect.TileColor
{
   public class PanelType
   {
      // Типы покраски панелей
      public List<PanelTypeColor> PanelTypeColors;
      public string Name { get; set; }
      private List<Zone> _zonesTemplate; // шаблон списка зон для типа панели. с дефолтным типом окраски и цветом. 
      private IGrouping<string, Panel> p;

      public List<Zone> ZonesTemplate
      {
         get { return _zonesTemplate; }
      }     

      public PanelType (Panel panel)
      {
         Name = panel.Name;
         // Получение шаблона зон (номеров).
         _zonesTemplate = Zone.GetZones(panel.IdBlRef, false);
         PanelTypeColors = new List<PanelTypeColor>();
      }

      public static List<PanelType> GetTypes(List<Panel> panels)
      {
         List<PanelType> panelTypes = new List<PanelType>();
         var panelsGroup = panels.GroupBy(p => p.Name).Select(g => g.First());         
         foreach (var panel in panelsGroup)
         {
            PanelType panelType = new PanelType(panel);
            panelTypes.Add(panelType);
         }
         return panelTypes;
      }
   }
}
