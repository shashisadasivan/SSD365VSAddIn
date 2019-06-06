using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
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
            //TODO: check if table is in current model
            var table = Common.CommonUtil.GetModelSaveService().GetTable(this.iTable.Name); 
            if (table != null)
            {
                this.iTable.Label = LabelHelper.FindOrCreateLabel(this.iTable.Label);
                this.iTable.DeveloperDocumentation = LabelHelper.FindOrCreateLabel(this.iTable.DeveloperDocumentation);
            }
            //TODO: apply label for child elements (fields, groups)
            
        }
    }
}
