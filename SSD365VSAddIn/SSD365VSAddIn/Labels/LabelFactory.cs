using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.BaseTypes;
//using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSD365VSAddIn.FrameworkExtentions;

namespace SSD365VSAddIn.Labels
{
    public abstract class LabelFactory
    {
        IRootElement iRootElement;

        public static LabelFactory construct(IRootElement selectedElement)
        {
            LabelFactory labelFactory = null;

            if (selectedElement is ITable)
            {
                labelFactory = new LabelFactory_Table();
            }
            else if (selectedElement is ITableExtension)
            {
                labelFactory = new LabelFactory_TableExtension();
            }
            else if (selectedElement is IDataEntity)
            {
                labelFactory = new LabelFactory_DataEntity();
            }
            else if (selectedElement is Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews.IDataEntityViewExtension)
            {
                labelFactory = new LabelFactory_DataEntityExtension();
            }
            else if (selectedElement is IMenuItem || selectedElement is IMenuItemExtension)
            {
                labelFactory = new LabelFactory_IMenuItem();
            }
            else if (selectedElement is IForm)
            {
                labelFactory = new LabelFactory_Form();
            }
            else if (selectedElement is ISecurityDuty)
            {
                labelFactory = new LabelFactory_ISecurityDuty();
            }
            else if (selectedElement is ISecurityPrivilege)
            {
                labelFactory = new LabelFactory_ISecurityPrivilege();
            }
            else if (selectedElement is ISecurityRole)
            {
                labelFactory = new LabelFactory_ISecurityRole();
            }
            else if (selectedElement is IEdtBase)
            {
                labelFactory = new LabelFactory_IEdtBase();
            }
            else if (selectedElement is IBaseEnum)
            {
                labelFactory = new LabelFactory_IBaseEnum();
            }
            else if (selectedElement is IBaseEnumExtension)
            {
                labelFactory = new LabelFactory_IBaseEnumExtension();
            }
            else if (selectedElement is IConfigurationKey)
            {
                labelFactory = new LabelFactory_IConfigurationKey();
            }
            // add additional elseifs here
            else
            {
                throw new Exception($"Type {selectedElement.GetMetadataType().Name} not implemented yet");
            }

            labelFactory.setElement(selectedElement);

            return labelFactory;
        }

        public void setElement(IRootElement selectedElement)
        {
            this.iRootElement = selectedElement;
            this.setElementType(selectedElement);
        }

        public abstract void setElementType(IRootElement selectedElement);

        public abstract void ApplyLabel();

        public string GetLabel(string label)
        {
            string labelId = LabelHelper.FindOrCreateLabel(label);
            if (labelId.Equals(label) == false)
                return labelId;

            return label;
        }
    }

    public class LabelFactory_IConfigurationKey : LabelFactory
    {
        IConfigurationKey iConfigKey;

        public override void ApplyLabel()
        {
            var configKeyexists = Common.CommonUtil.GetMetaModelProviders()
                                        .CurrentMetadataProvider
                                        .ConfigurationKeys
                                        .ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                        .Where(t => t.Equals(this.iConfigKey.Name))
                                        .FirstOrDefault();
            if (String.IsNullOrEmpty(configKeyexists) == false)
            {
                this.iConfigKey.Label = this.GetLabel(this.iConfigKey.Label);
                this.iConfigKey.Description = this.GetLabel(this.iConfigKey.Description);
            }
        }

        public override void setElementType(IRootElement selectedElement)
        {
            iConfigKey = selectedElement as IConfigurationKey;
        }
    }

    public class LabelFactory_Table : LabelFactory
    {
        ITable iTable;

        public override void setElementType(IRootElement selectedElement)
        {
            this.iTable = selectedElement as ITable;
        }

