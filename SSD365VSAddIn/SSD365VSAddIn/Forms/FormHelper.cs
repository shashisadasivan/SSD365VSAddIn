using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Forms
{
    class FormHelper
    {
        public static string CreateExtension(IForm form)
        {
            var name = form.Name + Common.Constants.DotEXTENSION;
            name = Common.CommonUtil.GetNextFormExtension(name);

            // Find current model
            //Create menu item in the right model
            var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;


            //Create an extension object
            var axExtension = new AxFormExtension() { Name = name };
            //var tableExts = metaModelProviders.CurrentMetadataProvider.TableExtensions.Common.CommonUtil.GetCurrentModel().Name);

            Common.CommonUtil.GetMetaModelProviders()
                .CurrentMetadataProvider
                .FormExtensions
                .Create(axExtension, Common.CommonUtil.GetCurrentModelSaveInfo());

            // Add to project
            Common.CommonUtil.AddElementToProject(axExtension);

            return name;
        }

        //public static AxFormExtension GetFirstExtension(string name)
        //{
        //    // Find current model
        //    var metaModelService = Common.CommonUtil.GetModelSaveService();

        //    var extensionName = metaModelService
        //                              No method to get all form extensions :(
        //                            .ToList()
        //                            .Where(extName => extName.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
        //                            .FirstOrDefault();

        //    if (String.IsNullOrEmpty(extensionName) == false)
        //    {
        //        var extension = metaModelService.GetTableExtension(extensionName);
        //        return extension;
        //    }

        //    return null;
        //}
    }
}
