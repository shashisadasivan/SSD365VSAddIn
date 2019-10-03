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

            var classNameExt = metaModelService.GetClassNames().ToList()
                                            .Where(searchClass =>
                                             searchClass.ToLowerInvariant().Contains(className.ToLowerInvariant())
                                             &&
                                             metaModelService.GetClass(searchClass)
                                                                                .Declaration.ToLowerInvariant()
                                                                                .Contains(extensionOfStr.ToLowerInvariant())
                                            )
                                            .FirstOrDefault();
            if(String.IsNullOrEmpty(classNameExt) == false)
            {
                axClass = metaModelService.GetClass(classNameExt);
            }

            return axClass;
        }
    }
}
