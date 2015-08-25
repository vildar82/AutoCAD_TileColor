using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCAD.Architect.TileColor
{
   public partial class FormPanelColorEditor : Form
   {
      Project project;

      public FormPanelColorEditor(Project project)
      {
         InitializeComponent();

         this.project = project;

         // Заполнение типов панелей
         FillComboBoxPanelType();

         // Заполнение цветов
         FillColors();
      }
      
      // Заполнение типов панелей
      private void FillComboBoxPanelType()
      {
         comboBoxPaneltype.Items.Clear(); 
         comboBoxPaneltype.Items.AddRange(project.panelsType.Keys.ToArray());
      }    
      
      // Заполнение цветов
      private void FillColors()
      {
         comboBoxColor.Items.Clear();
         foreach (var item in project.colors.Values)
         {
            comboBoxColor.Items.Add(item);
         }         
      }

      // Заполнение типов окраски      
      private void FillPanelTypeColor()
      {
         comboBoxPanelTypeColor.Items.Clear();

         if (comboBoxPaneltype.SelectedIndex == -1)
            return;
         PanelType paneltype = (PanelType)comboBoxPaneltype.SelectedItem;         
         foreach (PanelTypeColor item in paneltype.Panels.Values)
         {
            comboBoxPanelTypeColor.Items.Add(item); 
         }            
      }

      // Переключение типа панели
      private void comboBoxPaneltype_SelectedIndexChanged(object sender, EventArgs e)
      {
         // Переключение таба на выбранную панель
         string panelTypeName = (string)comboBoxPaneltype.SelectedItem;
         switch (panelTypeName)
         {
            case "ПП_4ОК_66-75":
               tabControl1.SelectedIndex = 0;
               break;
         }
         // Заполнение типов окраски.
         FillPanelTypeColor();
      }

      // Переключение типа окраски
      private void comboBoxPanelTypeColor_SelectedIndexChanged(object sender, EventArgs e)
      {
         PanelTypeColor ptc = (PanelTypeColor)comboBoxPanelTypeColor.SelectedItem;
         ResetPanelColor();
         foreach (Zone zone in ptc.Zones)
         {
            foreach (Control control in tabControl1.SelectedTab.Controls)
            {
               if (control.Tag.ToString() == zone.Name)
               {
                  control.BackColor = zone.ZoneColor.Color.ColorValue;
                  continue;
               }
            } 
         }
      }

      private void ResetPanelColor()
      {
         foreach (Control control in tabControl1.SelectedTab.Controls)
         {
            if (control.Tag != null )
            {
               control.BackColor = Color.Transparent;               
            }
         }
      }

      private void panel_Click(object sender, EventArgs e)
      {
         System.Windows.Forms.Panel panel = (System.Windows.Forms.Panel)sender;
         string tag = panel.Tag.ToString();

         TileColor tylecolor = (TileColor)comboBoxColor.SelectedItem; 

         PanelTypeColor ptc = (PanelTypeColor)comboBoxPanelTypeColor.SelectedItem;
         Zone zone = ptc.Zones.Find(z => z.Name == tag);
         zone.ZoneColor = tylecolor;         
      }
   }
}
