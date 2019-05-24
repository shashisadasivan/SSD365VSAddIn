using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using Microsoft.Dynamics.AX.Metadata.MetaModel;

namespace SSD365VSAddIn.Forms
{
    /// <summary>
    /// Creates Display menu item for the selected Form design
    /// </summary>
    [Export(typeof(IDesignerMenu))]
    // If you need to specify any other element, change this AutomationNodeType value.
    // You can specify multiple DesignerMenuExportMetadata attributes to meet your needs
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IForm))]
    public class FormsUtilMenuItemAddin : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.FormsUtilMenuItemAddin";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.FormsUtilMenuItemAddin;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return FormsUtilMenuItemAddin.addinName;
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
            Microsoft.Dynamics.AX.Metadata.Core.MetaModel.EntryPointType entryPointType;
            try
            {
                // TODO: Do your magic for your add-in
                // we will create 2 security privileges for the menu item with the same name + Maintain,  +View
                var selectedMenuItem = e.SelectedElement as IForm;
                if(selectedMenuItem != null)
                {
                    var metadataType = selectedMenuItem.GetMetadataType();

                    if (selectedMenuItem is IForm)
                    {
                        IForm axForm = selectedMenuItem as IForm;

                        this.createMenuDisplayItem(axForm);
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

        private string createMenuDisplayItem(IForm iForm)
        {
            AxMenuItemDisplay menuItemDisplay = new AxMenuItemDisplay()
            {
                Name = iForm.Name,
                ObjectType = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.MenuItemObjectType.Form,
                Object = iForm.Name,
                Label = iForm.FormDesign.Caption
            };
            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();

            //Create menu item in the right model
            var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;
            var metaModelService = metaModelProviders.CurrentMetaModelService;

            metaModelService.CreateMenuItemDisplay(menuItemDisplay, modelSaveInfo);

            // Add the menu item display to the active project
            Common.CommonUtil.AddElementToProject(menuItemDisplay);

            // Common.CommonUtil.ShowLog($"Security privilege: {axSecurityPrivMaint} created");

            return menuItemDisplay.Name;
        }
    }
}
