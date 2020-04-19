using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.MenuItems
{
    class MenuItemHelper
    {
        public static AxMenuItemExtension GetExtensionObject(IMenuItem menuItem)
        {
            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();
            var metaModelService = Common.CommonUtil.GetModelSaveService();

            if (menuItem is IMenuItemAction)
            {
                var ext = metaModelService.GetMenuItemActionExtensionNames().ToList()
                                            .Where(extName => extName.StartsWith(menuItem.Name, StringComparison.InvariantCultureIgnoreCase))
                                            .ToList();
                if (ext != null)
                {
                    var currentModel = Common.CommonUtil.GetCurrentModel();
                    foreach (var extName in ext)
                    {
                        var extModels = metaModelService.GetMenuItemActionExtensionModelInfo(extName)
                                        .Where(modelInfo => modelInfo.Module.Equals(currentModel.Module, StringComparison.InvariantCultureIgnoreCase))
                                        .FirstOrDefault();
                        if (extModels != null)
                            return metaModelService.GetMenuItemActionExtension(extName);
                        //if(extModels != null)
                        //{
                        //    foreach (var model in extModels)
                        //    {
                        //        if (model.Module.Equals(currentModel.Module, StringComparison.InvariantCultureIgnoreCase))
                        //        {
                        //            return metaModelService.GetMenuItemActionExtension(extName);
                        //        }
                        //    }
                        //}
                    }
                }
            }
            else if (menuItem is IMenuItemDisplay)
            {
                var ext = metaModelService.GetMenuItemDisplayExtensionNames().ToList()
                                            .Where(extName => extName.StartsWith(menuItem.Name, StringComparison.InvariantCultureIgnoreCase))
                                            .ToList();
                if (ext != null)
                {
                    var currentModel = Common.CommonUtil.GetCurrentModel();
                    foreach (var extName in ext)
                    {
                        var extModels = metaModelService.GetMenuItemDisplayExtensionModelInfo(extName)
                                        .Where(modelInfo => modelInfo.Module.Equals(currentModel.Module, StringComparison.InvariantCultureIgnoreCase))
                                        .FirstOrDefault();
                        if (extModels != null)
                            return metaModelService.GetMenuItemDisplayExtension(extName);
                    }
                }
            }
            else if (menuItem is IMenuItemOutput)
            {
                var ext = metaModelService.GetMenuItemOutputExtensionNames().ToList()
                                            .Where(extName => extName.StartsWith(menuItem.Name, StringComparison.InvariantCultureIgnoreCase))
                                            .ToList();
                if (ext != null)
                {
                    var currentModel = Common.CommonUtil.GetCurrentModel();
                    foreach (var extName in ext)
                    {
                        var extModels = metaModelService.GetMenuItemOutputExtensionModelInfo(extName)
                                        .Where(modelInfo => modelInfo.Module.Equals(currentModel.Module, StringComparison.InvariantCultureIgnoreCase))
                                        .FirstOrDefault();
                        if (extModels != null)
                            return metaModelService.GetMenuItemOutputExtension(extName);
                    }
                }
            }
            return null;
            
        }
    }
}
