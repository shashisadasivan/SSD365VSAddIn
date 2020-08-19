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

            if (Settings.FetchSettings.FindOrCreateSettings().SecurityLabelAutoCreate)
            {
                if (String.IsNullOrEmpty(duty.Label) == false)
                {
                    duty.Label = Labels.LabelHelper.FindOrCreateLabel(duty.Label);
                }
                if (String.IsNullOrEmpty(duty.Description) == false)
                {
                    duty.Description = Labels.LabelHelper.FindOrCreateLabel(duty.Description);
                }
            }
            
            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();

            //Create menu item in the right model
            //var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;
            //var metaModelService = metaModelProviders.CurrentMetaModelService;

            var metaModelService = Common.CommonUtil.GetModelSaveService();
            metaModelService.CreateSecurityDuty(duty, modelSaveInfo);

            // Addd to project
            Common.CommonUtil.AddElementToProject(duty);

            return duty.Name;
        }

        public static string CreateDutyExtension(ISecurityDuty duty)
        {
            
            var existingSecDutyExt = SecurityDuty.SecurityDutyHelper.GetExtensionObject(duty);
            if(existingSecDutyExt != null)
            {
                // We already have an exisisting security duty extension, so just add this to the project
                // Add this to the project
                Common.CommonUtil.AddElementToProject(existingSecDutyExt);
                return existingSecDutyExt.Name;
            }

            var securityDutyExtensionName = Common.CommonUtil.GetNextNameSecurityDutyExtension(duty.Name);

            var extSecDuty = new AxSecurityDutyExtension() { Name = securityDutyExtensionName };
            // var secDuty = new AxSecurityDuty() { Name = securityDutyExtensionName };
            //var ex = new AxSecurityDutyExtension() { Name = securityDutyExtensionName };

            //create security duty extension
            Common.CommonUtil.GetMetaModelProviders()
                .CurrentMetadataProvider
                .SecurityDutyExtensions
                .Create(extSecDuty, Common.CommonUtil.GetCurrentModelSaveInfo());

            // Add this to the project
            Common.CommonUtil.AddElementToProject(extSecDuty);

            return securityDutyExtensionName;
        }
    }
}
