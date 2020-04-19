using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSD365VSAddIn.MainMenuAddIns.CreateFromTemplate
{
    public partial class CreateFromTemplateForm : Form
    {
        Dictionary<string, string> templateFolderMap;
        List<TemplateRepleacableText> replaceableTextValues;
        string templatePathSelected;

        public CreateFromTemplateForm()
        {
            InitializeComponent();
            this.InitData();
        }

        private void InitData()
        {
            this.templateNameLabel.Text = String.Empty;
            replaceableTextValues = new List<TemplateRepleacableText>();

            var templateFolders = this.GetTemplateFolders();
            templateFolderMap = new Dictionary<string, string>();
            foreach (var templateFolder in templateFolders)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(templateFolder);
                var dirName = dirInfo.Name; // Path.GetDirectoryName(templateFolder);
                templateFolderMap.Add(dirName, templateFolder);
                this.templatesListBox.Items.Add(dirName);
            }
        }

        private List<string> GetTemplateFolders()
        {
            var templatePath = CreateFromTemplateMainMenuAddIn.GetTemplatePath();
            return Directory.GetDirectories(templatePath).ToList();
        }

        private void templatesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dirName = templatesListBox.SelectedItem.ToString();
            templatePathSelected = templateFolderMap.Where(d => d.Key.Equals(dirName)).FirstOrDefault().Value;

            // Get the Description file
            var descriptionFilePath = Path.Combine(templatePathSelected, "Description.txt");
            if(File.Exists(descriptionFilePath))
            {
                templateDescriptionTxt.Text = File.ReadAllText(descriptionFilePath);
            }
            else
            {
                templateDescriptionTxt.Text = string.Empty;
            }

            templateNameLabel.Text = dirName;

            replaceableTextValues.Clear();
            var replaceableTextPath = Path.Combine(templatePathSelected, "ReplaceableText.txt");
            if (File.Exists(replaceableTextPath))
            {
                var replaceableText = File.ReadAllLines(replaceableTextPath).ToList();
                replaceableText.ForEach(r => replaceableTextValues.Add(new TemplateRepleacableText()
                {
                    ReplaceableText = r,
                }));
            }

            this.templateRepleacableTextBindingSource.DataSource = replaceableTextValues;
            //templateReplaceableTextGridView.Refresh();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Put a try catch around this
            try
            {
                this.ProcessTemplate();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void ProcessTemplate()
        {
            // For each file inside the directory replace the text and create the objects
            //Complete this, for each XML found, create the Element in the 
            var classDirPath = Path.Combine(templatePathSelected, "AxClass");
            if (Directory.Exists(classDirPath))
            {
                //var designMetaModelService = Microsoft.Dynamics.Framework.Tools.MetaModel.Core.DesignMetaModelService.Instance;

                var classFiles = Directory.GetFiles(classDirPath, "*.xml");
                foreach (var classFile in classFiles)
                {
                    var loadedClass = this.ProcessFile(classFile) as Microsoft.Dynamics.AX.Metadata.MetaModel.AxClass;
                    // 4. Save the file appropriately
                    Common.CommonUtil.GetModelSaveService().CreateClass(loadedClass, Common.CommonUtil.GetCurrentModelSaveInfo());
                    Common.CommonUtil.AddElementToProject(loadedClass);
                }
            }
            // Menu item Action
            var menuItemActionDirPath = Path.Combine(templatePathSelected, "AxMenuItemAction");
            if (Directory.Exists(menuItemActionDirPath))
            {
                var menuItemActionFiles = Directory.GetFiles(menuItemActionDirPath, "*.xml");
                foreach (var menuItemActionFile in menuItemActionFiles)
                {
                    var loadedMenuItemAction = this.ProcessFile(menuItemActionFile) as Microsoft.Dynamics.AX.Metadata.MetaModel.AxMenuItemAction;
                    Common.CommonUtil.GetModelSaveService().CreateMenuItemAction(loadedMenuItemAction, Common.CommonUtil.GetCurrentModelSaveInfo());
                    Common.CommonUtil.AddElementToProject(loadedMenuItemAction);
                }
            }
        }

        private Microsoft.Dynamics.AX.Metadata.Core.MetaModel.ISingleKeyedMetadata<string> ProcessFile(string fileName)
        {
            var designMetaModelService = Microsoft.Dynamics.Framework.Tools.MetaModel.Core.DesignMetaModelService.Instance;


            // 1. Copy the file to a temp file
            var tempFile = this.getTempFile(fileName);
            var fileData = File.ReadAllText(fileName);
            // 2. Apply any renames to it
            foreach (var replaceableTextValue in replaceableTextValues)
            {
                // A. Replace the case sensitive search first
                if (String.IsNullOrWhiteSpace(replaceableTextValue.ReplacedText) == false)
                {
                    fileData = fileData.Replace(replaceableTextValue.ReplaceableText, replaceableTextValue.ReplacedText);
                }
                // B. Replace case insesitive search
                if(String.IsNullOrWhiteSpace(replaceableTextValue.ReplacedOtherCaseText) == false)
                {
                    var regex = new System.Text.RegularExpressions.Regex(replaceableTextValue.ReplaceableText, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    fileData = regex.Replace(fileData, replaceableTextValue.ReplacedOtherCaseText);
                }
            }
            File.WriteAllText(tempFile, fileData);
            // 3. Load the temp file as a metadata
            var loadedClass = designMetaModelService.LoadMetadataRootElementFromExternalFile(tempFile);

            File.Delete(tempFile);

            return loadedClass;
        }

        public string getTempFile(string fileToCopy)
        {
            string xmlFile;


            var fileInfo = new FileInfo(fileToCopy);

            xmlFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "_" + fileInfo.Name);
            //File.Copy(fileToCopy, xmlFile);

            return xmlFile;
        }
    }
}
