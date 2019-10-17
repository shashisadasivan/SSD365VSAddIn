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
            AxEnumExtension axExtension;
            axExtension = BaseEnumHelper.GetFirstExtension(baseEnum.Name);
            if (axExtension != null)
            {
                // Add existing extension to project & quit
                Common.CommonUtil.AddElementToProject(axExtension);
                return axExtension.Name;
            }

            var name = baseEnum.Name; // + Common.Constants.DotEXTENSION;
            name = Common.CommonUtil.GetNextBaseEnumExtension(name);

            // Find current model
            //Create menu item in the right model
            var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;


            //Create an extension object
            axExtension = new AxEnumExtension() { Name = name };
            //var tableExts = metaModelProviders.CurrentMetadataProvider.TableExtensions.Common.CommonUtil.GetCurrentModel().Name);

            Common.CommonUtil.GetMetaModelProviders()
                .CurrentMetadataProvider
                .EnumExtensions.Create(axExtension, Common.CommonUtil.GetCurrentModelSaveInfo());

            // Add to project
            Common.CommonUtil.AddElementToProject(axExtension);

            return name;
        }

        public static AxEnumExtension GetFirstExtension(string name)
        {
            // Find current model
            var metaModelService = Common.CommonUtil.GetModelSaveService();

            var extensionNames = metaModelService.GetEnumExtensionNames()
                                    .ToList()
                                    .Where(extName => extName.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
                                    .ToList();

            if (extensionNames == null)
            {
                return null;
            }

            var currentModel = Common.CommonUtil.GetCurrentModel();
            foreach (var extName in extensionNames)
            {
                var extModels = metaModelService.GetEnumExtensionModelInfo(extName).ToList();
                if (extModels != null)
                {
                    foreach (var model in extModels)
                    {
                        if (model.Module.Equals(currentModel.Module, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return metaModelService.GetEnumExtension(extName);
                        }
                    }
                }
            }

            //if (String.IsNullOrEmpty(extensionName) == false)
            //{
            //    var extension = metaModelService.GetEnumExtension(extensionName);
            //    return extension;
            //}

            return null;
        }
    }
}
