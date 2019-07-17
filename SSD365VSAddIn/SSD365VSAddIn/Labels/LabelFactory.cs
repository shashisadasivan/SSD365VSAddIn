using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Security;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Labels
{
    public abstract class LabelFactory
    {
        IRootElement iRootElement;

        public static LabelFactory construct(IRootElement selectedElement)
        {
            LabelFactory labelFactory;

            if (selectedElement is ITable)
            {
                labelFactory = new LabelFactory_Table();
            }
            else if(selectedElement  is ITableExtension)
            {
                labelFactory = new LabelFactory_TableExtension();
            }
            else if(selectedElement is IMenuItem || selectedElement is IMenuItemExtension)
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
                    
                    if (formControl is IFormActionPaneControl)
                    {
                        var formActionPaneControl = formControl as IFormActionPaneControl;
                        formActionPaneControl.Caption = this.GetLabel(formActionPaneControl.Caption);
                    }
                    else if(formControl is IFormGroupControl)
                    {
                        var formGroupControl = formControl as IFormGroupControl;
                        formGroupControl.Caption = this.GetLabel(formGroupControl.Caption);
                    }
                    else if (formControl is IFormGridControl)
                    {
                        var formGridControl = formControl as IFormGridControl;
                        // nothing in the grid to change any text for
                    }
                    else if (formControl is IFormStringControl)
                    {
                        var formStringControl = formControl as IFormStringControl;
                        formStringControl.Text = this.GetLabel(formStringControl.Text);
                        formStringControl.Label = this.GetLabel(formStringControl.Label);
                        // nothing in the grid to change any text for
                    }
                    // quick filter control ? - is only recognized as a FormControl
                    // TODO: #2 other types of controls ?

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

}
