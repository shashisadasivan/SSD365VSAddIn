using EnvDTE;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.MainMenuAddIns
{
    //TODO: #34 error returns when Breakpoints.Add called "Multiple locations specified for new breakpoint". Might be thefile wasnt found
    //[Export(typeof(IMainMenu))]
    public class AddInfoBreakpointMainMenuAddIn : MainMenuBase
    {
        /// <summary>
        /// Get the DTE object
        /// </summary>
        internal static DTE DTE => CoreUtility.ServiceProvider.GetService(typeof(DTE)) as DTE;

        #region Member variables
        private const string addinName = "SSD365VSAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                //return "Build and add reference";
                return "Add breakpoint to Info class (SSD365)"; //TODO: add label

            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return AddInfoBreakpointMainMenuAddIn.addinName;
            }
        }

        #endregion

        #region Callbacks
        /// <summary>
        /// Called when user clicks on the add-in menu
        /// </summary>
        /// <param name="e">The context of the VS tools and metadata</param>
        public override void OnClick(AddinEventArgs e)
        {
            this.SetBreakpoint();
        }
        #endregion

        public void SetBreakpoint()
        {
            DTE.Debugger.Breakpoints.Add("add", @"K:\AosService\PackagesLocalDirectory\ApplicationPlatform\ApplicationPlatform\AxClass\Info.xml", 1);
        }
    }
}
