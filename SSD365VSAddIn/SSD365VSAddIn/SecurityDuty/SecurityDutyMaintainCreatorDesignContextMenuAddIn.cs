using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security;
using System.ComponentModel.Composition;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using Microsoft.Dynamics.AX.Metadata.MetaModel;

namespace SSD365VSAddIn.SecurityDuty
{
    /// <summary>
    /// Creates Security Duty for a given Object
    /// </summary>
    [Export(typeof(Microsoft.Dynamics.Framework.Tools.Extensibility.IDesignerMenu))] // IDesignerMenu
    // If you need to specify any other element, change this AutomationNodeType value.
    // You can specify multiple DesignerMenuExportMetadata attributes to meet your needs
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ISecurityPrivilege))]
    class SecurityDutyMaintainCreatorDesignContextMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.SecurityDutyMaintainCreatorDesignContextMenuAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.SecurityDutyMaintainCreatorDesignContextMenuAddInCaption;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return SecurityDutyMaintainCreatorDesignContextMenuAddIn.addinName;
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
            try
            {
                //var selectedItem = e.SelectedElement as ISecurityPrivilege;
                if (e.SelectedElement is ISecurityPrivilege)
                {
                    //var metadataType = selectedItem.GetMetadataType();

                    SecurityDutyCreator.CreateDuty_fromSecPriv(e.SelectedElement as ISecurityPrivilege, Common.Constants.MAINTAIN);

                    //this.createSecurityElement_FromMenuItem(selectedItem, entryPointType, "Maintain");
                    //this.createSecurityElement_FromMenuItem(selectedItem, entryPointType, "View");
                }
                
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
        #endregion

        
    }
}
