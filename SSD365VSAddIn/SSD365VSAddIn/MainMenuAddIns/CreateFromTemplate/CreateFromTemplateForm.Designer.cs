namespace SSD365VSAddIn.MainMenuAddIns.CreateFromTemplate
{
    partial class CreateFromTemplateForm
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
            this.components = new System.ComponentModel.Container();
            this.templatesListBox = new System.Windows.Forms.ListBox();
            this.templatesLabel = new System.Windows.Forms.Label();
            this.templateDescriptionTxt = new System.Windows.Forms.TextBox();
            this.templateNameLabel = new System.Windows.Forms.Label();
            this.templateReplaceableTextGridView = new System.Windows.Forms.DataGridView();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.templateRepleacableTextBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.replaceableTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.replacedTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.templateReplaceableTextGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.templateRepleacableTextBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // templatesListBox
            // 
            this.templatesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.templatesListBox.FormattingEnabled = true;
            this.templatesListBox.HorizontalScrollbar = true;
            this.templatesListBox.Location = new System.Drawing.Point(0, 39);
            this.templatesListBox.Name = "templatesListBox";
            this.templatesListBox.Size = new System.Drawing.Size(168, 368);
            this.templatesListBox.TabIndex = 0;
            this.templatesListBox.SelectedIndexChanged += new System.EventHandler(this.templatesListBox_SelectedIndexChanged);
            // 
            // templatesLabel
            // 
            this.templatesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.templatesLabel.AutoSize = true;
            this.templatesLabel.Location = new System.Drawing.Point(-3, 9);
            this.templatesLabel.Name = "templatesLabel";
            this.templatesLabel.Size = new System.Drawing.Size(56, 13);
            this.templatesLabel.TabIndex = 1;
            this.templatesLabel.Text = "Templates";
            // 
            // templateDescriptionTxt
            // 
            this.templateDescriptionTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.templateDescriptionTxt.Location = new System.Drawing.Point(185, 39);
            this.templateDescriptionTxt.Multiline = true;
            this.templateDescriptionTxt.Name = "templateDescriptionTxt";
            this.templateDescriptionTxt.ReadOnly = true;
            this.templateDescriptionTxt.Size = new System.Drawing.Size(500, 72);
            this.templateDescriptionTxt.TabIndex = 2;
            // 
            // templateNameLabel
            // 
            this.templateNameLabel.AutoSize = true;
            this.templateNameLabel.Location = new System.Drawing.Point(185, 9);
            this.templateNameLabel.Name = "templateNameLabel";
            this.templateNameLabel.Size = new System.Drawing.Size(80, 13);
            this.templateNameLabel.TabIndex = 3;
            this.templateNameLabel.Text = "Template name";
            // 
            // templateReplaceableTextGridView
            // 
            this.templateReplaceableTextGridView.AllowUserToAddRows = false;
            this.templateReplaceableTextGridView.AllowUserToDeleteRows = false;
            this.templateReplaceableTextGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.templateReplaceableTextGridView.AutoGenerateColumns = false;
            this.templateReplaceableTextGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.templateReplaceableTextGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.replaceableTextDataGridViewTextBoxColumn,
            this.replacedTextDataGridViewTextBoxColumn});
            this.templateReplaceableTextGridView.DataSource = this.templateRepleacableTextBindingSource;
            this.templateReplaceableTextGridView.Location = new System.Drawing.Point(185, 118);
            this.templateReplaceableTextGridView.Name = "templateReplaceableTextGridView";
            this.templateReplaceableTextGridView.Size = new System.Drawing.Size(500, 262);
            this.templateReplaceableTextGridView.TabIndex = 4;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Location = new System.Drawing.Point(185, 386);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(286, 386);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // templateRepleacableTextBindingSource
            // 
            this.templateRepleacableTextBindingSource.DataSource = typeof(SSD365VSAddIn.MainMenuAddIns.CreateFromTemplate.TemplateRepleacableText);
            // 
            // replaceableTextDataGridViewTextBoxColumn
            // 
            this.replaceableTextDataGridViewTextBoxColumn.DataPropertyName = "ReplaceableText";
            this.replaceableTextDataGridViewTextBoxColumn.HeaderText = "ReplaceableText";
            this.replaceableTextDataGridViewTextBoxColumn.Name = "replaceableTextDataGridViewTextBoxColumn";
            this.replaceableTextDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // replacedTextDataGridViewTextBoxColumn
            // 
            this.replacedTextDataGridViewTextBoxColumn.DataPropertyName = "ReplacedText";
            this.replacedTextDataGridViewTextBoxColumn.HeaderText = "ReplacedText";
            this.replacedTextDataGridViewTextBoxColumn.Name = "replacedTextDataGridViewTextBoxColumn";
            // 
            // CreateFromTemplateForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(697, 421);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.templateReplaceableTextGridView);
            this.Controls.Add(this.templateNameLabel);
            this.Controls.Add(this.templateDescriptionTxt);
            this.Controls.Add(this.templatesLabel);
            this.Controls.Add(this.templatesListBox);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(713, 460);
            this.Name = "CreateFromTemplateForm";
            this.ShowIcon = false;
            this.Text = "Create from template";
            ((System.ComponentModel.ISupportInitialize)(this.templateReplaceableTextGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.templateRepleacableTextBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox templatesListBox;
        private System.Windows.Forms.Label templatesLabel;
        private System.Windows.Forms.TextBox templateDescriptionTxt;
        private System.Windows.Forms.Label templateNameLabel;
        private System.Windows.Forms.DataGridView templateReplaceableTextGridView;
        private System.Windows.Forms.BindingSource templateRepleacableTextBindingSource;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn replaceableTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn replacedTextDataGridViewTextBoxColumn;
    }
}