using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Tables
{
    /// <summary>
    /// Creates Display menu item for the selected Form design
    /// </summary>
    [Export(typeof(IDesignerMenu))]
    // If you need to specify any other element, change this AutomationNodeType value.
    // You can specify multiple DesignerMenuExportMetadata attributes to meet your needs
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ITable))]
    public class TableFormCreatorMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.TableFormCreatorMenuAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.TableFormCreatorMenuAddIn;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return TableFormCreatorMenuAddIn.addinName;
            }
        }
        #endregion

        #region Callbacks
        /// <summary>
        /// Called when user clicks on the add-in menu
        /// </summary>
        /// <param name="e">The context of the VS tools and metadata</param>
        public override void OnClick(AddinDesignerEventArgs e)
        {
            //Microsoft.Dynamics.AX.Metadata.Core.MetaModel.EntryPointType entryPointType;
            try
            {
                // we will create 2 security privileges for the menu item with the same name + Maintain,  +View
                var selectedMenuItem = e.SelectedElement as ITable;
                if (selectedMenuItem != null)
                {
                    var metadataType = selectedMenuItem.GetMetadataType();

                    if (selectedMenuItem is ITable)
                    {
                        ITable axForm = selectedMenuItem as ITable;

                        this.createForm(axForm);
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }

        #endregion


        private string createForm(ITable table)
        {
            AxForm axForm = new AxForm()
            {
                Name = table.Name,
                Design = new AxFormDesign() { Caption = table.Label }
            };
            axForm.DataSources.Add(new AxFormDataSourceRoot() { Table = table.Name, Name = table.Name });

            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();

            //Create menu item in the right model
            var metaModelService = Common.CommonUtil.GetModelSaveService();
            metaModelService.CreateForm(axForm, modelSaveInfo);

            // Add the menu item display to the active project
            Common.CommonUtil.AddElementToProject(axForm);

            // Common.CommonUtil.ShowLog($"Security privilege: {axSecurityPrivMaint} created");

            return axForm.Name;
        }
    }
}
