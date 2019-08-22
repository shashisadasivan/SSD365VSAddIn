using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.BaseTypes;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.ShowTheLabel
{
    /// <summary>
    /// Show the label for the element
    /// this will show the label / help label for the element
    /// If none found then look at the extended data type
    /// </summary>
    [Export(typeof(IDesignerMenu))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IEdtBase))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables.BaseField))]
    //[DesignerMenuExportMetadata(AutomationNodeType = typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews.IDataEntityViewField))] //TODO: Enable labels for Data entity view fields
    class ShowTheLabelMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.ShowTheLabelMenuAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.ShowTheLabelMenuAddIn;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return addinName;
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
                var selectedItem = e.SelectedElement as INamedElement;
                if (selectedItem != null)
                {
                    //var metadataType = selectedItem.GetMetadataType();

                    EdtLabelInfo edtLabelInfo = new EdtLabelInfo();

                    if(selectedItem is IEdtBase)
                    {
                        var axEdt = Common.CommonUtil.GetModelSaveService().GetExtendedDataType(selectedItem.Name); 
                        edtLabelInfo = this.getEdtBaseLabel(axEdt, edtLabelInfo);
                    }       
                    else if(selectedItem is BaseField)
                    {
                        var axBaseField = selectedItem as BaseField;
                        edtLabelInfo = this.getTableFieldLabel(axBaseField, edtLabelInfo);
                    }
                    else if(selectedItem is IDataEntityViewField)
                    {
                        var axEntityField = selectedItem as IDataEntityViewField;
                        edtLabelInfo = this.getDataEntityFieldLabel(axEntityField, edtLabelInfo);
                    }


                    edtLabelInfo.DecypherLabels();
                    System.Windows.Forms.MessageBox.Show("Label: " + edtLabelInfo.Label + Environment.NewLine + "Help: " + edtLabelInfo.HelpLabel);
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }

        #endregion

        protected EdtLabelInfo getDataEntityFieldLabel(IDataEntityViewField tableField, EdtLabelInfo labelInfo)
        {

            if (String.IsNullOrEmpty(labelInfo.Label) == true
                && String.IsNullOrEmpty(tableField.Label) == false)
            {
                // find the label here
                labelInfo.Label = tableField.Label;
            }
            if (String.IsNullOrEmpty(labelInfo.HelpLabel) == true
                && String.IsNullOrEmpty(tableField.HelpText) == false)
            {
                // find the help label here
                labelInfo.HelpLabel = tableField.HelpText;
            }

            if(labelInfo.RequiresDigging()
                )
            {

            }
            //if (labelInfo.RequiresDigging()
            //    && String.IsNullOrEmpty(tableField.ExtendedDataType) == false)
            //{
            //    AxEdt axEdt = Common.CommonUtil.GetModelSaveService().GetExtendedDataType(tableField.ExtendedDataType);
            //    labelInfo = this.getEdtBaseLabel(axEdt, labelInfo);
            //}

            //if (labelInfo.RequiresDigging()
            //    && tableField is FieldEnum)
            //{
            //    FieldEnum tableFieldEnum = tableField as FieldEnum;

            //    var axEnum = Common.CommonUtil.GetModelSaveService().GetEnum(tableFieldEnum.EnumType);
            //    if (axEnum != null)
            //    {
            //        if (String.IsNullOrEmpty(labelInfo.Label) == true
            //                        && String.IsNullOrEmpty(axEnum.Label) == false)
            //        {
            //            // find the label here
            //            labelInfo.Label = axEnum.Label;
            //        }
            //        if (String.IsNullOrEmpty(labelInfo.HelpLabel) == true
            //            && String.IsNullOrEmpty(axEnum.HelpText) == false)
            //        {
            //            // find the help label here
            //            labelInfo.HelpLabel = axEnum.HelpText;
            //        }
            //    }
            //}

            return labelInfo;
        }

        protected EdtLabelInfo getTableFieldLabel(BaseField tableField, EdtLabelInfo labelInfo)
        {

            if (String.IsNullOrEmpty(labelInfo.Label) == true
                && String.IsNullOrEmpty(tableField.Label) == false)
            {
                // find the label here
                labelInfo.Label = tableField.Label;
            }
            if (String.IsNullOrEmpty(labelInfo.HelpLabel) == true
                && String.IsNullOrEmpty(tableField.HelpText) == false)
            {
                // find the help label here
                labelInfo.HelpLabel = tableField.HelpText;
            }

            if (labelInfo.RequiresDigging()
                && String.IsNullOrEmpty(tableField.ExtendedDataType) == false)
            {
                AxEdt axEdt = Common.CommonUtil.GetModelSaveService().GetExtendedDataType(tableField.ExtendedDataType);
                labelInfo = this.getEdtBaseLabel(axEdt, labelInfo);
            }

            if (labelInfo.RequiresDigging()
                && tableField is FieldEnum )
            {
                FieldEnum tableFieldEnum = tableField as FieldEnum;

                var axEnum = Common.CommonUtil.GetModelSaveService().GetEnum(tableFieldEnum.EnumType);
                if (axEnum != null)
                {
                    if (String.IsNullOrEmpty(labelInfo.Label) == true
                                    && String.IsNullOrEmpty(axEnum.Label) == false)
                    {
                        // find the label here
                        labelInfo.Label = axEnum.Label;
                    }
                    if (String.IsNullOrEmpty(labelInfo.HelpLabel) == true
                        && String.IsNullOrEmpty(axEnum.HelpText) == false)
                    {
                        // find the help label here
                        labelInfo.HelpLabel = axEnum.HelpText;
                    }
                }
            }

            return labelInfo;
        }

        protected EdtLabelInfo getEdtBaseLabel(AxEdt edtBase, EdtLabelInfo labelInfo)
        {
            if(String.IsNullOrEmpty(labelInfo.Label) == true 
                && String.IsNullOrEmpty(edtBase.Label) == false)
            {
                // find the label here
                labelInfo.Label = edtBase.Label;
            }
            if(String.IsNullOrEmpty(labelInfo.HelpLabel) == true 
                && String.IsNullOrEmpty(edtBase.HelpText) == false)
            {
                // find the help label here
                labelInfo.HelpLabel = edtBase.HelpText;
            }

            if(labelInfo.RequiresDigging() == true
                && (String.IsNullOrEmpty(edtBase.Extends) == false
                    || edtBase is AxEdtEnum)
                )
            {
                // if there is a extended data type to this then move it to the next one
                var edtExt = Common.CommonUtil.GetModelSaveService().GetExtendedDataType(edtBase.Extends);
                if (edtExt != null)
                {
                    labelInfo = this.getEdtBaseLabel(edtExt, labelInfo);
                }
                else if (edtExt == null)
                {
                    
                    // if this is null then chances are that this extends an enum
                    if(edtBase is AxEdtEnum)
                    {
                        AxEdtEnum edtBaseEnum = edtBase as AxEdtEnum;
                        var axEnum = Common.CommonUtil.GetModelSaveService().GetEnum(edtBaseEnum.EnumType);
                        if (axEnum != null)
                        {
                            if (String.IsNullOrEmpty(labelInfo.Label) == true
                                            && String.IsNullOrEmpty(axEnum.Label) == false)
                            {
                                // find the label here
                                labelInfo.Label = axEnum.Label;
                            }
                            if (String.IsNullOrEmpty(labelInfo.HelpLabel) == true
                                && String.IsNullOrEmpty(axEnum.HelpText) == false)
                            {
                                // find the help label here
                                labelInfo.HelpLabel = axEnum.HelpText;
                            }
                        }
                    }
                }
            }

            return labelInfo;
        }



        public class EdtLabelInfo
        {
            public string Label { get; set; }
            public string HelpLabel { get; set; }

            public bool RequiresDigging()
            {
                if(String.IsNullOrEmpty(this.Label)
                    || String.IsNullOrEmpty(HelpLabel))
                {
                    return true;
                }
                return false;
            }

            public void DecypherLabels()
            {
                if (String.IsNullOrEmpty(this.Label))
                    this.Label = string.Empty;
                if (String.IsNullOrEmpty(this.HelpLabel))
                    this.HelpLabel = string.Empty;

                if (this.Label.StartsWith("@"))
                {
                    var labelContent = Labels.LabelHelper.FindLabelGlobally(this.Label);
                    if (String.IsNullOrEmpty(labelContent.LabelText) == false)
                    {
                        this.Label = labelContent.LabelText;
                    }
                    
                }

                if (this.HelpLabel.StartsWith("@"))
                {
                    var labelContent = Labels.LabelHelper.FindLabelGlobally(this.HelpLabel);
                    if (String.IsNullOrEmpty(labelContent.LabelText) == false)
                    {
                        this.HelpLabel = labelContent.LabelText;
                    }

                }
            }
        }

    }


}
