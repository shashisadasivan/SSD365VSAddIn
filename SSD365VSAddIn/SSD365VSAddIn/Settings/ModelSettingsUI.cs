using Microsoft.Dynamics.AX.Metadata.MetaModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSD365VSAddIn.Settings
{
    public partial class ModelSettingsUI : Form
    {
        ModelSettings modelSettings;
        // List<string> labelFileIds;
        IList<AxLabelFile> labelFiles;

        public ModelSettingsUI()
        {
            InitializeComponent();
            this.initModelSettings();
            this.labelModelName.Text = Common.CommonUtil.GetCurrentModel().Name;
        }

        /// <summary>
        /// Fetches the current model settings file and sets the data in the form
        /// </summary>
        public void initModelSettings()
        {
            // Get the model settings file
            modelSettings = FetchSettings.FindOrCreateSettings();

            // Get the list of labels in the current model
            //this.labelFiles = Labels.LabelHelper.GetLabelFiles();
            this.labelFiles = Labels.LabelHelper.GetAllLabelFilesCurrentModel();

            this.BindData();
        }

        private void BindData()
        {
            this.textPrefix.Text = modelSettings.Prefix;
            this.textSuffix.Text = modelSettings.Suffix;
            this.textExtensionName.Text = modelSettings.Extension;
            this.chkSecurityLabelCreate.Checked = modelSettings.SecurityLabelAutoCreate;

            // make sure that the label files selected are still valid
            var selectedLabels = this.modelSettings.LabelsToUpdate.Intersect(this.labelFiles.Select(l => l.Name))
                                .ToList();

            var availableLabels = this.labelFiles.Select(l => l.Name)
                                        .Except(selectedLabels).ToList();

            this.listBoxLangAvailable.Items.Clear();
            availableLabels.ForEach(l => this.listBoxLangAvailable.Items.Add(l));

            this.listBoxLangSelected.Items.Clear();
            selectedLabels.ForEach(l => this.listBoxLangSelected.Items.Add(l));

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(this.ValidateLabels() == false)
            {
                MessageBox.Show("Labels cannot contain mixed files, only labels of the same language can be added");
                return;
            }

            modelSettings.Prefix = this.textPrefix.Text;
            modelSettings.Suffix = this.textSuffix.Text;
            modelSettings.Extension = this.textExtensionName.Text;
            modelSettings.SecurityLabelAutoCreate = this.chkSecurityLabelCreate.Checked;

            //Add languages to update
            modelSettings.LabelsToUpdate.Clear();
            modelSettings.LabelsToUpdate.AddRange(this.listBoxLangSelected.Items.Cast<string>().ToList());
            
            // Save the model
            FetchSettings.SaveSettings(modelSettings);
            this.Close();
        }

        private Boolean ValidateLabels()
        {
            // You cant have 2 different label files in here. Only the same language for the label file
            Boolean valid = true;

            var selectedLabels = this.listBoxLangSelected.Items.Cast<string>().ToList();

            if(selectedLabels.Count > 0)
            {
                var firstLabel = selectedLabels.First();
                var labelFileId = Common.CommonUtil.GetMetaModelProviders().CurrentMetaModelService.GetLabelFile(firstLabel).LabelFileId;
                selectedLabels.ForEach(label =>
                {
                    var curlableFileId = Common.CommonUtil.GetMetaModelProviders().CurrentMetaModelService.GetLabelFile(label).LabelFileId;
                    if(curlableFileId.Equals(labelFileId) == false)
                    {
                        valid = false;
                    }
                });
            }

            return valid;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMoveToSelected_Click(object sender, EventArgs e)
        {
            MoveListBoxItems(this.listBoxLangAvailable, this.listBoxLangSelected);
        }

        private void btnMoveToAvailable_Click(object sender, EventArgs e)
        {
            MoveListBoxItems(this.listBoxLangSelected, this.listBoxLangAvailable);
        }

        private void MoveListBoxItems(ListBox source, ListBox destination)
        {
            ListBox.SelectedObjectCollection sourceItems = source.SelectedItems;
            foreach (var item in sourceItems)
            {
                destination.Items.Add(item);
            }
            while (source.SelectedItems.Count > 0)
            {
                source.Items.Remove(source.SelectedItems[0]);
            }
        }

    }
}
