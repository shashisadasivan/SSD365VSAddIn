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
    [DesignerMenuExportMetadata(AutomationNodeType =typeof(ITableExtension))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IDataEntityView))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IForm))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFormExtension))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFormDataSource))]
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

                    string elementName = selectedElement.Name;

                    string intrinsicStr = String.Empty;
                    if(selectedElement is ITable
                        || selectedElement is IDataEntityView
                        || selectedElement is ITableExtension)
                    {
                        intrinsicStr = "tableStr";
                        if (selectedElement is ITableExtension)
                        {
                            // we need to take out everything after and including the . 
                            elementName = elementName.Substring(0, elementName.IndexOf("."));
                        }
                    }
                    else if(selectedElement is IClassItem)
                    {
                        intrinsicStr = "classStr";
                    }
                    else if(selectedElement is IForm || selectedElement is IFormExtension)
                    {
                        intrinsicStr = "formStr";
                        if(selectedElement is IFormExtension)
                        {
                            // we need to take out everything after and including the . 
                            elementName = elementName.Substring(0, elementName.IndexOf("."));
                        }
                    }
                    
                    // Create a class with the same name + _Extension and add it to the project
                    string className = Common.CommonUtil.GetNextClassExtensionName(elementName);

                    //Microsoft.Dynamics.AX.Metadata.MetaModel.AxClass extensionClass;

                    string extensionOfStr = $"ExtensionOf({intrinsicStr}({elementName}))";
                    // Find an existing class where the extension is already used
                    var extensionClass = ClassHelper.GetExistingExtensionClass(elementName, extensionOfStr);
                    if (extensionClass == null)
                    {
                        extensionClass = new AxClass()
                        {
                            Name = className
                        };

                        extensionClass.SourceCode.Declaration = $"[{extensionOfStr}]\npublic final class {className}\n{{\n\n}}";
                    }

                    metaModelService.CreateClass(extensionClass, modelSaveInfo);
                    Common.CommonUtil.AddElementToProject(extensionClass);
                }
                else if(e.SelectedElement is IFormDataSource)
                {
                    this.CreateFormDSExtension(e.SelectedElement as IFormDataSource);
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
        #endregion

        public void CreateFormDSExtension(IFormDataSource formDataSource)
        {
            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();
            var metaModelService = Common.CommonUtil.GetModelSaveService();

            string className = Common.CommonUtil.GetNextClassExtensionName(formDataSource.Name);
            string intrinsicStr = "formdatasourcestr";

            string extensionOfStr = $"ExtensionOf({intrinsicStr}({formDataSource.RootElement.Name},{formDataSource.Name}))";
            Microsoft.Dynamics.AX.Metadata.MetaModel.AxClass extensionClass = ClassHelper.GetExistingExtensionClass(formDataSource.Name, extensionOfStr);
            if (extensionClass == null)
            {
                extensionClass = new AxClass()
                {
                    Name = className
                };

                extensionClass.SourceCode.Declaration = $"[{extensionOfStr}]\npublic final class {className}\n{{\n\n}}";
            }

            metaModelService.CreateClass(extensionClass, modelSaveInfo);
            Common.CommonUtil.AddElementToProject(extensionClass);
        }
    }


}
