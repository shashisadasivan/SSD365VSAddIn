using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Classes;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.ExtensionCommand
{
    /// <summary>
    /// Creates a Extension from given object
    /// </summary>
    [Export(typeof(Microsoft.Dynamics.Framework.Tools.Extensibility.IDesignerMenu))] // IDesignerMenu
    // If you need to specify any other element, change this AutomationNodeType value.
    // You can specify multiple DesignerMenuExportMetadata attributes to meet your needs
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ISecurityDuty))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ITable))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IForm))]
    class CreateExtensionCreatorDesignContextMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.CreateExtensionCreatorDesignContextMenuAddIn";
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
                return CreateExtensionCreatorDesignContextMenuAddIn.addinName;
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
                    SecurityDuty.SecurityDutyCreator.CreateDutyExtension(e.SelectedElement as ISecurityDuty);
                }
                else if (e.SelectedElement is ITable)
                {
                    Tables.TableHelper.CreateTableExtension(e.SelectedElement as ITable);
                }
                else if (e.SelectedElement is IForm)
                {
                    Forms.FormHelper.CreateExtension(e.SelectedElement as IForm);
                }
                else
                {
                    CoreUtility.DisplayError("Not implemented yet");
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
