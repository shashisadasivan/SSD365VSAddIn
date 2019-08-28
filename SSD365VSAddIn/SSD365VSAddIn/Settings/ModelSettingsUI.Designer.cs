namespace SSD365VSAddIn.Settings
{
    partial class ModelSettingsUI
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
            this.labelModelName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textPrefix = new System.Windows.Forms.TextBox();
            this.textSuffix = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMoveToAvailable = new System.Windows.Forms.Button();
            this.btnMoveToSelected = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.listBoxLangSelected = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.listBoxLangAvailable = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelModelName
            // 
            this.labelModelName.AutoSize = true;
            this.labelModelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModelName.Location = new System.Drawing.Point(145, 10);
            this.labelModelName.Name = "labelModelName";
            this.labelModelName.Size = new System.Drawing.Size(98, 20);
            this.labelModelName.TabIndex = 0;
            this.labelModelName.Text = "Model Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Settings for model:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Prefix for element name";
            // 
            // textPrefix
            // 
            this.textPrefix.Location = new System.Drawing.Point(160, 45);
            this.textPrefix.Name = "textPrefix";
            this.textPrefix.Size = new System.Drawing.Size(107, 20);
            this.textPrefix.TabIndex = 7;
            // 
            // textSuffix
            // 
            this.textSuffix.Location = new System.Drawing.Point(160, 74);
            this.textSuffix.Name = "textSuffix";
            this.textSuffix.Size = new System.Drawing.Size(107, 20);
            this.textSuffix.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Suffix for element name";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnMoveToAvailable);
            this.panel1.Controls.Add(this.btnMoveToSelected);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.listBoxLangSelected);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.listBoxLangAvailable);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(2, 116);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(465, 227);
            this.panel1.TabIndex = 10;
            // 
            // btnMoveToAvailable
            // 
            this.btnMoveToAvailable.Location = new System.Drawing.Point(190, 110);
            this.btnMoveToAvailable.Name = "btnMoveToAvailable";
            this.btnMoveToAvailable.Size = new System.Drawing.Size(75, 23);
            this.btnMoveToAvailable.TabIndex = 6;
            this.btnMoveToAvailable.Text = "<<";
            this.btnMoveToAvailable.UseVisualStyleBackColor = true;
            this.btnMoveToAvailable.Click += new System.EventHandler(this.btnMoveToAvailable_Click);
            // 
            // btnMoveToSelected
            // 
            this.btnMoveToSelected.Location = new System.Drawing.Point(190, 81);
            this.btnMoveToSelected.Name = "btnMoveToSelected";
            this.btnMoveToSelected.Size = new System.Drawing.Size(75, 23);
            this.btnMoveToSelected.TabIndex = 5;
            this.btnMoveToSelected.Text = ">>";
            this.btnMoveToSelected.UseVisualStyleBackColor = true;
            this.btnMoveToSelected.Click += new System.EventHandler(this.btnMoveToSelected_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(280, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Selected";
            // 
            // listBoxLangSelected
            // 
            this.listBoxLangSelected.FormattingEnabled = true;
            this.listBoxLangSelected.Location = new System.Drawing.Point(269, 64);
            this.listBoxLangSelected.Name = "listBoxLangSelected";
            this.listBoxLangSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxLangSelected.Size = new System.Drawing.Size(168, 147);
            this.listBoxLangSelected.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Available";
            // 
            // listBoxLangAvailable
            // 
            this.listBoxLangAvailable.FormattingEnabled = true;
            this.listBoxLangAvailable.Location = new System.Drawing.Point(17, 64);
            this.listBoxLangAvailable.Name = "listBoxLangAvailable";
            this.listBoxLangAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxLangAvailable.Size = new System.Drawing.Size(168, 147);
            this.listBoxLangAvailable.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Labels to update";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Location = new System.Drawing.Point(2, 362);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(465, 42);
            this.panel2.TabIndex = 11;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(110, 16);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(14, 16);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ModelSettingsUI
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(468, 407);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textSuffix);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textPrefix);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelModelName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelSettingsUI";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Model settings";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelModelName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textPrefix;
        private System.Windows.Forms.TextBox textSuffix;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnMoveToAvailable;
        private System.Windows.Forms.Button btnMoveToSelected;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox listBoxLangSelected;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox listBoxLangAvailable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}