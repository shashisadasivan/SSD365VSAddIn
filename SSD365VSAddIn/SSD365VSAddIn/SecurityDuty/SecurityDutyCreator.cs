using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.SecurityDuty
{
    class SecurityDutyCreator
    {
        public static string CreateDuty_fromSecPriv(ISecurityPrivilege privilege, string postfix)
        {
            var name = privilege.Name;
            if (name.EndsWith(postfix))
            {
                // We dont need to do anything here
                postfix = String.Empty;
            }
            else if (postfix.Equals(Common.Constants.INQUIRE, StringComparison.InvariantCultureIgnoreCase)
                && name.EndsWith(Common.Constants.VIEW, StringComparison.InvariantCultureIgnoreCase))
            {
                name = name.Replace(Common.Constants.VIEW, String.Empty);
            }

            var duty = new AxSecurityDuty() { Name = privilege.Name + postfix };
            duty.Privileges.Add(new AxSecurityPrivilegeReference() { Name = privilege.Name });
            duty.Label = privilege.Label;
            duty.Description = privilege.Description;

            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();

            //Create menu item in the right model
            var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;
            var metaModelService = metaModelProviders.CurrentMetaModelService;

            metaModelService.CreateSecurityDuty(duty, modelSaveInfo);

            // Addd to project
            Common.CommonUtil.AddElementToProject(duty);

            return duty.Name;


        }
    }
}
