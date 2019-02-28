using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Tables
{
    class TableHelper
    {
        public static string CreateTableExtension(ITable table)
        {
            var name = table.Name + Common.Constants.DotEXTENSION;
            name = Common.CommonUtil.GetNextTableExtension(name);

            var axTableExtension = new AxTableExtension() { Name = name };

            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();

            //Create menu item in the right model
            var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;
            var metaModelService = metaModelProviders.CurrentMetaModelService;

            //TODO: how do we create an extension object ?
            // metaModelService.CreateTable(axTableExtension);
            
            // Addd to project
            //TODO: this isint working and throws an error on the front end
            // Common.CommonUtil.AddElementToProject(axTableExtension);

            return axTableExtension.Name;
        }
    }
}
