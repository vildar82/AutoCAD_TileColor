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

         comboBoxPanelType.DataSource = _project.PanelTypes;
         comboBoxPanelType.DisplayMember = "Name";
         UpdateComboBoxColors();        

         HideTabPagesHeader();         
      }     

      // Смена типа панели. (ПП_4ОК_66-75)
      private void comboBoxPaneltype_SelectedIndexChanged(object sender, EventArgs e)
      {
         // Заполнение типов окраски этого типа панелей
         PanelType pt = (PanelType)comboBoxPanelType.SelectedItem;
         if (pt == null) return;
         if (pt.PanelTypeColors.Count ==0)
         {
            // дефолтный тип окраски
            PanelTypeColor ptcdefault = PanelTypeColor.Default(pt.ZonesTemplate, 1, _project.Colors[0]);
            pt.PanelTypeColors.Add(ptcdefault);
         }
         UpdateComboBoxPTC();

         // Переключение вкладки панелей для раскраски этого типа панелей.
         int tabIndex = 0;
         switch (pt.Name)
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
         // раскраска панелей на вкладке
         PaintPanels();         
      }      

      // Покраска зоны на форме.
      private void panel_Click(object sender, EventArgs e)
      {
         System.Windows.Forms.Panel panelCtrl = (System.Windows.Forms.Panel)sender;
         string tag = panelCtrl.Tag.ToString();// Tag - номер зоны (11).

         TileColor tc = (TileColor)comboBoxColor.SelectedItem;
         if (tc == null) return;

         PanelTypeColor ptc = (PanelTypeColor)comboBoxPanelTypeColor.SelectedItem;
         if (ptc == null) return;
         Zone zone = ptc.Zones.Find(z => z.Name == tag);
         zone.ZoneColor = tc;

         panelCtrl.BackColor = zone.ZoneColor.Color.ColorValue;
      }      

      // Покраска выбранных блоков панелей в чертеже
      private void buttonPaint_Click(object sender, EventArgs e)
      {
         // покраска выбранных блоков панелей по типу панели и типу окраски.
         PanelType pt = (PanelType)comboBoxPanelType.SelectedItem;
         if (pt == null) return;
         PanelTypeColor ptc = (PanelTypeColor)comboBoxPanelTypeColor.SelectedItem;
         if (ptc == null) return;
         Hide();        
         _project.PaintSelected(pt.Name,ptc);
         Show();        
      }

      // Добавление цвета в проект
      private void buttonAddColor_Click(object sender, EventArgs e)
      {
         Autodesk.AutoCAD.Windows.ColorDialog colorDlg = new Autodesk.AutoCAD.Windows.ColorDialog();
         colorDlg.IncludeByBlockByLayer = false;         
         var dlgRes = colorDlg.ShowDialog();
         if (dlgRes == DialogResult.OK)
         {
            TileColor tileColor = new TileColor(_project.Colors.Count, colorDlg.Color);
            _project.Colors.Add(tileColor);
            UpdateComboBoxColors();
            // Установка добавленного цвета текущим
            comboBoxColor.SelectedItem = tileColor; 
         }
      }

      // Изменение цвета
      private void buttonChangeColor_Click(object sender, EventArgs e)
      {
         Autodesk.AutoCAD.Windows.ColorDialog colorDlg = new Autodesk.AutoCAD.Windows.ColorDialog();
         colorDlg.IncludeByBlockByLayer = false;
         var dlgRes = colorDlg.ShowDialog();
         if (dlgRes == DialogResult.OK)
         {
            TileColor tc = (TileColor)comboBoxColor.SelectedItem;
            if (tc == null) return;
            tc.Color = colorDlg.Color;
            // Перекраска панелей
            PaintPanels();
         }
      }

      private void PaintPanels()
      {
         PanelTypeColor ptc = (PanelTypeColor)comboBoxPanelTypeColor.SelectedItem;
         if (ptc == null) return;

         TabPage tabPage = tabControl1.SelectedTab;
         foreach (Control item in tabPage.Controls)
         {
            if (item is TableLayoutPanel)
            {
               TableLayoutPanel tableLayout = (TableLayoutPanel)item;
               foreach (Control itemPanel in tableLayout.Controls)
               {
                  if (itemPanel is System.Windows.Forms.Panel)
                  {
                     System.Windows.Forms.Panel panelControl = (System.Windows.Forms.Panel)itemPanel;
                     if (panelControl.Tag != null)
                     {
                        string tag = panelControl.Tag.ToString();
                        Zone zone = ptc.GetZone(tag);
                        panelControl.BackColor = zone.ZoneColor.Color.ColorValue;
                     }
                  }
               }
            }
         }
      }

      // Добавление типа окраски
      private void buttonAddPTC_Click(object sender, EventArgs e)
      {
         PanelType pt = (PanelType)comboBoxPanelType.SelectedItem;
         if (pt == null) return;
         PanelTypeColor ptc = PanelTypeColor.Default(pt.ZonesTemplate, pt.PanelTypeColors.Count+1, _project.Colors[0]);
         pt.PanelTypeColors.Add(ptc);
         UpdateComboBoxPTC();
         // Установка добавленного типа окраски
         comboBoxPanelTypeColor.SelectedItem = ptc; 
      }

      private void comboBoxColor_DrawItem(object sender, DrawItemEventArgs e)
      {         
         e.DrawBackground();
         var g = e.Graphics;
         TileColor tileColor = (TileColor)((ComboBox)sender).Items[e.Index];         
         Brush brush = new SolidBrush(tileColor.Color.ColorValue);
         g.FillRectangle(brush,e.Bounds); 
         // Draw the text    
         g.DrawString(tileColor.Name.ToString(), ((Control)sender).Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
      }

      private void HideTabPagesHeader()
      {
         tabControl1.Appearance = TabAppearance.FlatButtons; tabControl1.ItemSize = new Size(0, 1); tabControl1.SizeMode = TabSizeMode.Fixed;
      }

      private void UpdateComboBoxColors()
      {
         comboBoxColor.DataSource = null;
         comboBoxColor.DataSource = _project.Colors;
      }
      private void UpdateComboBoxPTC()
      {
         PanelType pt = (PanelType)comboBoxPanelType.SelectedItem;
         if (pt == null) return;
         comboBoxPanelTypeColor.DataSource = null; 
         comboBoxPanelTypeColor.DataSource = pt.PanelTypeColors;
         comboBoxPanelTypeColor.DisplayMember = "Number";
      }
   }
}
