namespace SSD365VSAddIn.ClassesUtil
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel.Composition;
    using System.Drawing;
    using Microsoft.Dynamics.Framework.Tools.Extensibility;
    using Microsoft.Dynamics.AX.Metadata.MetaModel;
    using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
    using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Classes;
    using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
    using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews;
    using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;

    /// <summary>
    /// Creates class extension for a given class
    /// </summary>
    [Export(typeof(Microsoft.Dynamics.Framework.Tools.Extensibility.IDesignerMenu))] // IDesignerMenu
    // If you need to specify any other element, change this AutomationNodeType value.
    // You can specify multiple DesignerMenuExportMetadata attributes to meet your needs
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IClassItem))]
    [DesignerMenuExportMetadata(AutomationNodeType =typeof(ITable))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IDataEntityView))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IForm))]
    class ClassExtensionCreatorDesignContextMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.ClassExtensionCreatorDesignContextMenuAddIn";
        #endregion
        
        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.CreateCodeExtension;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return ClassExtensionCreatorDesignContextMenuAddIn.addinName;
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
                // we will create Extension class here
                var selectedElement = e.SelectedElement as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.IRootElement;
                if (selectedElement != null)
                {
                    // Find current model
                    var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();
                    var metaModelService = Common.CommonUtil.GetModelSaveService();

                    var metadataType = selectedElement.GetMetadataType() ;

                    // Create a class with the same name + _Extension and add it to the project
                    // ClassName
                    string baseClassName = selectedElement.Name + "_Extension";
                    string className = baseClassName;
                    int numClassesFound = 0;
                    while (metaModelService.GetClass(className) != null)
                    {
                        numClassesFound++;
                        className = baseClassName + numClassesFound.ToString();
                    }

                    string intrinsicStr = String.Empty;
                    if(selectedElement is ITable
                        || selectedElement is IDataEntityView)
                    {
                        intrinsicStr = "tableStr";
                    }
                    else if(selectedElement is IClassItem)
                    {
                        intrinsicStr = "classStr";
                    }
                    else if(selectedElement is IForm)
                    {
                        intrinsicStr = "formStr";
                    }

                    Microsoft.Dynamics.AX.Metadata.MetaModel.AxClass extensionClass = new AxClass()
                    {
                        Name = className
                    };

                    extensionClass.SourceCode.Declaration = $"[ExtensionOf({intrinsicStr}({selectedElement.Name}))]\npublic final class {className}\n{{\n\n}}";


                    metaModelService.CreateClass(extensionClass, modelSaveInfo);
                    Common.CommonUtil.AddElementToProject(extensionClass);

                    //var classx = metaModelService.GetClass("DXC_LedgerJournalEntity_Extension");
                    //int x = 10;

                    
                    
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
