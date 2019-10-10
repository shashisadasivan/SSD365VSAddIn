using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This class will be resposible for storing settings for this addin
/// </summary>
namespace SSD365VSAddIn.MainMenuAddIns
{
    //[Export(typeof(IMainMenu))]
    //public class SettingsMainMenuAddIn : MainMenuBase
    //{
    //    #region Member variables
    //    private const string addinName = "SSD365VSSettingsMainMenuAddIn";
    //    #endregion

    //    #region Properties
    //    /// <summary>
    //    /// Caption for the menu item. This is what users would see in the menu.
    //    /// </summary>
    //    public override string Caption
    //    {
    //        get
    //        {
    //            return AddinResources.SettingsMainMenuCaption;
    //        }
    //    }

    //    /// <summary>
    //    /// Unique name of the add-in
    //    /// </summary>
    //    public override string Name
    //    {
    //        get
    //        {
    //            return SettingsMainMenuAddIn.addinName;
    //        }
    //    }

    //    #endregion

    //    #region Callbacks
    //    /// <summary>
    //    /// Called when user clicks on the add-in menu
    //    /// </summary>
    //    /// <param name="e">The context of the VS tools and metadata</param>
    //    public override void OnClick(AddinEventArgs e)
    //    {
    //        try
    //        {
    //            // Do your magic for your add-in
                
    //        }
    //        catch (Exception ex)
    //        {
    //            CoreUtility.HandleExceptionWithErrorMessage(ex);
    //        }
    //    }
    //    #endregion
    //}
}
