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
            this.labelFiles = Labels.LabelHelper.GetLabelFiles();

            this.BindData();
        }

        private void BindData()
        {
            this.textPrefix.Text = modelSettings.Prefix;
            this.textSuffix.Text = modelSettings.Suffix;

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
            modelSettings.Prefix = this.textPrefix.Text;
            modelSettings.Suffix = this.textSuffix.Text;

            //Add languages to update
            modelSettings.LabelsToUpdate.Clear();
            modelSettings.LabelsToUpdate.AddRange(this.listBoxLangSelected.Items.Cast<string>().ToList());
            
            // Save the model
            FetchSettings.SaveSettings(modelSettings);
            this.Close();
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
