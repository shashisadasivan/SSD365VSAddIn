using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Menus
{
    class MenuHelper
    {
        public static AxMenuExtension GetExtensionObject(IMenu menu)
        {
            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();
            var metaModelService = Common.CommonUtil.GetModelSaveService();

            var ext = metaModelService.GetMenuExtensionNames().ToList()
                                        .Where(extName => extName.StartsWith(menu.Name, StringComparison.InvariantCultureIgnoreCase))
                                        .ToList();
            if (ext != null)
            {
                var currentModel = Common.CommonUtil.GetCurrentModel();
                foreach (var extName in ext)
                {
                    var extModels = metaModelService.GetMenuExtensionModelInfo(extName)
                                    .Where(modelInfo => modelInfo.Module.Equals(currentModel.Module, StringComparison.InvariantCultureIgnoreCase))
                                    .FirstOrDefault();
                    if (extModels != null)
                        return metaModelService.GetMenuExtension(extName);
                }
            }
            
            return null;
            
        }
    }
}
