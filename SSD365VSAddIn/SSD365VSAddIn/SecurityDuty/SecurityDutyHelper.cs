using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.SecurityDuty
{
    class SecurityDutyHelper
    {
        public static AxSecurityDutyExtension GetExtensionObject(ISecurityDuty securityDuty)
        {
            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();
            var metaModelService = Common.CommonUtil.GetModelSaveService();

            var securityDutyExtensions = metaModelService.GetSecurityDutyExtensionNames().ToList()
                                            .Where(extName => extName.StartsWith(securityDuty.Name, StringComparison.InvariantCultureIgnoreCase))
                                            .ToList();
            if(securityDutyExtensions != null)
            {
                var currentModel = Common.CommonUtil.GetCurrentModel();
                foreach (var dutyExtName in securityDutyExtensions)
                {
                    var dutyExtModels = metaModelService.GetSecurityDutyExtensionModelInfo(dutyExtName).ToList();
                    if (dutyExtModels != null)
                    {
                        foreach (var model in dutyExtModels)
                        {
                            if (model.Module.Equals(currentModel.Module, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return metaModelService.GetSecurityDutyExtension(dutyExtName);
                            }
                        }
                    }
                }
                
            }
            return null;

            //var securityDutyExtension = metaModelService.GetSecurityDutyExtensionNames().ToList()
            //                                .Where(extName => extName.StartsWith(securityDuty.Name, StringComparison.InvariantCultureIgnoreCase))
            //                                .FirstOrDefault() ;

            //if(String.IsNullOrEmpty(securityDutyExtension) == false)
            //{
            //    var extension = metaModelService.GetSecurityDutyExtension(securityDutyExtension);
            //    return extension;
            //}
            //return null;
        }
    }
}
