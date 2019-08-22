using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.BaseTypes;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.AX.Metadata.MetaModel;

namespace SSD365VSAddIn.DataTypes
{
    class BaseEnumHelper
    {
        public static string CreateExtension(IBaseEnum baseEnum)
        {
            var name = baseEnum.Name + Common.Constants.DotEXTENSION;
            name = Common.CommonUtil.GetNextBaseEnumExtension(name);

            // Find current model
            //Create menu item in the right model
            var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;


            //Create an extension object
            var axExtension = new AxEnumExtension() { Name = name };
            //var tableExts = metaModelProviders.CurrentMetadataProvider.TableExtensions.Common.CommonUtil.GetCurrentModel().Name);

            Common.CommonUtil.GetMetaModelProviders()
                .CurrentMetadataProvider
                .EnumExtensions.Create(axExtension, Common.CommonUtil.GetCurrentModelSaveInfo());

            // Add to project
            Common.CommonUtil.AddElementToProject(axExtension);

            return name;
        }
    }
}
