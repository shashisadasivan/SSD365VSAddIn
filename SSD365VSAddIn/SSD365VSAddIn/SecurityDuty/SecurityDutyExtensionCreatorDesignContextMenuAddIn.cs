using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.SecurityDuty
{
    /*
    /// <summary>
    /// Creates a Security Duty extension for a given Security Duty
    /// </summary>
    [Export(typeof(Microsoft.Dynamics.Framework.Tools.Extensibility.IDesignerMenu))] // IDesignerMenu
    // If you need to specify any other element, change this AutomationNodeType value.
    // You can specify multiple DesignerMenuExportMetadata attributes to meet your needs
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ISecurityDuty))]
    class SecurityDutyExtensionCreatorDesignContextMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.SecurityDutyExtensionCreatorDesignContextMenuAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.CreateExtension;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return SecurityDutyExtensionCreatorDesignContextMenuAddIn.addinName;
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
                if (e.SelectedElement is ISecurityDuty)
                {
                    SecurityDutyExtensionCreatorDesignContextMenuAddIn.CreateDutyExtension(e.SelectedElement as ISecurityDuty);
                }

            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
        #endregion


        public static string CreateDutyExtension(ISecurityDuty securityDuty)
        {
            var name = securityDuty.Name + Common.Constants._EXTENSION;
            name = Common.CommonUtil.GetNextNameSecurityDutyExtension(name);

            var duty = new AxSecurityDutyExtension() { Name = name };

            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();

            //Create menu item in the right model
            var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;
            var metaModelService = metaModelProviders.CurrentMetaModelService;

            //TODO: how do we create an extension object ?
            //metaModelService.CreateSecurityDuty(duty, modelSaveInfo);

            // Addd to project
            //Common.CommonUtil.AddElementToProject(duty);

            return duty.Name;
        }
    }
    */
}
