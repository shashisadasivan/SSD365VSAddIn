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
            AxFormExtension axExtension = FormHelper.GetFirstExtension(form.Name);
            if (axExtension != null)
            {
                // Add existing extension to project & quit
                Common.CommonUtil.AddElementToProject(axExtension);
                return axExtension.Name;
            }
            var name = form.Name;// + Common.Constants.DotEXTENSION;
            name = Common.CommonUtil.GetNextFormExtension(name);

            // Find current model
            //Create menu item in the right model
            var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;


            //Create an extension object
            axExtension = new AxFormExtension() { Name = name };
            //var tableExts = metaModelProviders.CurrentMetadataProvider.TableExtensions.Common.CommonUtil.GetCurrentModel().Name);

            Common.CommonUtil.GetMetaModelProviders()
                .CurrentMetadataProvider
                .FormExtensions
                .Create(axExtension, Common.CommonUtil.GetCurrentModelSaveInfo());

            // Add to project
            Common.CommonUtil.AddElementToProject(axExtension);

            return name;
        }

        public static AxFormExtension GetFirstExtension(string name)
        {
            //TODO: There is no GetFormExtensions() method, so will need to get an alternative way of doing this.
            // Find current model
            var metaModelService = Common.CommonUtil.GetModelSaveService();
            var formNames = metaModelService.GetFormNames().ToList()
                                .Where(frmName => frmName.ToLowerInvariant().Contains(name.ToLower())).ToList();
            //if(String.IsNullOrEmpty(formName) == false)
            if(formNames != null)
            {
                foreach (var formName in formNames)
                {
                    var form = metaModelService.GetFormExtension(formName);
                    if (form != null)
                    {
                        return form;
                    }
                }
            }
            return null;
        }
        //public static AxFormExtension GetFirstExtension(string name)
        //{
        //    // Find current model
        //    var metaModelService = Common.CommonUtil.GetModelSaveService();

        //    var extensionName = metaModelService
        //                              No method to get all form extensions: (
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
