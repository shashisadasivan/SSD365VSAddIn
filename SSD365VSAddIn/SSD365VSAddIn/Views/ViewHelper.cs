using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Views;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Views
{
    class ViewHelper
    {
        public static string CreateExtension(IView view)
        {
            AxViewExtension axExtension;
            axExtension = ViewHelper.GetFirstExtension(view.Name);
            if(axExtension != null)
            {
                // Add existing extension to project & quit
                Common.CommonUtil.AddElementToProject(axExtension);
                return axExtension.Name;
            }

            var name = view.Name;
            name = Common.CommonUtil.GetNextTableExtension(name);

            // Find current model
            //Create menu item in the right model
            //var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;


            //Create an extension object
            axExtension = new AxViewExtension() { Name = name };
            //var tableExts = metaModelProviders.CurrentMetadataProvider.TableExtensions.Common.CommonUtil.GetCurrentModel().Name);

            Common.CommonUtil.GetMetaModelProviders()
                .CurrentMetadataProvider
                .ViewExtensions.Create(axExtension, Common.CommonUtil.GetCurrentModelSaveInfo());

            // Add to project
            Common.CommonUtil.AddElementToProject(axExtension);

            return name;
        }

        public static AxViewExtension GetFirstExtension(string name)
        {
            // Find current model
            var metaModelService = Common.CommonUtil.GetModelSaveService();

            var extensionNames = metaModelService.GetViewExtensionNames()
                                    .ToList()
                                    .Where(extName => extName.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
                                    .ToList();

            if(extensionNames == null)
            {
                return null;
            }

            var currentModel = Common.CommonUtil.GetCurrentModel();

            foreach (var extName in extensionNames)
            {
                var extModels = metaModelService.GetViewExtensionModelInfo(extName).ToList();
                if (extModels != null)
                {
                    foreach (var model in extModels)
                    {
                        if (model.Module.Equals(currentModel.Module, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return metaModelService.GetViewExtension(extName);
                        }
                    }
                }
            }

            return null;
        }
    }
}
