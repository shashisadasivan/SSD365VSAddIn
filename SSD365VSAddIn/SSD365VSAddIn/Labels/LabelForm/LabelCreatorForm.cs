using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSD365VSAddIn.Labels.LabelForm
{
    public partial class LabelCreatorForm : Form
    {
        public LabelCreatorForm()
        {
            InitializeComponent();
        }

        private void btnFindLabel_Click(object sender, EventArgs e)
        {
            try
            {
                var label = LabelHelper.FindOrCreateLabel(this.txtLabel.Text);
                if (String.IsNullOrEmpty(label) == false)
                {
                    this.lblResult.Text = $"Label {label} copied to clipboard";
                    Clipboard.SetText(label);
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
    }
}
