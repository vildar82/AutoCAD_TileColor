namespace AutoCAD.Architect.TileColor
{
   partial class FormColor
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.listBoxColor = new System.Windows.Forms.ListBox();
         this.buttonColor = new System.Windows.Forms.Button();
         this.buttonCancel = new System.Windows.Forms.Button();
         this.buttonOk = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // listBoxColor
         // 
         this.listBoxColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
         this.listBoxColor.FormattingEnabled = true;
         this.listBoxColor.Location = new System.Drawing.Point(12, 22);
         this.listBoxColor.Name = "listBoxColor";
         this.listBoxColor.Size = new System.Drawing.Size(212, 186);
         this.listBoxColor.TabIndex = 0;
         this.listBoxColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxColor_DrawItem);
         // 
         // buttonColor
         // 
         this.buttonColor.Location = new System.Drawing.Point(240, 22);
         this.buttonColor.Name = "buttonColor";
         this.buttonColor.Size = new System.Drawing.Size(75, 23);
         this.buttonColor.TabIndex = 1;
         this.buttonColor.Text = "Цвет";
         this.buttonColor.UseVisualStyleBackColor = true;
         this.buttonColor.Click += new System.EventHandler(this.buttonColor_Click);
         // 
         // buttonCancel
         // 
         this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.buttonCancel.Location = new System.Drawing.Point(326, 262);
         this.buttonCancel.Name = "buttonCancel";
         this.buttonCancel.Size = new System.Drawing.Size(75, 23);
         this.buttonCancel.TabIndex = 2;
         this.buttonCancel.Text = "Отмена";
         this.buttonCancel.UseVisualStyleBackColor = true;
         // 
         // buttonOk
         // 
         this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonOk.Location = new System.Drawing.Point(245, 262);
         this.buttonOk.Name = "buttonOk";
         this.buttonOk.Size = new System.Drawing.Size(75, 23);
         this.buttonOk.TabIndex = 3;
         this.buttonOk.Text = "Ок";
         this.buttonOk.UseVisualStyleBackColor = true;
         // 
         // FormColor
         // 
         this.AcceptButton = this.buttonOk;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.buttonCancel;
         this.ClientSize = new System.Drawing.Size(412, 295);
         this.Controls.Add(this.buttonOk);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.buttonColor);
         this.Controls.Add(this.listBoxColor);
         this.Name = "FormColor";
         this.Text = "Задание цветов плитки";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ListBox listBoxColor;
      private System.Windows.Forms.Button buttonColor;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.Button buttonOk;
   }
}