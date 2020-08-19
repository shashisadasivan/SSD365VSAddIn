using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
//using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Forms
{
    [Export(typeof(IDesignerMenu))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms.IForm))]
    //[DesignerMenuExportMetadata(AutomationNodeType = typeof(Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms.IFormDesign))]
    class FormsFillPatternMenuItemAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.FormsFillPatternMenuItemAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                //return AddinResources.FormsUtilMenuItemAddin;
                //TODO: create label
                return "Fill missing pattern elements (SS D365)";
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return FormsFillPatternMenuItemAddIn.addinName;
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
            try
            {
                var selectedMenuItem = e.SelectedElement as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Forms.IForm;
                if (selectedMenuItem != null)
                {
                    //var metadataType = selectedMenuItem.GetMetadataType();
                    var metaModelService = Common.CommonUtil.GetModelSaveService();
                    AxForm axForm = metaModelService.GetForm(selectedMenuItem.Name);
                    this.CheckPattern(axForm);
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
        #endregion

        protected void CheckPattern(AxForm axForm)
        {
            Microsoft.Dynamics.AX.Metadata.Patterns.PatternFactory pf = new Microsoft.Dynamics.AX.Metadata.Patterns.PatternFactory();
            var formDesignPattern = pf.AllPatterns.Where(p => p.Name.Equals(axForm.Design.Pattern)).FirstOrDefault();
            if (formDesignPattern != null)
            {
                Microsoft.Dynamics.AX.Metadata.Patterns.PatternAnalyzer pa = new Microsoft.Dynamics.AX.Metadata.Patterns.PatternAnalyzer();
                var patternResult = pa.TestPattern(axForm.Design, formDesignPattern);
                if(patternResult.AnyViolations)
                {
                    //Check the sub nodes for the pattern
                    foreach (var subNode in patternResult.Node.SubNodes)
                    {
                        if(subNode.RequireOne)
                        {
                            // check if the design has this node, else add it
                            // Microsoft.Dynamics.AX.Metadata.MetaModel.AxFormActionPaneControl
                            string violationMsg = "1";
                            // Find the type of control to search for
                            Type controlType = null;
                            if(subNode.Type == "ActionPane")
                            {
                                controlType = typeof(AxFormActionPaneControl);
                            }

                            var controlsFound = axForm.Design.Controls.Where(c => c.GetType() == controlType);
                            if(controlsFound.Count() == 0)
                            {
                                // Add the control to the form
                                //Activator.CreateInstance(controlType);
                                if(controlType == typeof(AxFormActionPaneControl))
                                {
                                    axForm.Design.AddControl(new AxFormActionPaneControl() { Name = subNode.FriendlyName });
                                }
                            }
                            //foreach (var control in axForm.Design.Controls)
                            //{
                            //    if (control.GetType() == controlType) // == Microsoft.Dynamics.AX.Metadata.Core.MetaModel.FormControlType.ActionPane)
                            //    {
                            //        if (control is IPatternable)
                            //        {
                            //            // TODO: check further pattern analysis here
                            //        }
                            //        else
                            //        {

                            //        }
                            //        break;
                            //    }
                            //}
                            ////var nodeTypeObj = Object.crea subNode.Type
                        }
                    }
                    foreach (var violation in patternResult.Violations)
                    {
                        string violationMsg = "1";
                    }
                }
            }
        }
    }
}
