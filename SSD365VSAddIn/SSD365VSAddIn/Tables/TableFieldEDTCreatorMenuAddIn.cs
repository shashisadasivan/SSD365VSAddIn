//using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Tables
{
    /// <summary>
    /// Creates EDT's for the table field
    /// </summary>
    [Export(typeof(IDesignerMenu))]
    // If you need to specify any other element, change this AutomationNodeType value.
    // You can specify multiple DesignerMenuExportMetadata attributes to meet your needs
    //[DesignerMenuExportMetadata(AutomationNodeType = typeof(IBaseField))] // once all the other types are created, then Change it to IBaseField
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFieldContainer))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFieldDate))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFieldEnum))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFieldGuid))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFieldInt))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFieldInt64))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFieldReal))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFieldString))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFieldTime))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IFieldUtcDateTime))]
    class TableFieldEDTCreatorMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.TableFieldEDTCreatorMenuAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.TableFieldEDTCreatorMenuAddIn;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return TableFieldEDTCreatorMenuAddIn.addinName;
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
                // we will create 2 security privileges for the menu item with the same name + Maintain,  +View
                var selectedElement = e.SelectedElement as IBaseField;
                if (selectedElement != null)
                {
                    this.CreateEDT(selectedElement);
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }

        #endregion

        protected void CreateEDT(IBaseField baseField)
        { 
            if(String.IsNullOrEmpty(baseField.ExtendedDataType) == false)
            {
                // There is already a EDT defined for this field
                return;
            }

            //  The Name of the edt to create should have the prefix as per the Settings
            //  If the Prefix already exists on the 
            var modelSettings = Settings.FetchSettings.FindOrCreateSettings();
            var edtName = baseField.Name;
            if(String.IsNullOrEmpty(modelSettings.Prefix) == false
                && edtName.StartsWith(modelSettings.Prefix, StringComparison.InvariantCultureIgnoreCase) == false)
            {
                edtName = modelSettings.Prefix + edtName;
            }
            Microsoft.Dynamics.AX.Metadata.MetaModel.AxEdt edtToCreate = null;

            if(baseField is IFieldContainer)
            {
                //var currentField = baseField as IFieldContainer;
                edtToCreate = new Microsoft.Dynamics.AX.Metadata.MetaModel.AxEdtContainer();
            }
            else if (baseField is IFieldDate)
            {
                edtToCreate = new Microsoft.Dynamics.AX.Metadata.MetaModel.AxEdtDate();
            }
            else if (baseField is IFieldEnum)
            {
                edtToCreate = new Microsoft.Dynamics.AX.Metadata.MetaModel.AxEdtEnum();
            }
            else if (baseField is IFieldGuid)
            {
                edtToCreate = new Microsoft.Dynamics.AX.Metadata.MetaModel.AxEdtGuid();
            }
            else if (baseField is IFieldInt)
            {
                edtToCreate = new Microsoft.Dynamics.AX.Metadata.MetaModel.AxEdtInt();
            }
            else if (baseField is IFieldReal)
            {
                edtToCreate = new Microsoft.Dynamics.AX.Metadata.MetaModel.AxEdtReal();
            }
            else if (baseField is IFieldString)
            {
                edtToCreate = new Microsoft.Dynamics.AX.Metadata.MetaModel.AxEdtString();
            }
            else if (baseField is IFieldTime)
            {
                edtToCreate = new Microsoft.Dynamics.AX.Metadata.MetaModel.AxEdtTime();
            }
            else if (baseField is IFieldUtcDateTime)
            {
                edtToCreate = new Microsoft.Dynamics.AX.Metadata.MetaModel.AxEdtUtcDateTime();
            }

            this.copyProperties(baseField, edtToCreate);
            edtToCreate.Name = edtName;

            if (edtToCreate != null)
            {
                // Find current model
                var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();

                //Create menu item in the right model
                //var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;
                //var metaModelService = metaModelProviders.CurrentMetaModelService;

                var metaModelService = Common.CommonUtil.GetModelSaveService();
                metaModelService.CreateExtendedDataType(edtToCreate, modelSaveInfo);

                //Update the table field with the EDT, also remove any labels from the field
                baseField.ExtendedDataType = edtToCreate.Name;
                baseField.Label = String.Empty;
                baseField.HelpText = String.Empty;

                // Add to the current active project
                Common.CommonUtil.AddElementToProject(edtToCreate);
            }
        }
        private void copyProperties(object source, object target)
        {
            var propertyMap = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("StringSizeDesignTime",  "StringSize")//,
                //new KeyValuePair<string, string>("SourcePropertyName",  "TargetPropertyName") -- add in new fields that doesnt match
            };
            foreach (var prop in source.GetType().GetProperties())
            {
                var targetProperty = target.GetType().GetProperty(prop.Name);
                
                if (targetProperty != null)
                {
                    targetProperty.SetValue(target, prop.GetValue(source));
                }
                else
                {
                    //if targetProperty not found with same name as Source Property, check if an alternative property exists from our map
                    var targetMappedProperty = propertyMap.FirstOrDefault(x => x.Key == prop.Name);
                    if(! String.IsNullOrEmpty(targetMappedProperty.Value))
                    {
                        targetProperty = target.GetType().GetProperty(targetMappedProperty.Value);
                        if (targetProperty.GetValue(target).GetType() == typeof(Int32))
                        {
                            var sourceValue = Int32.Parse(prop.GetValue(source).ToString());
                            targetProperty.SetValue(target, sourceValue);
                        }
                        else
                        {
                            targetProperty.SetValue(target, prop.GetValue(source));
                        }
                    }
                }
            }
            foreach (var fields in source.GetType().GetFields())
            {
                var targetField = target.GetType().GetField(fields.Name);
                if (targetField != null)
                {
                    targetField.SetValue(target, fields.GetValue(source));
                }
            }
        }
    }
}
