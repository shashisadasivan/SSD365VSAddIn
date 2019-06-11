using Microsoft.Dynamics.AX.Metadata.Core.MetaModel;
using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.AddToProject
{
    /// <summary>
    /// Creates Display menu item for the selected Form design
    /// </summary>
    //[Export(typeof(IDesignerMenu))] // enable this
    // If you need to specify any other element, change this AutomationNodeType value.
    // You can specify multiple DesignerMenuExportMetadata attributes to meet your needs
    //[DesignerMenuExportMetadata(AutomationNodeType = typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.IRootElement))] // enable this
    //[DesignerMenuExportMetadata(AutomationNodeType = typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.IRootExtensionElement))] // enable this
    public class AddToProjectMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.AddToProjectMenuAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.AddToProjectMenuAddIn;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return AddToProjectMenuAddIn.addinName;
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
                INamedElement namedElement = e.SelectedElement as INamedElement;
                if (namedElement != null)
                {
                    //Common.CommonUtil.AddElementToProject(namedObject);
                    AddToProjectMenuAddIn.AddExistingElementToProject(namedElement);
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
        #endregion


        /// <summary>
        /// TODO: Unfortunately AddExistingElementToProject this doesnt work - and gives an error that says - Metadata operation <Exists> on type <> does not exist
        /// </summary>
        /// <param name="namedElement"></param>
        internal static void AddExistingElementToProject(INamedElement namedElement)
        {
            var vsProject = Common.CommonUtil.GetCurrentProject();
            if (vsProject != null)
            {
                vsProject.AddModelElementsToProject(new List<MetadataReference>()
                {
                    new MetadataReference(namedElement.Name, namedElement.GetType())
                });
            }
        }
    }
}
