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
      Project _project;

      public FormPanelColorEditor(Project project)
      {
         InitializeComponent();

         _project = project;

         comboBoxPaneltype.DataSource = _project.PanelTypes;
         comboBoxPaneltype.DisplayMember = "Name";

         comboBoxColor.DataSource = _project.Colors;
         comboBoxColor.DisplayMember = "Name";
      }        

      // Смена типа панели. (ПП_4ОК_66-75)
      private void comboBoxPaneltype_SelectedIndexChanged(object sender, EventArgs e)
      {
         // Заполнение типов окраски этого типа панелей
         PanelType panelType = (PanelType)comboBoxPaneltype.SelectedItem;
         if (panelType.PanelTypeColors.Count ==0)
         {
            // дефолтный тип окраски
            PanelTypeColor ptcdefault = PanelTypeColor.Default(panelType, 1);
            panelType.PanelTypeColors.Add(ptcdefault);
         }
         comboBoxPanelTypeColor.DataSource = panelType.PanelTypeColors;
         comboBoxPanelTypeColor.DisplayMember = "Number";

         // Переключение вкладки панелей для раскраски этого типа панелей.
         int tabIndex = 0;
         switch (panelType.Name)
         {
            case "ПП_4ОК_66-75":
               tabIndex = 0;
               break;            
         }
         tabControl1.SelectedIndex = tabIndex; 
      }

      // Смена типа окраски панели
      private void comboBoxPanelTypeColor_SelectedIndexChanged(object sender, EventArgs e)
      {
         PanelTypeColor ptc = (PanelTypeColor)comboBoxPanelTypeColor.SelectedItem;
         // раскраска панелей на вкладке
         PanelControlsPaint(ptc);
      }      

      // Покраска зоны на форме.
      private void panel_Click(object sender, EventArgs e)
      {
         System.Windows.Forms.Panel panelCtrl = (System.Windows.Forms.Panel)sender;
         string tag = panelCtrl.Tag.ToString();// Tag - номер зоны (11).

         TileColor tylecolor = (TileColor)comboBoxColor.SelectedItem;

         PanelTypeColor ptc = (PanelTypeColor)comboBoxPanelTypeColor.SelectedItem;
         Zone zone = ptc.Zones.Find(z => z.Name == tag);
         zone.ZoneColor = tylecolor;

         panelCtrl.BackColor = zone.ZoneColor.Color.ColorValue;
      }

      private void PanelControlsPaint(PanelTypeColor ptc)
      {
         TabPage tabPage = tabControl1.SelectedTab;

         foreach (Control item in tabPage.Controls)
         {
            var panelControl = item as System.Windows.Forms.Panel;
            if (panelControl == null)
               continue;
            if (panelControl.Tag != null)
            {
               string tag = panelControl.Tag.ToString();
               panelControl.BackColor = ptc.GetColorForZone(tag);
            }            
         }
      }

      // Покраска выбранных блоков панелей в чертеже
      private void buttonPaint_Click(object sender, EventArgs e)
      {
         // покраска выбранных блоков панелей по типу панели и типу окраски.
         PanelType paneltype = (PanelType)comboBoxPaneltype.SelectedItem;
         this.Hide();        
         _project.PaintSelected(paneltype);
         this.Show();        
      }
   }
}
