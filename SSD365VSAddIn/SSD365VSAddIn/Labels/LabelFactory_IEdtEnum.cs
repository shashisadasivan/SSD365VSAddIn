using System;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.BaseTypes;
using System.Linq;

namespace SSD365VSAddIn.Labels
{
    internal class LabelFactory_IBaseEnum : LabelFactory
    {
        private IBaseEnum iBaseEnum;
        public override void ApplyLabel()
        {
            var edtEnumExists = Common.CommonUtil.GetMetaModelProviders()
                                       .CurrentMetadataProvider
                                       .Enums.ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                       .Where(t => t.Equals(this.iBaseEnum.Name))
                                       .FirstOrDefault();
            if (String.IsNullOrEmpty(edtEnumExists) == false)
            {
                this.iBaseEnum.Label = this.GetLabel(this.iBaseEnum.Label);
                this.iBaseEnum.Help = this.GetLabel(this.iBaseEnum.Help);

                // now add the labels for the elements
                foreach (var item in this.iBaseEnum.BaseEnumValues)
                {
                    BaseEnumValue enumValue = item as BaseEnumValue;
                    enumValue.Label = this.GetLabel(enumValue.Label);
                }
            }
        }

        public override void setElementType(IRootElement selectedElement)
        {
            this.iBaseEnum = selectedElement as IBaseEnum;
        }
    }
}