namespace SSD365VSAddIn.Labels.LabelForm
{
    partial class LabelCreatorForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.btnFindLabel = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblResult);
            this.groupBox1.Controls.Add(this.btnFindLabel);
            this.groupBox1.Controls.Add(this.txtLabel);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 282);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enter Label text";
            // 
            // txtLabel
            // 
            this.txtLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLabel.Location = new System.Drawing.Point(7, 20);
            this.txtLabel.Multiline = true;
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(416, 227);
            this.txtLabel.TabIndex = 0;
            // 
            // btnFindLabel
            // 
            this.btnFindLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFindLabel.Location = new System.Drawing.Point(7, 253);
            this.btnFindLabel.Name = "btnFindLabel";
            this.btnFindLabel.Size = new System.Drawing.Size(96, 23);
            this.btnFindLabel.TabIndex = 1;
            this.btnFindLabel.Text = "Find Label";
            this.btnFindLabel.UseVisualStyleBackColor = true;
            this.btnFindLabel.Click += new System.EventHandler(this.btnFindLabel_Click);
            // 
            // lblResult
            // 
            this.lblResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(109, 258);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(35, 13);
            this.lblResult.TabIndex = 2;
            this.lblResult.Text = "label1";
            // 
            // LabelCreatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 307);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LabelCreatorForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Create label";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFindLabel;
        private System.Windows.Forms.TextBox txtLabel;
        private System.Windows.Forms.Label lblResult;
    }
}