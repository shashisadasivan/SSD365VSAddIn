using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.MainMenuAddIns.Build
{
    [Export(typeof(IMainMenu))]
    class BuildAddReferenceMainMenuAddIn : MainMenuBase
    {
        public List<string> ReferencesAdded { get; set; }
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
                return AddinResources.AddMissingReferences;

            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return BuildAddReferenceMainMenuAddIn.addinName;
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
            this.ReferencesAdded = new List<string>();
            try
            {
                bool referenceAdded;
                do
                {
                    // Check fior errors and then add the module reference
                    referenceAdded = this.CheckErrorsAndAddReference();
                    // now compile the project and this will give the next set of missing reference
                    this.BuildSolution();

                } while (referenceAdded); // Keep doing this until errors found are similar
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
            if (ReferencesAdded.Count > 0)
            {
                this.ReferencesAdded.Add("Rebuild the project to check further errors");
                // show the list of references added
                System.Windows.Forms.MessageBox.Show(
                    string.Join(Environment.NewLine, this.ReferencesAdded),
                    "Module references added");
            }
        }
        #endregion

        public const string vsWindowKindErrorList = "{D78612C7-9962-4B83-95D9-268046DAD23A}";

        /// <summary>
        /// Get the errors in the Error list window
        /// Picks up errors with "The name 'ABCPercentA' does not denote a class, a table, or an extended data type"
        /// as this denotes that there is a missing module reference
        /// </summary>
        /// <returns>True is a reference was added</returns>
        public bool CheckErrorsAndAddReference()
        {
            // Most of the code comes from this stackoverflow conversation
            // https://stackoverflow.com/questions/36834038/visual-studio-2015-envdte-read-errorlist

            bool moduleReferenceAdded = false;

            // get the list of errors from the Error list wondow
            EnvDTE.Window window = Common.CommonUtil.DTE.Windows.Item(vsWindowKindErrorList);
            EnvDTE80.ErrorList sel = (EnvDTE80.ErrorList)window.Selection;
            var errorItemsDte = sel.ErrorItems;
            for (int i = 1; i <= errorItemsDte.Count; i++)
            {
                var errorItem = errorItemsDte.Item(i);
                if(errorItem.ErrorLevel == EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelHigh
                    && errorItem.Description.Contains("does not denote a class, a table, or an extended data type"))
                    // The name 'ABCPercentA' does not denote a class, a table, or an extended data type
                {
                    //System.Windows.Forms.MessageBox.Show($"Error found: {errorItem.ErrorLevel}; {errorItem.Description}");
                    
                    //now find the element - check if it is an EDT, Table, Class
                    string elementName = this.GetElementNameFromError(errorItem.Description);
                    var moduleNameToReference = this.GetModuleFromElement(elementName);
                    if(String.IsNullOrEmpty(moduleNameToReference))
                    {
                        throw new Exception($"No matching element found for {elementName} in EDT's, tables, classes");
                    }
                    // Then find the model
                    
                    // then add the model to the current models reference
                    var currentModel = Common.CommonUtil.GetCurrentModel();
                    // Update the module references
                    if (currentModel.Readonly || currentModel.ModuleReferences.IsReadOnly)
                    {
                        //you cant add to the list as it is read only.
                        // So copy the list into a new one, add the module & updated the model with this list as moduleReference
                        List<string> modules = new List<string>(currentModel.ModuleReferences);
                        if (modules.Contains(moduleNameToReference) == false)
                        {
                            modules.Add(moduleNameToReference);
                            currentModel.ModuleReferences = modules;

                            Common.CommonUtil.GetModelSaveService().UpdateModel(currentModel);
                            this.ReferencesAdded.Add(moduleNameToReference);
                            moduleReferenceAdded = true;
                        }
                        else
                        {
                            // Something else is going on,so break;
                            // User has to manually refresh models and build
                            break;
                        }
                    }
                    //else
                    //{
                    //    // Dont think this will ever go here
                    //    currentModel.ModuleReferences.Add(moduleNameToReference); // this cannot be updated as it is readonly
                    //}
                    
                    // break; // there is usually only one error list this at a time
                }
            }

            return moduleReferenceAdded;
        }

        /// <summary>
        /// Get the element name from the error message
        /// Error message is like: The name 'ABCPercentA' does not denote a class, a table, or an extended data type
        /// </summary>
        /// <param name="errorMsg">Error message with element name in it</param>
        /// <returns>Element name</returns>
        private string GetElementNameFromError(string errorMsg)
        {
            // Error is always like this:
            // The name 'ABCPercentA' does not denote a class, a table, or an extended data type

            string elementName = String.Empty;

            int quote1, quote2;
            quote1 = errorMsg.IndexOf("'", 0);
            quote2 = errorMsg.IndexOf("'", quote1 + 1);
            if(quote1 > 0 && quote2 > quote1)
            {
                elementName = errorMsg.Substring(quote1 + 1, quote2 - quote1 - 1);
            }

            return elementName;
        }

        /// <summary>
        /// Gets the module name for the element
        /// Search the EDTs, Tables, Classes
        /// Will need to add other elements as well here maybe
        /// </summary>
        /// <param name="elementName">Element name whose modulel is to be found</param>
        /// <returns>Module name of the givent element name</returns>
        private string GetModuleFromElement(string elementName)
        {
            
            // Check if this is an EDT
            var edt = Common.CommonUtil.GetModelSaveService()
                            .GetExtendedDataType(elementName);
            if(edt != null)
            {
                var modelInfo = Common.CommonUtil.GetModelSaveService()
                    .GetExtendedDataTypeModelInfo(elementName)
                    .FirstOrDefault();
                if(modelInfo != null)
                {
                    return modelInfo.Module;
                }
                return null;
            }

            // Check if this is a Table
            var table = Common.CommonUtil.GetModelSaveService()
                            .GetTable(elementName);
            if (table != null)
            {
                var modelInfo = Common.CommonUtil.GetModelSaveService()
                    .GetTableModelInfo(elementName)
                    .FirstOrDefault();
                if (modelInfo != null)
                {
                    return modelInfo.Module;
                }
                return null;
            }

            // Check if this is a Class
            var classType = Common.CommonUtil.GetModelSaveService()
                                .GetClass(elementName);
            if (classType != null)
            {
                var modelInfo = Common.CommonUtil.GetModelSaveService()
                    .GetClassModelInfo(elementName)
                    .FirstOrDefault();
                if (modelInfo != null)
                {
                    return modelInfo.Module;
                }
                return null;
            }

            // Else not found
            return null;
        }

        /// <summary>
        /// Build the solution so that a new set of errors shows
        /// </summary>
        public void BuildSolution()
        {
            //Microsoft.Dynamics.AX.Metadata.Providers.IMetadataProvider
            var metaModelProviders = ServiceLocator.GetService(typeof(Microsoft.Dynamics.AX.Metadata.Providers.IMetadataProvider)) as Microsoft.Dynamics.AX.Metadata.Providers.IMetadataProvider;
            if(metaModelProviders !=null)
            {
                //TODO: #24 somehow refresh the models
                metaModelProviders.ModelManifest.RefreshModels();
            }

            //TODO: #24 somehow do a rebuild instead of a clean & build
            Common.CommonUtil.DTE.Solution.SolutionBuild.Clean(true);
            Common.CommonUtil.DTE.Solution.SolutionBuild.Build(true);
        }
    }
}
