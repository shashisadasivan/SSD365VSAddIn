using System;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.BaseTypes;
using System.Linq;

namespace SSD365VSAddIn.Labels
{
    internal class LabelFactory_IEdtBase : LabelFactory
    {
        private IEdtBase iEdtBase;
        public override void ApplyLabel()
        {
            var edtBaseExists = Common.CommonUtil.GetMetaModelProviders()
                                        .CurrentMetadataProvider
                                        .Edts.ListObjectsForModel(Common.CommonUtil.GetCurrentModel().Name)
                                        .Where(t => t.Equals(this.iEdtBase.Name))
                                        .FirstOrDefault();
            if (String.IsNullOrEmpty(edtBaseExists) == false)
            {
                this.iEdtBase.Label = this.GetLabel(this.iEdtBase.Label, "EDT", "Label");
                this.iEdtBase.HelpText = this.GetLabel(this.iEdtBase.HelpText, "EDT", "HelpText");
                this.iEdtBase.CollectionLabel = this.GetLabel(this.iEdtBase.CollectionLabel, "EDT", "CollectionLabel");
                //IEdtString, IEdtGuid, EdtInt, Edtint64, EdtReal, Container, EdtEnum // these dont have any special labels
                if (iEdtBase is IEdtDate)
                {
                    IEdtDate edt = iEdtBase as IEdtDate;
                    edt.MaxDateLabel = this.GetLabel(edt.MaxDateLabel, "EDT", "MaxDateLabel");
                }
                else if (iEdtBase is IEdtUtcDateTime)
                {
                    IEdtUtcDateTime edt = iEdtBase as IEdtUtcDateTime;
                    edt.MaxDateLabel = this.GetLabel(edt.MaxDateLabel, "EDT", "MaxDateLabel");
                }
            }
        }

        public override void setElementType(IRootElement selectedElement)
        {
            this.iEdtBase = selectedElement as IEdtBase;
        }
    }
}