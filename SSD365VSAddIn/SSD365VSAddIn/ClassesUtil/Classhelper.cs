using Microsoft.Dynamics.AX.Metadata.MetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.ClassesUtil
{
    class ClassHelper
    {
        public static AxClass GetExistingExtensionClass(string className, string extensionOfStr)
        {
            AxClass axClass = null;

            var metaModelService = Common.CommonUtil.GetModelSaveService();
            //TODO: only search within model
            var classNameExts = metaModelService.GetClassNames().ToList()
                                            .Where(searchClass => searchClass.ToLowerInvariant().Contains(className.ToLowerInvariant())
                                             && metaModelService.GetClass(searchClass).Declaration.ToLowerInvariant().Contains(extensionOfStr.ToLowerInvariant())
                                            )
                                            .ToList();

            string classNameExt = String.Empty;
            var currentModel = Common.CommonUtil.GetCurrentModel();
            foreach (var classNameE in classNameExts)
            {
                // Determine if the model this class belongs to is a part of the current project model
                var modelInfo = metaModelService.GetClassModelInfo(classNameE).ToList()
                    .Where(modelInfoCur => modelInfoCur.Module.Equals(currentModel.Module, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
                if(modelInfo != null)
                {
                    classNameExt = classNameE;
                    break;
                }
            }

            if (String.IsNullOrEmpty(classNameExt) == false)
            {
                axClass = metaModelService.GetClass(classNameExt);
            }

            return axClass;
        }
    }
}
