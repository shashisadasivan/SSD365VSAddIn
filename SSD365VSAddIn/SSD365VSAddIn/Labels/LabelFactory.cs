using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Menus;
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
}
