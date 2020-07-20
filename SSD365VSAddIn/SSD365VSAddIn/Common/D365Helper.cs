using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Classes;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Common
{
    class D365Helper
    {
        /// <summary>
        /// Check the extension name in D365 if it exists, else adds a number at the end until it does not exist
        /// </summary>
        /// <param name="d365Object"></param>
        /// <returns></returns>
        public static string GetCheckExtensionObjectName(IRootElement d365Object)
        {
            string name = d365Object.Name;

            var metaModelService = Common.CommonUtil.GetModelSaveService();

            // Create a class with the same name + _Extension and add it to the project
            // ClassName
            int numFound = 0;
            if (d365Object is ISecurityDuty)
            {
                while (metaModelService.GetSecurityDutyExtension(name) != null)
                {
                    numFound++;
                    name = d365Object.Name + numFound.ToString();
                }
            }
            else if(d365Object is IClassItem)
            {
                while (metaModelService.GetClass(name) != null)
                {
                    numFound++;
                    name = d365Object.Name + numFound.ToString();
                }
            }
            return name;
        }

        public static Type GetElementType(INamedElement namedElement)
        {
            Type elementType = namedElement.GetType();

            if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.BaseTypes.BaseEnum))
            {
                elementType = typeof(AxEnum);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.BaseTypes.BaseEnumExtension))
            {
                elementType = typeof(AxEnumExtension);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.BaseTypes.EdtBase))
            {
                elementType = typeof(AxEdt);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.BaseTypes.EdtExtension))
            {
                elementType = typeof(AxEdtExtension);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.BaseTypes.BaseEnum))
            {
                elementType = typeof(AxEnum);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables.Table))
            {
                elementType = typeof(AxTable);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables.TableExtension))
            {
                elementType = typeof(AxTableExtension);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Views.View))
            {
                elementType = typeof(AxView);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Views.ViewExtension))
            {
                elementType = typeof(AxViewExtension);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Query))
            {
                elementType = typeof(AxQuery);
            }
            // Query extension ?
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntity))
            {
                elementType = typeof(AxDataEntity);
            }
            // Dataentity extension ?
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Classes.ClassItem))
            {
                elementType = typeof(AxClass);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms.Form))
            {
                elementType = typeof(AxForm);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms.FormExtension))
            {
                elementType = typeof(AxFormExtension);
            }
            // Tiles ?
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus.Menu))
            {
                elementType = typeof(AxMenu);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus.MenuExtension))
            {
                elementType = typeof(AxMenuExtension);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus.MenuItem))
            {
                elementType = typeof(AxMenuItem);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus.MenuItemExtension))
            {
                elementType = typeof(AxMenuItemExtension);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Reports.Report))
            {
                elementType = typeof(AxReport);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security.SecurityRole))
            {
                elementType = typeof(AxSecurityRole);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security.SecurityRoleExtension))
            {
                elementType = typeof(AxSecurityRoleExtension);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security.SecurityDuty))
            {
                elementType = typeof(AxSecurityDuty);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security.SecurityDutyExtension))
            {
                elementType = typeof(AxSecurityDutyExtension);
            }
            else if (namedElement.GetType() == typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security.SecurityPrivilege))
            {
                elementType = typeof(AxSecurityPrivilege);
            }

            return elementType;
        }
    }
}
