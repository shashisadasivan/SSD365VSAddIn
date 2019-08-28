using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Classes;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
//using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Labels
{
    [Export(typeof(IDesignerMenu))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IClassItem))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IDataEntity))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ITable))]
    class LabelCodeCreatedMainMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.LabelCodeCreatedMainMenuAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.LabelCodeCreatedMainMenuAddIn;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return LabelCodeCreatedMainMenuAddIn.addinName;
            }
        }
        #endregion

        #region Callbacks
        /// <summary>
        /// Called when user clicks on the add-in menu
        /// </summary>
        /// <param name="e">The context of the VS tools and metadata</param>
        public override void OnClick(AddinDesignerEventArgs e)
        {
            //Microsoft.Dynamics.AX.Metadata.Core.MetaModel.EntryPointType entryPointType;
            try
            {
                var selectedItem = e.SelectedElement as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.IRootElement;
                if (selectedItem != null)
                {
                    var metadataType = selectedItem.GetMetadataType();

                    //LabelFactory labelFactory = LabelFactory.construct(selectedItem);
                    //labelFactory.ApplyLabel();

                    this.CreateLabelInCode(e.SelectedElement as IRootElement);
                }
            }
            catch (Exception ex)
            {
                Microsoft.Dynamics.Framework.Tools.MetaModel.Core.CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }

        #endregion

        public void CreateLabelInCode(IRootElement selectedElement)
        {
            int totalLabelsConverted = 0;
            if(selectedElement is ITable)
            {
                var axTable = Common.CommonUtil.GetModelSaveService().GetTable(selectedElement.Name);
                axTable.Methods.ToList().ForEach(method =>
                {
                    int labelsConverted = 0;
                    method.Source = this.convertToLabelInSourceCode(method.Source, out labelsConverted);
                    totalLabelsConverted += labelsConverted;
                });
                Common.CommonUtil.GetModelSaveService().UpdateTable(axTable, Common.CommonUtil.GetCurrentModelSaveInfo());
            }
            else if( selectedElement is IDataEntity)
            {
                var axDataEntity = Common.CommonUtil.GetModelSaveService().GetDataEntityView(selectedElement.Name);
                axDataEntity.Methods.ToList().ForEach(method =>
                {
                    int labelsConverted = 0;
                    method.Source = this.convertToLabelInSourceCode(method.Source, out labelsConverted);
                    totalLabelsConverted += labelsConverted;
                });
                Common.CommonUtil.GetModelSaveService().UpdateDataEntityView(axDataEntity, Common.CommonUtil.GetCurrentModelSaveInfo());
            }
            else if(selectedElement is IClassItem)
            {
                var axClass = Common.CommonUtil.GetModelSaveService().GetClass(selectedElement.Name);
                axClass.Methods.ToList().ForEach(method =>
                {
                    int labelsConverted = 0;
                    method.Source = this.convertToLabelInSourceCode(method.Source, out labelsConverted);
                    totalLabelsConverted += labelsConverted;
                });
                Common.CommonUtil.GetModelSaveService().UpdateClass(axClass, Common.CommonUtil.GetCurrentModelSaveInfo());
            }
        }

        public string convertToLabelInSourceCode(string sourceCode, out int labelsConverted)
        {
            labelsConverted = 0;
            string result = sourceCode;

            // we need to find every alternate "
            int currentQuoteIdx = 0;
            while(currentQuoteIdx >= 0 
                && currentQuoteIdx < result.Length)
            {
                currentQuoteIdx = result.IndexOf("\"", currentQuoteIdx);
                if(currentQuoteIdx >= 0)
                {
                    int nextQuoteIdx = result.IndexOf("\"", currentQuoteIdx + 1);
                    string labelValue = result.Substring(currentQuoteIdx + 1, (nextQuoteIdx - currentQuoteIdx - 1));
                    if(labelValue.StartsWith("@") == false)
                    {
                        // this is a string value we need to convert into a label
                        var labelId = this.GetLabel(labelValue);
                        labelsConverted++;
                        // replace the labelValue with this labelId
                        // 1. Remove the text
                        result = result.Remove(currentQuoteIdx + 1, (nextQuoteIdx - currentQuoteIdx - 1));
                        // 2. insert the label here instead
                        result = result.Insert(currentQuoteIdx + 1, labelId);
                        // now the position of the next Quoteid would hav changed so move it to that position
                        nextQuoteIdx = currentQuoteIdx + labelId.Length + 1;
                    }

                    currentQuoteIdx = nextQuoteIdx + 1;
                }
            }

            return result;
        }

        public string GetLabel(string label)
        {
            string labelId = LabelHelper.FindOrCreateLabel(label);
            if (labelId.Equals(label) == false)
                return labelId;

            return label;
        }
    }
}
