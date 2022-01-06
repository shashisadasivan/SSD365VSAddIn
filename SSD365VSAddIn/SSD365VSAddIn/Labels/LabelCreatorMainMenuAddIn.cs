using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.BaseTypes;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Labels
{
    /// <summary>
    /// Creates a Label for the selected elements
    /// </summary>
    [Export(typeof(IDesignerMenu))]
    // If you need to specify any other element, change this AutomationNodeType value.
    // You can specify multiple DesignerMenuExportMetadata attributes to meet your needs
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ITable))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ITableExtension))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IDataEntity))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IDataEntityViewExtension))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IMenuItem))]
    //[DesignerMenuExportMetadata(AutomationNodeType = typeof(IMenuItemExtension))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IForm))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFormExtension))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ISecurityDuty))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ISecurityPrivilege))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ISecurityRole))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IEdtBase))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IBaseEnum))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IBaseEnumExtension))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IConfigurationKey))]
    class LabelCreatorMainMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.LabelCreatorMainMenuAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.LabelCreatorForProperties;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return LabelCreatorMainMenuAddIn.addinName;
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
                var selectedItem = e.SelectedElement as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.IRootElement;
                if (selectedItem != null)
                {
                    var metadataType = selectedItem.GetMetadataType();

                    LabelFactory labelFactory = LabelFactory.construct(selectedItem);
                    labelFactory.ApplyLabel();
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }

        #endregion

        protected void createLabelsTable(ITable table)
        {
            table.Label = LabelHelper.FindOrCreateLabel(table.Label);
            table.DeveloperDocumentation = LabelHelper.FindOrCreateLabel(table.DeveloperDocumentation);
        }
    }
}
