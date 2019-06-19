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
                var label = LabelHelper.FindOrCreateLabel(this.iTable.Label);
                if (label.Equals(this.iTable.Label) == false)
                {
                    this.iTable.Label = label;
                }

                label = LabelHelper.FindOrCreateLabel(this.iTable.DeveloperDocumentation);
                if (label.Equals(this.iTable.Label) == false)
                {
                    this.iTable.DeveloperDocumentation = label;
                }

                // Apply label on fields
                var enumerator = this.iTable.BaseFields.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var baseField = enumerator.Current as IBaseField;
                    label = LabelHelper.FindOrCreateLabel(baseField.Label);
                    if (label.Equals(baseField.Label) == false)
                    {
                        baseField.Label = label;
                    }

                    label = LabelHelper.FindOrCreateLabel(baseField.HelpText);
                    if (label.Equals(baseField.HelpText) == false)
                    {
                        baseField.HelpText = label;
                    }
                }

                // Apply label for groups
                var fieldGrpEnumerator = this.iTable.FieldGroups.GetEnumerator();
                while (fieldGrpEnumerator.MoveNext())
                {
                    var fieldGroup = fieldGrpEnumerator.Current as IFieldGroup;
                    label = LabelHelper.FindOrCreateLabel(fieldGroup.Label);
                    if (label.Equals(fieldGroup.Label) == false)
                    {
                        fieldGroup.Label = label;
                    }
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
                    var label = LabelHelper.FindOrCreateLabel(baseField.Label);
                    if(label.Equals(baseField.Label) == false)
                    {
                        baseField.Label = label;
                    }

                    label = LabelHelper.FindOrCreateLabel(baseField.HelpText);
                    if (label.Equals(baseField.HelpText) == false)
                    {
                        baseField.HelpText = label;
                    }
                }

                //Apply label for fieldGroups
                var fieldGrpEnumerator = this.iTableExtension.FieldGroups.GetEnumerator();
                while (fieldGrpEnumerator.MoveNext())
                {
                    var fieldGroup = fieldGrpEnumerator.Current as IFieldGroup;
                    var label = LabelHelper.FindOrCreateLabel(fieldGroup.Label);
                    if (label.Equals(fieldGroup.Label) == false)
                    {
                        fieldGroup.Label = label;
                    }
                }
            }
            
            //throw new NotImplementedException();
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
                    label = LabelHelper.FindOrCreateLabel(this.iMenuItem.Label);
                    if (label.Equals(this.iMenuItem.Label) == false)
                        this.iMenuItem.Label = label;

                    label = LabelHelper.FindOrCreateLabel(this.iMenuItem.HelpText);
                    if (label.Equals(this.iMenuItem.HelpText) == false)
                        this.iMenuItem.HelpText = label;
                }
            }
        }

        public override void setElementType(IRootElement selectedElement)
        {
            //if (selectedElement is IMenuItem)
                this.iMenuItem = selectedElement as IMenuItem;
            //else if (selectedElement is IMenuItemExtension)
            //    this.iMenuItemExtension = selectedElement as IMenuItemExtension;
        }
    }
}
