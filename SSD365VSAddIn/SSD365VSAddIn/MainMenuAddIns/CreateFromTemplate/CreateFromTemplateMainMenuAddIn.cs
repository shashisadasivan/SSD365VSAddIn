using Microsoft.Dynamics.Framework.Tools.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.MainMenuAddIns.CreateFromTemplate
{
    [Export(typeof(IMainMenu))] 
    class CreateFromTemplateMainMenuAddIn : MainMenuBase
    {

        #region Member variables
        private const string addinName = "SSD365VSAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                //return "Build and add reference";
                return AddinResources.CreateFromTemplate;

            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return CreateFromTemplateMainMenuAddIn.addinName;
            }
        }

        #endregion

        #region Callbacks
        /// <summary>
        /// Called when user clicks on the add-in menu
        /// </summary>
        /// <param name="e">The context of the VS tools and metadata</param>
        public override void OnClick(AddinEventArgs e)
        {
            if(this.IsProjectSelected())
            {
                var templateFolderPath = CreateFromTemplateMainMenuAddIn.GetTemplatePath();
                if (String.IsNullOrEmpty(templateFolderPath))
                {
                    return;
                }

                //var tempalteFolders = this.GetTemplateFolders(templateFolderPath);
                CreateFromTemplateForm createFromTemplateForm = new CreateFromTemplateForm();
                createFromTemplateForm.ShowDialog();
            }
        }
        #endregion

        // Check if project is selected
        private bool IsProjectSelected()
        {
            var curProject = Common.CommonUtil.GetCurrentProject();
            if (curProject == null)
            {
                System.Windows.Forms.MessageBox.Show("Select a project from the solution explorer", "No project selected", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        // Find where the DLL runs
        public static string GetTemplatePath()
        {
            string dllFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            dllFolderPath = Path.Combine(dllFolderPath, "Templates");
            if (Directory.Exists(dllFolderPath))
            {
                return dllFolderPath;
            }
            System.Windows.Forms.MessageBox.Show($"Templates folder does not exist: {dllFolderPath}", "Template folder not found", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.MessageBoxDefaultButton.Button1);
            return String.Empty;
        }

        // Each folder is a template - as it can contain multiple classes
        private List<string> GetTemplateFolders(string templatePath)
        {
            return Directory.GetDirectories(templatePath).ToList();
        }

        // Find the replacable text (collectively) and get the user to change it

        // Replace the text as per user input and create objects
    }
}
