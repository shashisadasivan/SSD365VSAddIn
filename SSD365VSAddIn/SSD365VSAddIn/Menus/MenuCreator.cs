using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus;

namespace SSD365VSAddIn.Menus
{
    class MenuCreator
    {
        public static string CreateExtension(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus.IMenu menu)
        {
            var existingItem = Menus.MenuHelper.GetExtensionObject(menu);
            if(existingItem != null)
            {
                // We already have an exisisting security duty extension, so just add this to the project
                // Add this to the project
                Common.CommonUtil.AddElementToProject(existingItem);
                return existingItem.Name;
            }

            var extensionName = Common.CommonUtil.GetNextMenuExtension(menu.Name);

            AxMenuExtension menuExtension = menuExtension = new AxMenuExtension() { Name = extensionName };
            Common.CommonUtil.GetMetaModelProviders().CurrentMetadataProvider
                .MenuExtensions.Create(menuExtension as AxMenuExtension, Common.CommonUtil.GetCurrentModelSaveInfo());
            
            // Add this to the project
            Common.CommonUtil.AddElementToProject(menuExtension);

            return extensionName;
        }
    }
}