        public override void ApplyLabel()
        {
            //check if table is in current model
            var tableExists = Common.CommonUtil.GetMetaModelProviders()
                                .CurrentMetadataProvider
                                .Tables.ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                .Where(t => t.Equals(this.iTable.Name))
                                .FirstOrDefault();

            if (string.IsNullOrEmpty(tableExists) == false)
            {
                this.iTable.Label = this.GetLabel(this.iTable.Label);
                this.iTable.DeveloperDocumentation = this.GetLabel(this.iTable.DeveloperDocumentation);

                // Apply label on fields
                var enumerator = this.iTable.BaseFields.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var baseField = enumerator.Current as IBaseField;

                    baseField.Label = this.GetLabel(baseField.Label);
                    baseField.HelpText = this.GetLabel(baseField.HelpText);
                }

                // Apply label for groups
                var fieldGrpEnumerator = this.iTable.FieldGroups.GetEnumerator();
                while (fieldGrpEnumerator.MoveNext())
                {
                    var fieldGroup = fieldGrpEnumerator.Current as IFieldGroup;
                    fieldGroup.Label = this.GetLabel(fieldGroup.Label);
                }
            }
            
        }
    }

    public class LabelFactory_TableExtension : LabelFactory
    {
        private ITableExtension iTableExtension;
        public override void ApplyLabel()
        {
            var tableExists = Common.CommonUtil.GetMetaModelProviders()
                                .CurrentMetadataProvider
                                .TableExtensions
                                .ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                .Where(t => t.Equals(this.iTableExtension.Name))
                                .FirstOrDefault();
            if (String.IsNullOrEmpty(tableExists) == false)
            {
                // this extension is in the current model
                // get the fields for this extension table
                var enumerator = iTableExtension.BaseFields.VisualChildren.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    var baseField = enumerator.Current as IBaseField;
                    baseField.Label = this.GetLabel(baseField.Label);
                    baseField.HelpText = this.GetLabel(baseField.HelpText);
                   
                }

                //Apply label for fieldGroups
                var fieldGrpEnumerator = this.iTableExtension.FieldGroups.GetEnumerator();
                while (fieldGrpEnumerator.MoveNext())
                {
                    var fieldGroup = fieldGrpEnumerator.Current as IFieldGroup;
                    fieldGroup.Label = this.GetLabel(fieldGroup.Label);
                }
            }
        }

        public override void setElementType(IRootElement selectedElement)
        {
            this.iTableExtension = selectedElement as ITableExtension;
        }
    }

    public class LabelFactory_IMenuItem : LabelFactory
    {
        private IMenuItem iMenuItem;
        //private IMenuItemExtension iMenuItemExtension;

        public override void ApplyLabel()
        {
            string menuItemExists = String.Empty;

            // Check if this menu item / extension exists in the current model
            if (this.iMenuItem != null)
            {
                if (this.iMenuItem is IMenuItemAction)
                {
                    menuItemExists = Common.CommonUtil.GetMetaModelProviders()
                                           .CurrentMetadataProvider
                                           .MenuItemActions
                                           .ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                           .Where(t => t.Equals(this.iMenuItem.Name))
                                           .FirstOrDefault();
                }
                else if (this.iMenuItem is IMenuItemDisplay)
                {
                    menuItemExists = Common.CommonUtil.GetMetaModelProviders()
                                           .CurrentMetadataProvider
                                           .MenuItemDisplays
                                           .ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                           .Where(t => t.Equals(this.iMenuItem.Name))
                                           .FirstOrDefault();
                }
                else if (this.iMenuItem is IMenuItemOutput)
                {
                    menuItemExists = Common.CommonUtil.GetMetaModelProviders()
                                           .CurrentMetadataProvider
                                           .MenuItemOutputs
                                           .ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                           .Where(t => t.Equals(this.iMenuItem.Name))
                                           .FirstOrDefault();
                }
            }
            /*
            else if(iMenuItemExtension != null)
            {
                if (this.iMenuItemExtension is IMenuItemActionExtension)
                {
                    menuItemExists = Common.CommonUtil.GetMetaModelProviders()
                                           .CurrentMetadataProvider
                                           .MenuItemActionExtensions
                                           .ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                           .Where(t => t.Equals(this.iMenuItem.Name))
                                           .FirstOrDefault();
                }
                else if (this.iMenuItemExtension is IMenuItemDisplayExtension)
                {
                    menuItemExists = Common.CommonUtil.GetMetaModelProviders()
                                           .CurrentMetadataProvider
                                           .MenuItemDisplayExtensions
                                           .ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                           .Where(t => t.Equals(this.iMenuItem.Name))
                                           .FirstOrDefault();
                }
                else if (this.iMenuItemExtension is IMenuItemOutputExtension)
                {
                    menuItemExists = Common.CommonUtil.GetMetaModelProviders()
                                           .CurrentMetadataProvider
                                           .MenuItemOutputExtensions
                                           .ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                           .Where(t => t.Equals(this.iMenuItem.Name))
                                           .FirstOrDefault();
                }
            }*/

            string label = String.Empty;
            if (String.IsNullOrEmpty(menuItemExists) == false)
            {
                if(this.iMenuItem != null)
                {
                    this.iMenuItem.Label = this.GetLabel(this.iMenuItem.Label);
                    this.iMenuItem.HelpText = this.GetLabel(this.iMenuItem.HelpText);
                }
            }
        }

        public override void setElementType(IRootElement selectedElement)
        {
            if (selectedElement is IMenuItem)
                this.iMenuItem = selectedElement as IMenuItem;
            //else if (selectedElement is IMenuItemExtension)
            //    this.iMenuItemExtension = selectedElement as IMenuItemExtension;
        }
    }

    public class LabelFactory_Form : LabelFactory
    {
        IForm iForm;

        public override void setElementType(IRootElement selectedElement)
        {
            this.iForm = selectedElement as IForm;
        }

        public override void ApplyLabel()
        {
            //check if table is in current model
            var tableExists = Common.CommonUtil.GetMetaModelProviders()
                                .CurrentMetadataProvider
                                .Tables.ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                .Where(t => t.Equals(this.iForm.Name))
                                .FirstOrDefault();

            if (string.IsNullOrEmpty(tableExists) == false)
            {
                this.iForm.FormDesign.Caption = this.GetLabel(this.iForm.FormDesign.Caption);

                this.RunEnumerator(this.iForm.FormDesign.VisualChildren);
            }

        }

        private void RunEnumerator(System.Collections.IEnumerable formControls)
        {
            foreach (var item in formControls)
            {
                var formControl = item as IFormControl;
                if(formControl != null)
                {
                    formControl.HelpText = this.GetLabel(formControl.HelpText); // most elements has a help text
                    string textValue = formControl.GetPropertyValueSS<String>("Text");
                    if(!String.IsNullOrEmpty(textValue))
                    {
                        formControl.SetPropertyValueSS("Text", this.GetLabel(textValue));
                    }

                    string labelValue = formControl.GetPropertyValueSS<String>("Label");
                    if (!String.IsNullOrEmpty(labelValue))
                    {
                        formControl.SetPropertyValueSS("Label", this.GetLabel(labelValue));
                    }

                    //string helpTextValue = formControl.GetPropertyValueSS<String>("HelpText");
                    //if (!String.IsNullOrEmpty(labelValue))
                    //{
                    //    formControl.SetPropertyValueSS("HelpText", this.GetLabel(helpTextValue));
                    //}

                    string captionValue = formControl.GetPropertyValueSS<String>("Caption");
                    if (!String.IsNullOrEmpty(captionValue))
                    {
                        formControl.SetPropertyValueSS("Caption", this.GetLabel(captionValue));
                    }

                    string exportValue = formControl.GetPropertyValueSS<String>("ExportLabel");
                    if (!String.IsNullOrEmpty(exportValue))
                    {
                        formControl.SetPropertyValueSS("ExportLabel", this.GetLabel(exportValue));
                    }

                    if (formControl.FormControlExtension != null && formControl.FormControlExtension.ExtensionProperties != null)
                    {
                        var extprops = formControl.FormControlExtension.ExtensionProperties.GetEnumerator();
                        while (extprops.MoveNext())
                        {
                            var extProperty = extprops.Current as ExtensionProperty;

                            if (extProperty.Name.Equals("PlaceholderText", StringComparison.InvariantCultureIgnoreCase))
                            {
                                extProperty.Value = this.GetLabel(extProperty.Value);
                                //formControl.SetPropertyValueSS("PlaceholderText", this.GetLabel(placeholderTextValue));
                            }
                            else if (extProperty.Name.Equals("ExportLabel", StringComparison.InvariantCultureIgnoreCase))
                            {
                                extProperty.Value = this.GetLabel(extProperty.Value); ;
                            }
                        }
                    }

                    // If the control has children (grid, groups, etc) then run through them as well
                    if (formControl is IFormControlWithChildren)
                    {
                        var formControlWithChildren = formControl as IFormControlWithChildren;
                        if (formControlWithChildren != null && formControlWithChildren.FormControls != null)
                        {
                            this.RunEnumerator(formControlWithChildren.FormControls);
                        }
                    }

                }
            }
        }
    }

    public class LabelFactory_ISecurityDuty : LabelFactory
    {
        private ISecurityDuty iSecurityDuty;
        //private IMenuItemExtension iMenuItemExtension;

        public override void ApplyLabel()
        {
            var securityDutyexists = Common.CommonUtil.GetMetaModelProviders()
                                        .CurrentMetadataProvider
                                        .SecurityDuties.ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                        .Where(t => t.Equals(this.iSecurityDuty.Name))
                                        .FirstOrDefault();
            if(String.IsNullOrEmpty(securityDutyexists) == false)
            {
                this.iSecurityDuty.Label = this.GetLabel(this.iSecurityDuty.Label);
                this.iSecurityDuty.Description = this.GetLabel(this.iSecurityDuty.Description);
            }
        }

        public override void setElementType(IRootElement selectedElement)
        {
            this.iSecurityDuty = selectedElement as ISecurityDuty;
        }
    }

    public class LabelFactory_ISecurityPrivilege : LabelFactory
    {
        private ISecurityPrivilege iSecurityPrev;

        public override void ApplyLabel()
        {
            var securityDutyexists = Common.CommonUtil.GetMetaModelProviders()
                                        .CurrentMetadataProvider
                                        .SecurityPrivileges.ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                        .Where(t => t.Equals(this.iSecurityPrev.Name))
                                        .FirstOrDefault();
            if (String.IsNullOrEmpty(securityDutyexists) == false)
            {
                this.iSecurityPrev.Label = this.GetLabel(this.iSecurityPrev.Label);
                this.iSecurityPrev.Description = this.GetLabel(this.iSecurityPrev.Description);
            }
        }
        public override void setElementType(IRootElement selectedElement)
        {
            this.iSecurityPrev = selectedElement as ISecurityPrivilege;
        }
    }

    public class LabelFactory_ISecurityRole : LabelFactory
    {
        private ISecurityRole iSecurityRole;

        public override void ApplyLabel()
        {
            var securityDutyexists = Common.CommonUtil.GetMetaModelProviders()
                                        .CurrentMetadataProvider
                                        .SecurityRoles.ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                        .Where(t => t.Equals(this.iSecurityRole.Name))
                                        .FirstOrDefault();
            if (String.IsNullOrEmpty(securityDutyexists) == false)
            {
                this.iSecurityRole.Label = this.GetLabel(this.iSecurityRole.Label);
                this.iSecurityRole.Description = this.GetLabel(this.iSecurityRole.Description);
            }
        }
        public override void setElementType(IRootElement selectedElement)
        {
            this.iSecurityRole = selectedElement as ISecurityRole;
        }
    }

    public class LabelFactory_DataEntity : LabelFactory
    {
        IDataEntity iDateEntity;

        public override void setElementType(IRootElement selectedElement)
        {
            this.iDateEntity = selectedElement as IDataEntity;
        }

        public override void ApplyLabel()
        {
            //check if table is in current model
            var dataEntityExists = Common.CommonUtil.GetMetaModelProviders()
                                .CurrentMetadataProvider
                                .DataEntityViews.ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                .Where(t => t.Equals(this.iDateEntity.Name))
                                .FirstOrDefault();

            if (string.IsNullOrEmpty(dataEntityExists) == false)
            {
                this.iDateEntity.Label = this.GetLabel(this.iDateEntity.Label);
                this.iDateEntity.DeveloperDocumentation = this.GetLabel(this.iDateEntity.DeveloperDocumentation);

                // Apply label on fields
                var dataEntityView = this.iDateEntity as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews.DataEntityView;
                if(dataEntityView != null)
                {
                    var fieldsEnum = dataEntityView.Fields.VisualChildren.GetEnumerator();
                    while (fieldsEnum.MoveNext())
                    {
                        var f = fieldsEnum.Current as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews.DataEntityViewField;
                        if(f != null)
                        {
                            f.Label = this.GetLabel(f.Label);
                            f.HelpText = this.GetLabel(f.HelpText);
                        }
                    }

                    var groupsEnum = dataEntityView.FieldGroups.VisualChildren.GetEnumerator();
                    while (groupsEnum.MoveNext())
                    {
                        var f = groupsEnum.Current as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews.FieldGroup;
                        if(f != null)
                        {
                            f.Label = this.GetLabel(f.Label);
                        }
                    }
                }
            }

        }
    }

    public class LabelFactory_DataEntityExtension : LabelFactory
    {
        Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews.IDataEntityViewExtension iDateEntity;

        public override void setElementType(IRootElement selectedElement)
        {
            this.iDateEntity = selectedElement as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews.IDataEntityViewExtension;
        }

        public override void ApplyLabel()
        {
            //check if table is in current model
            var dataEntityExists = Common.CommonUtil.GetMetaModelProviders()
                                .CurrentMetadataProvider
                                .DataEntityViewExtensions.ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                .Where(t => t.Equals(this.iDateEntity.Name))
                                .FirstOrDefault();

            if (string.IsNullOrEmpty(dataEntityExists) == false)
            {
                //this.iDateEntity.Label = this.GetLabel(this.iDateEntity.Label);
                //this.iDateEntity.DeveloperDocumentation = this.GetLabel(this.iDateEntity.DeveloperDocumentation);

                // Apply label on fields
                var dataEntityView = this.iDateEntity as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews.DataEntityViewExtension;
                if (dataEntityView != null)
                {
                    var fieldsEnum = dataEntityView.Fields.VisualChildren.GetEnumerator();
                    while (fieldsEnum.MoveNext())
                    {
                        var f = fieldsEnum.Current as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews.DataEntityViewField;
                        if (f != null)
                        {
                            f.Label = this.GetLabel(f.Label);
                            f.HelpText = this.GetLabel(f.HelpText);
                            f.GroupPrompt = this.GetLabel(f.GroupPrompt);
                        }
                    }

                    var groupsEnum = dataEntityView.FieldGroups.VisualChildren.GetEnumerator();
                    while (groupsEnum.MoveNext())
                    {
                        var f = groupsEnum.Current as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.DataEntityViews.FieldGroup;
                        if (f != null)
                        {
                            f.Label = this.GetLabel(f.Label);
                        }
                    }
                }
            }

        }
    }
}
