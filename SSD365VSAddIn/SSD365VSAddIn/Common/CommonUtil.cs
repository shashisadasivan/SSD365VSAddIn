using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnvDTE;
using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.AX.Metadata.Service;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using Microsoft.Dynamics.Framework.Tools.ProjectSystem;

namespace SSD365VSAddIn.Common
{
    /// <summary>
    /// Common utilities for the Add ins
    /// </summary>
    internal static class CommonUtil
    {
        /// <summary>
        /// Get teh DTE object
        /// </summary>
        internal static DTE DTE => CoreUtility.ServiceProvider.GetService(typeof(DTE)) as DTE;
        
        /// <summary>
        /// Current VS project
        /// </summary>
        /// <returns></returns>
        internal static VSProjectNode GetCurrentProject()
        {
            Array projects = CommonUtil.DTE.ActiveSolutionProjects as Array;

            if (projects?.Length > 0)
            {
                Project project = projects.GetValue(0) as Project;
                return project?.Object as VSProjectNode;
            }
            return null;
        }

        /// <summary>
        /// Current VS Application Context
        /// </summary>
        /// <returns>VSApplicationContext</returns>
        internal static Microsoft.Dynamics.Framework.Tools.Labels.VSApplicationContext GetVSApplicationContext()
        {
            var context = new Microsoft.Dynamics.Framework.Tools.Labels.VSApplicationContext(CommonUtil.DTE.DTE);
            return context;
        }

        /// <summary>
        /// Current model
        /// </summary>
        /// <returns>ModelInfo</returns>
        internal static ModelInfo GetCurrentModel()
        {
            var project = CommonUtil.GetCurrentProject();

            ModelInfo model = project.GetProjectsModelInfo();

            return model;
            
        }

        /// <summary>
        /// Get model save info, used when creating an element
        /// </summary>
        /// <returns>save info</returns>
        internal static ModelSaveInfo GetCurrentModelSaveInfo()
        {
            var model = CommonUtil.GetCurrentModel();
            // Prepare information needed for saving
            ModelSaveInfo saveInfo = new ModelSaveInfo
            {
                Id = model.Id,
                Layer = model.Layer
            };

            return saveInfo;
        }
        /// <summary>
        /// Add element to the current project
        /// </summary>
        /// <param name="modelElement">Element</param>
        internal static void AddElementToProject(Microsoft.Dynamics.AX.Metadata.Core.MetaModel.INamedObject modelElement)
        {
            var vsProject = CommonUtil.GetCurrentProject();
            if(vsProject != null)
            {
                vsProject.AddModelElementsToProject(new List<MetadataReference>()
                {
                    new MetadataReference(modelElement.Name, modelElement.GetType())
                });
            }
        }


        /// <summary>
        /// Display any logs to the user
        /// This shows as Modal dialogs, so keep bunch all messages into 1 message if possible
        /// </summary>
        /// <param name="message">message to display in modal form</param>
        internal static void ShowLog(string message)
        {
            CoreUtility.DisplayInfo(message);
        }

        internal static IMetaModelProviders GetMetaModelProviders()
        {
            var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;

            return metaModelProviders;
        }


        internal static IMetaModelService GetModelSaveService()
        {
            // Find current model
            //var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();

            // Get the model servivce to save the element in //Create menu item in the right model
            var metaModelService = CommonUtil.GetMetaModelProviders().CurrentMetaModelService;

            return metaModelService;
        }

        private static string GetCodeExtensionName(string name, int number)
        {
            string className = name;
            var modelSettings = Settings.FetchSettings.FindOrCreateSettings();
            if (number > 0)
            {
                className = modelSettings.Prefix + name + modelSettings.Suffix + number.ToString() + Constants._EXTENSION;
            }
            else
            {
                className = modelSettings.Prefix + name + modelSettings.Suffix + Constants._EXTENSION;
            }
            return className;
        }
        private static string GetObjectExtensionName(string name, int number)
        {
            string extName = name;
            var modelSettings = Settings.FetchSettings.FindOrCreateSettings();
            //extName = modelSettings.Prefix + name + modelSettings.Suffix + Constants.DOT + modelSettings.Extension;// + numFound.ToString();
            extName = name + Constants.DOT + modelSettings.Extension;// + numFound.ToString();
            if (number > 0)
            {
                extName += number.ToString();
            }

            return extName;
        }

        internal static string GetNextNameSecurityDutyExtension(string name)
        {
            string objectName = name;

            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();
            var metaModelService = Common.CommonUtil.GetModelSaveService();

            // Create a class with the same name + _Extension and add it to the project
            // ClassName
            int numFound = 0;
            while (metaModelService.GetSecurityDutyExtension(objectName) != null)
            {
                numFound++;
                objectName = name + numFound.ToString();
            }

            return objectName;
        }

        internal static string GetNextTableExtension(string name)
        {
            var modelSettings = Settings.FetchSettings.FindOrCreateSettings();
            string objectName = GetObjectExtensionName(name, 0);

            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();
            var metaModelService = Common.CommonUtil.GetModelSaveService();

            // Create a class with the same name + _Extension and add it to the project
            // ClassName
            int numFound = 0;
            while (metaModelService.GetTableExtension(objectName) != null)
            {
                numFound++;
                objectName = GetObjectExtensionName(name, numFound);
                //objectName = modelSettings.Prefix + name + modelSettings.Suffix + Constants.DOT + modelSettings.Extension + numFound.ToString();
            }

            return objectName;
        }

        internal static string GetNextBaseEnumExtension(string name)
        {
            var modelSettings = Settings.FetchSettings.FindOrCreateSettings();
            string objectName = GetObjectExtensionName(name, 0);

            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();
            var metaModelService = Common.CommonUtil.GetModelSaveService();

            // Create a class with the same name + _Extension and add it to the project
            // ClassName
            int numFound = 0;
            while (metaModelService.GetEnumExtension(objectName) != null)
            {
                numFound++;
                objectName = GetObjectExtensionName(name, numFound);
                //objectName = modelSettings.Prefix + name + modelSettings.Suffix + Constants.DOT + modelSettings.Extension + numFound.ToString();
            }

            return objectName;
        }

        internal static string GetNextFormExtension(string name)
        {
            var modelSettings = Settings.FetchSettings.FindOrCreateSettings();
            string objectName = GetObjectExtensionName(name, 0);

            // Find current model
            var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();
            var metaModelService = Common.CommonUtil.GetModelSaveService();

            // Create a class with the same name + _Extension and add it to the project
            // ClassName
            int numFound = 0;
            while (metaModelService.GetFormExtension(objectName) != null)
            {
                numFound++;
                objectName = GetObjectExtensionName(name, 0);
                //objectName = modelSettings.Prefix + name + modelSettings.Suffix + Constants.DOT + modelSettings.Extension + numFound.ToString();
            }

            return objectName;
        }

        internal static string GetNextClassExtensionName(string name)
        {
            var modelSettings = Settings.FetchSettings.FindOrCreateSettings();
            var metaModelService = Common.CommonUtil.GetModelSaveService();

            string className = GetCodeExtensionName(name, 0);
            int numClassesFound = 0;
            while (metaModelService.GetClass(className) != null)
            {
                numClassesFound++;
                className = GetCodeExtensionName(name, numClassesFound);
                //className = modelSettings.Prefix + name + modelSettings.Suffix + numClassesFound.ToString() + Constants._EXTENSION;
            }

            return className;
        }
    }
}
