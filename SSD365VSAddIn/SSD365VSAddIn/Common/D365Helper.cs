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
    }
}
