using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Labels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Labels
{
    /// <summary>
    /// Helper class to create labels
    /// Most of the code has been adapted from https://stoneridgesoftware.com/learn-to-use-a-label-creator-add-in-extension-in-dynamics-365-for-finance-operations/
    /// </summary>
    class LabelHelper
    {
        public static IList<AxLabelFile> GetLabelFiles()
        {
            List<AxLabelFile> labelFilesToUpdate = new List<AxLabelFile>();
            // Choose the current model
            var modelInfo = Common.CommonUtil.GetCurrentModel();

            // Get the list of label files from that model
            var metaModelProvider = Common.CommonUtil.GetModelSaveService();

            var metaModelProviders = Microsoft.Dynamics.Framework.Tools.MetaModel.Core.ServiceLocator.GetService(typeof(Microsoft.Dynamics.Framework.Tools.Extensibility.IMetaModelProviders)) as Microsoft.Dynamics.Framework.Tools.Extensibility.IMetaModelProviders;
            var metaModelService = metaModelProviders.CurrentMetaModelService;

            IList<string> labelFiles = metaModelProviders.CurrentMetadataProvider.LabelFiles.ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name);

            var labelFileName = labelFiles.First();
            if(labelFileName != null)
            {
                var labelFile = metaModelProvider.GetLabelFile(labelFileName);
                //LabelControllerFactory factory = new LabelControllerFactory();
                //LabelEditorController labelController = factory.GetLabelController(labelFile.LabelContentFileName); //factory.GetOrCreateLabelController(labelFile);
                // we want to get a list of labels with the same labelid (all languages)
                foreach (var labelFileNameToCheck in labelFiles)
                {
                    if (labelFileNameToCheck.Equals(labelFileName))
                    {
                        labelFilesToUpdate.Add(labelFile);
                        //labelFile = metaModelProvider.GetLabelFile(labelFileNameToCheck);
                    }
                    else
                    {
                        var labelFileToCheck = metaModelProvider.GetLabelFile(labelFileNameToCheck);
                        if(labelFileToCheck.LabelFileId.Equals(labelFile.LabelFileId))
                        {
                            labelFilesToUpdate.Add(labelFileToCheck);
                        }
                        
                    }
                }
            }
            return labelFilesToUpdate;
            //IList<String> labelFileNames = metaModelProvider.GetLabelFileNames();
            // Choose the first
            // What happens if there is no label file?
            //var labelFile = metaModelProvider.GetLabelFile(labelFileNames[0]);

            //return labelFile;
        }

        public static string FindOrCreateLabel(string labelText)
        {
            string newLabelId = String.Empty;

            //Is the labeltext Already a label?
            if(labelText.StartsWith("@"))
            {
                return labelText;
            }

            // Don't bother if the string is empty
            if (String.IsNullOrEmpty(labelText) == false)
            {
                // Construct a label id that will be unique
                //string labelId = $"{elementName}{propertyName}";
                // CamelCase this
                
                //string labelId = labelText.Replace(" ", "") // remove spaces
                //                           .Replace("%", "P") // Replace %1 with P1 or %2 with P2 etc
                //                           .Replace("_", ""); // Underscores to be removed

                string labelId = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(labelText)
                                    .Replace(" ", "") // remove spaces
                                    .Replace("%", "P") // Replace %1 with P1 or %2 with P2 etc
                                    .Replace("_", "");
                // Get the label factory
                LabelControllerFactory factory = new LabelControllerFactory();

                // Get the label edit controller
                var labelFiles = LabelHelper.GetLabelFiles();

                var labelFile = labelFiles.First();

                LabelEditorController labelController = factory.GetOrCreateLabelController(labelFile, Common.CommonUtil.GetVSApplicationContext());
                labelController.LabelSearchOption = SearchOption.MatchExactly;
                labelController.IsMatchExactly = true;

                // Make sure the label doesn't exist.
                // What will you do if it does?
                var labelsx = labelController.Labels.ToList();
                if (labelController.Exists(labelId) == false
                    && labelController.Exists(labelText) == false) // the label text may be an Id by itself
                {
                    labelController.Insert(labelId, labelText, String.Empty);
                    labelController.Save();

                    // Construct a label reference to go into the label property
                    // newLabelId = $"@{labelFile.LabelFileId}:{labelId}";

                    //Create this label in the other languages as well.
                    foreach (var labelFileToAdd in labelFiles)
                    {
                        if (labelFileToAdd.LabelFileId == labelFile.LabelFileId
                            && !labelFileToAdd.Language.Equals(labelFile.Language))
                        {
                            LabelEditorController labelControllerToAdd = factory.GetOrCreateLabelController(labelFileToAdd, Common.CommonUtil.GetVSApplicationContext());
                            labelControllerToAdd.Insert(labelId, labelText, String.Empty);
                            labelControllerToAdd.Save();
                        }
                    }
                }
                newLabelId = $"@{labelFile.LabelFileId}:{labelId}";
            }

            return newLabelId;
        }
    }
}
