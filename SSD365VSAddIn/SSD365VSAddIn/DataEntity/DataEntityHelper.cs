using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.DataEntity
{
    class DataEntityHelper
    {
        public static string CreateExtension(IDataEntity dataEntity)
        {
            AxDataEntityViewExtension axExtension;
            axExtension = DataEntityHelper.GetFirstExtension(dataEntity.Name);
            if (axExtension != null)
            {
                // Add existing extension to project & quit
                Common.CommonUtil.AddElementToProject(axExtension);
                return axExtension.Name;
            }

            var name = dataEntity.Name;// + Common.Constants.DotEXTENSION;
            name = Common.CommonUtil.GetNextTableExtension(name);

            // Find current model
            //Create menu item in the right model
            var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;


            //Create an extension object
            axExtension = new AxDataEntityViewExtension() { Name = name };
            //var tableExts = metaModelProviders.CurrentMetadataProvider.TableExtensions.Common.CommonUtil.GetCurrentModel().Name);

            Common.CommonUtil.GetMetaModelProviders()
                .CurrentMetadataProvider
                .DataEntityViewExtensions.Create(axExtension, Common.CommonUtil.GetCurrentModelSaveInfo());

            // Add to project
            Common.CommonUtil.AddElementToProject(axExtension);

            return name;
        }

        public static AxDataEntityViewExtension GetFirstExtension(string name)
        {
            // Find current model
            var metaModelService = Common.CommonUtil.GetModelSaveService();
            // Currently the metaModelService API doesnt have a method for GetDataEntityViewExtensions (however, decompiling the dll, i found that it uses the provider to get that data)
            var extensionName = metaModelService.MetadataProvider.DataEntityViewExtensions.GetPrimaryKeys()
                                    .Where(extName => extName.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
                                    .FirstOrDefault();
            /*
            
            
            var extensionName = metaModelService.getDataEntity()
                                    .ToList()
                                    .Where(tableExtName => tableExtName.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
                                    .FirstOrDefault();
            */
            if (String.IsNullOrEmpty(extensionName) == false)
            {
                // Again this method for GetDataEntityViewExtension is not implemetned in the API
                var extension = metaModelService.MetadataProvider.DataEntityViewExtensions.Read(extensionName);
                //var extension = metaModelService.extension .GetTableExtension(extensionName);
                return extension;
            }

            return null;
        }
    }
}
