using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.MenuItems
{
    class MenuItemCreator
    {
        public static string CreateExtension(IMenuItem menuItem)
        {
            var existingItem = MenuItems.MenuItemHelper.GetExtensionObject(menuItem);
            if(existingItem != null)
            {
                // We already have an exisisting security duty extension, so just add this to the project
                // Add this to the project
                Common.CommonUtil.AddElementToProject(existingItem);
                return existingItem.Name;
            }

            var extensionName = Common.CommonUtil.GetNextMenuItemExtension(
                menuItem.Name, 
                menuItem is IMenuItemAction ? 0
                                            : menuItem is IMenuItemDisplay ? 1 
                                                                           : 2
                );
            //AxMenuItemExtension menuItemExtension = menuItem is IMenuItemAction
            //                                            ? new AxMenuItemActionExtension()
            //                                            : menuItem is IMenuItemDisplay
            //                                                ? new AxMenuItemDisplayExtension()
            //                                                : new AxMenuItemOutputExtension();
            AxMenuItemExtension menuitemExtension = null;
            if (menuItem is IMenuItemAction)
            {
                menuitemExtension = new AxMenuItemActionExtension() { Name = extensionName };
                Common.CommonUtil.GetMetaModelProviders()
                .CurrentMetadataProvider
                .MenuItemActionExtensions.Create(menuitemExtension as AxMenuItemActionExtension, Common.CommonUtil.GetCurrentModelSaveInfo());
            }
            else if (menuItem is IMenuItemDisplay)
            {
                menuitemExtension = new AxMenuItemDisplayExtension() { Name = extensionName };
                Common.CommonUtil.GetMetaModelProviders()
                .CurrentMetadataProvider
                .MenuItemDisplayExtensions.Create(menuitemExtension as AxMenuItemDisplayExtension, Common.CommonUtil.GetCurrentModelSaveInfo());
            }
            if (menuItem is IMenuItemOutput)
            {
                menuitemExtension = new AxMenuItemOutputExtension() { Name = extensionName };
                Common.CommonUtil.GetMetaModelProviders()
                .CurrentMetadataProvider
                .MenuItemOutputExtensions.Create(menuitemExtension as AxMenuItemOutputExtension, Common.CommonUtil.GetCurrentModelSaveInfo());
            }

            
                //.SecurityDutyExtensions
                //.Create(extSecDuty, Common.CommonUtil.GetCurrentModelSaveInfo());

            // Add this to the project
            Common.CommonUtil.AddElementToProject(menuitemExtension);

            return extensionName;
        }
    }
}
