namespace SSD365VSAddIn.SecurityPrivilege
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
    using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus;
    using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews;

    /// <summary>
    /// Creates 2 Security privileges Maintain & View for the selected Menu item
    /// </summary>
    [Export(typeof(IDesignerMenu))]
    // If you need to specify any other element, change this AutomationNodeType value.
    // You can specify multiple DesignerMenuExportMetadata attributes to meet your needs
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IMenuItem))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IDataEntityView))]
    class SecurityPrivilegeCreatorMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.SecurityPrivilegeCreatorMenuAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.SecurityPrivilegeCreatorMenuAddInCaption;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return SecurityPrivilegeCreatorMenuAddIn.addinName;
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
                // we will create 2 security privileges for the menu item with the same name + Maintain,  +View
                var selectedMenuItem = e.SelectedElement as IMenuItem;
                if (selectedMenuItem != null)
                {
                    var metadataType = selectedMenuItem.GetMetadataType();

                    if (selectedMenuItem is IMenuItemAction)
                        entryPointType = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.EntryPointType.MenuItemAction;
                    else if (selectedMenuItem is IMenuItemDisplay)
                        entryPointType = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.EntryPointType.MenuItemDisplay;
                    else if (selectedMenuItem is IMenuItemOutput)
                        entryPointType = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.EntryPointType.MenuItemOutput;
                    else
                        return;

                    this.createSecurityElement_FromMenuItem(selectedMenuItem, entryPointType, "Maintain");
                    this.createSecurityElement_FromMenuItem(selectedMenuItem, entryPointType, "View");
                }
                else if(e.SelectedElement is IDataEntityView)
                {
                    IDataEntityView selectedElementDataEntity = e.SelectedElement as IDataEntityView;
                    this.createSecurityElement_FromDataEntity(selectedElementDataEntity, "Maintain");
                    this.createSecurityElement_FromDataEntity(selectedElementDataEntity, "View");
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
        #endregion

        private void createSecurityElement_FromDataEntity(IDataEntityView selectedDataEntity, string suffix)
        {
            //Create Security privilege
            AxSecurityPrivilege axSecurityPriv = new AxSecurityPrivilege() { Name = selectedDataEntity.Name + suffix };

            axSecurityPriv.DataEntityPermissions.Add(
                new AxSecurityDataEntityPermission()
                {
                    Grant = suffix == "Maintain" 
                            ? Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrant.ConstructGrantDelete()
                            : Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrant.ConstructGrantRead(),
                    Name = selectedDataEntity.Name
                });

            // Assign the correct label on this by addint maintain or view in the label, copy the base label from the menu item
            string label = selectedDataEntity.Label;
            if (label.StartsWith("@"))
            {
                //label = Labels.LabelHelper.FindLabel(label).LabelText;
                label = Labels.LabelHelper.FindLabelGlobally(label).LabelText;
            }

            if (suffix.Equals("Maintain"))
            {
                label = "Maintain " + label;
            }
            else if (suffix.Equals("View"))
            {
                label = "View " + label;
            }
            // Convert to camel case
            if (String.IsNullOrEmpty(label) == false)
            {
                char[] a = label.ToLowerInvariant().ToCharArray();
                a[0] = char.ToUpperInvariant(a[0]);
                label = new String(a);
            }
            axSecurityPriv.Label = label;
            if (Settings.FetchSettings.FindOrCreateSettings().SecurityLabelAutoCreate)
            {
                axSecurityPriv.Label = Labels.LabelHelper.FindOrCreateLabel(axSecurityPriv.Label);
            }
              
            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();

            var metaModelService = Common.CommonUtil.GetModelSaveService();
            metaModelService.CreateSecurityPrivilege(axSecurityPriv, modelSaveInfo);

            Common.CommonUtil.AddElementToProject(axSecurityPriv);
        }

        private string createSecurityElement_FromMenuItem(
                IMenuItem selectedMenuItem, 
                Microsoft.Dynamics.AX.Metadata.Core.MetaModel.EntryPointType entryPointType, 
                string suffix)
        {
            // Find project to work with
            //VSProjectNode project = LocalUtils.GetActiveProject();

            //Create Security privilege
            AxSecurityPrivilege axSecurityPrivMaint = new AxSecurityPrivilege() { Name = selectedMenuItem.Name + suffix };

            var entryPoint = new AxSecurityEntryPointReference()
            {
                ObjectType = entryPointType,
                ObjectName = selectedMenuItem.Name,
                Name = selectedMenuItem.Name,
                Grant = new Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrant()
                {
                    Delete = suffix.Equals("Maintain")
                                                    ? Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow
                                                    : Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Unset,
                    Correct = suffix.Equals("Maintain")
                                                    ? Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow
                                                    : Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Unset,
                    Create = suffix.Equals("Maintain")
                                                    ? Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow
                                                    : Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Unset,
                    //Invoke = suffix.Equals("Maintain")
                    //                                ? Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow
                    //                                : Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Unset,
                    Update = suffix.Equals("Maintain")
                                                    ? Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow
                                                    : Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Unset,
                    Read = suffix.Equals("View")
                                                    ? Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Allow
                                                    : Microsoft.Dynamics.AX.Metadata.Core.MetaModel.AccessGrantPermission.Unset
                }
            };

            if (selectedMenuItem.ObjectType == Microsoft.Dynamics.AX.Metadata.Core.MetaModel.MenuItemObjectType.Form)
            {
                entryPoint.Forms.Add(new AxSecurityEntryPointReferenceForm() { Name = selectedMenuItem.Object });
            }
            axSecurityPrivMaint.EntryPoints.Add(entryPoint);
           

            // Assign the correct label on this by addint maintain or view in the label, copy the base label from the menu item
            string label = selectedMenuItem.Label;
            if(label.StartsWith("@"))
            {
                label = Labels.LabelHelper.FindLabelGlobally(label).LabelText;
            }
            
            if(suffix.Equals("Maintain"))
            {
                label = "Maintain " + label;
            }
            else if (suffix.Equals("View"))
            {
                label = "View " + label;
            }
            // Convert to camel case
            if (String.IsNullOrEmpty(label) == false)
            {
                char[] a = label.ToLowerInvariant().ToCharArray();
                a[0] = char.ToUpperInvariant(a[0]);
                label = new String(a);
            }
            axSecurityPrivMaint.Label = label;
            if (Settings.FetchSettings.FindOrCreateSettings().SecurityLabelAutoCreate)
            {
                axSecurityPrivMaint.Label = Labels.LabelHelper.FindOrCreateLabel(axSecurityPrivMaint.Label);
            }

            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();

            //Create item in the right model
            //var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;
            //var metaModelService = metaModelProviders.CurrentMetaModelService;
            var metaModelService = Common.CommonUtil.GetModelSaveService();
            metaModelService.CreateSecurityPrivilege(axSecurityPrivMaint, modelSaveInfo);
            //metaModelService.CreateClass(axClass, saveInfo);

            // Add the class to the active project
            //var projectService = ServiceLocator.GetService(typeof(IDynamicsProjectService)) as IDynamicsProjectService;
            //projectService.AddElementToActiveProject(axSecurityPrivMaint);
            Common.CommonUtil.AddElementToProject(axSecurityPrivMaint);

            // Common.CommonUtil.ShowLog($"Security privilege: {axSecurityPrivMaint} created");

            return axSecurityPrivMaint.Name;
        }
    }
}
