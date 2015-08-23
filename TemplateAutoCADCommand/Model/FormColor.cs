using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AutoCAD.Architect.TileColor
{
   public partial class FormColor : Form
   {
      public FormColor(List<TileColor> tileColors)
      {
         InitializeComponent();

         listBoxColor.DataSource = tileColors;
         listBoxColor.DisplayMember = "TypeColor";         
      }

      private void buttonColor_Click(object sender, EventArgs e)
      {
         // Выбор цвета для выбранного типа цвета
         TileColor tileColor = listBoxColor.SelectedItem as TileColor;
         if (tileColor == null)
            return;

         Autodesk.AutoCAD.Windows.ColorDialog colorDlg = new Autodesk.AutoCAD.Windows.ColorDialog();
         var res = colorDlg.ShowDialog();
         if (res == DialogResult.OK)
         {
            tileColor.Color = colorDlg.Color;             
         }           
      }     

      private void listBoxColor_DrawItem(object sender, DrawItemEventArgs e)
      {
         e.DrawBackground();                  

         int index = e.Index;
         if (index >= 0 && index < listBoxColor.Items.Count)
         {
            TileColor tileColor = listBoxColor.Items[index] as TileColor;
            if (tileColor == null)
               return;
            Graphics g = e.Graphics;
            //background:   
            if (tileColor.Color != null)
            {
               SolidBrush brush = new SolidBrush(tileColor.Color.ColorValue);
               g.FillRectangle(brush, e.Bounds);
            }
            //text:            
            g.DrawString(tileColor.TypeColor, e.Font, Brushes.Black, listBoxColor.GetItemRectangle(index).Location);
         }
         e.DrawFocusRectangle();
      }
   }
}
