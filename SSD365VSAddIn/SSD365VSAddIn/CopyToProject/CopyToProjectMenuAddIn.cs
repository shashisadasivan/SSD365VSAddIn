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

namespace SSD365VSAddIn.CopyToProject
{
    //[Export(typeof(IDesignerMenu))] // enable this
    //[DesignerMenuExportMetadata(AutomationNodeType = typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.IRootElement))] // enable this
    //[DesignerMenuExportMetadata(AutomationNodeType = typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.IRootExtensionElement))] // enable this
    public class CopyToProjectMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.CopyToProjectMenuAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return "Duplicate to prooject"; //AddinResources.AddToProjectMenuAddIn;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return "Duplicate to project";//TODO: create label // AddToProjectMenuAddIn.addinName;
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
                    CopyToProjectMenuAddIn.CopyExistingElementToProject(namedElement);
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
        #endregion

        internal static void CopyExistingElementToProject(INamedElement namedElement)
        {
            if (namedElement is Microsoft.Dynamics.AX.Metadata.Core.MetaModel.INamedObject)
            {
                Common.CommonUtil.DuplicateElementToProject(namedElement as Microsoft.Dynamics.AX.Metadata.Core.MetaModel.INamedObject);
            }
        }
    }
}
