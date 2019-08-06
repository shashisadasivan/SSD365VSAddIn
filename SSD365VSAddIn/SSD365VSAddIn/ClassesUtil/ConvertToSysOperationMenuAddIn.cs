using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Classes;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.ClassesUtil
{
    [Export(typeof(Microsoft.Dynamics.Framework.Tools.Extensibility.IDesignerMenu))] // IDesignerMenu
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IClassItem))]
    class ConvertToSysOperationMenuAddIn : DesignerMenuBase
    {
        #region Member variables
        private const string addinName = "SSD365VSAddIn.ConvertToSysOperationMenuAddIn";
        #endregion

        #region Properties
        /// <summary>
        /// Caption for the menu item. This is what users would see in the menu.
        /// </summary>
        public override string Caption
        {
            get
            {
                return AddinResources.ConvertToSysOperation;
            }
        }

        /// <summary>
        /// Unique name of the add-in
        /// </summary>
        public override string Name
        {
            get
            {
                return ConvertToSysOperationMenuAddIn.addinName;
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
                int displayOrder = 0;
                // we will create Extension class here
                var selectedElement = e.SelectedElement as Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.IRootElement;
                if (selectedElement != null)
                {
                    // Find current model
                    
                    var modelSaveInfo = Common.CommonUtil.GetCurrentModelSaveInfo();
                    var metaModelService = Common.CommonUtil.GetModelSaveService();

                    IClassItem classItem = selectedElement as IClassItem;
                    // Get the selected class AxClass object
                    AxClass selectedClass = Common.CommonUtil.GetModelSaveService().GetClass(classItem.Name);
                    
                    // TODO: check that the class is a RunBaseBatch class. Otherwise send message that this is not required.


                    // Create Contract class
                    Microsoft.Dynamics.AX.Metadata.MetaModel.AxClass contractClass = new AxClass()
                    {
                        Name = classItem.Name + "Contract",
                    };
                    contractClass.Implements.Add("SysOperationValidatable");
                    contractClass.AddAttribute(new AxAttribute() { Name = "DataContract" });
                    contractClass.SourceCode.Declaration = $"[DataContract]\npublic final class {contractClass.Name} implements SysOperationValidatable\n{{\n\n}}"; // this is replaced again later with the variables


                    List<string> contractParms = new List<string>(); // store the variables here
                    List<string> contractParmMethods = new List<string>(); // store the parm methods here
                    List<string> axParmVariables = new List<string>();
                    List<AxMethod> axMethods = new List<AxMethod>();
                    string currentGroup = string.Empty;
                    string headerAttributes = "DataContract";
                    int headerAttributeOrder = 0;

                    var dialogMethod = selectedClass.Methods.Where(m => m.Name.Equals("dialog", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if(dialogMethod != null)
                    {
                        // from the dialog method we would know what the code fields should be
                        var dialogSource = dialogMethod.Source;

                        //
                        var dialogLines = dialogSource.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        foreach (var dialogLine in dialogLines)
                        {
                            if (dialogLine.Trim().StartsWith("public"))
                            {
                                continue;
                            }
                            else if (dialogLine.Contains(".addGroup("))
                            {
                                // set the group name here
                                string dialogSourceLine = dialogLine.Substring(dialogLine.IndexOf("(") + 1);
                                dialogSourceLine = dialogSourceLine.Replace(");", "").Replace("\"", "").Trim();
                                string currentGrouplabel = dialogSourceLine;
                                currentGroup = dialogSourceLine;
                                if (currentGroup.StartsWith("@"))
                                {
                                    // we need to get a name for this group
                                    var label = Labels.LabelHelper.FindLabelGlobally(currentGrouplabel);
                                    if(label != null)
                                    {
                                        currentGroup = label.LabelText;
                                    }
                                }
                                currentGroup = currentGroup.Replace(" ", "").Trim();

                                headerAttributes += $", SysOperationGroupAttribute('{currentGroup}', \"{currentGrouplabel}\", '{headerAttributeOrder}')";

                                headerAttributeOrder++;
                            }
                            else if(dialogLine.Contains(".addField"))
                            {
                                string addFieldLine = dialogLine.Trim();
                                // so over here we will get the field to add as a member & parm method
                                addFieldLine = addFieldLine.Substring(addFieldLine.IndexOf(".addField"));
                                addFieldLine = addFieldLine.Substring(addFieldLine.IndexOf("(") + 1);

                                addFieldLine = addFieldLine.Replace(");", "").Trim();
                                var parameters = addFieldLine.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                                parameters.ForEach(p => p.Trim());
                                string fieldType = String.Empty;
                                string fieldLabel = String.Empty;
                                string fieldHelp = String.Empty;
                                string fieldName = String.Empty;

                                string attributes = String.Empty;
                                attributes = $"[DataMember, SysOperationDisplayOrder('{displayOrder}')";
                                // we will always have a fieldType (or atleast we should)
                                fieldType = parameters[0].Substring(parameters[0].IndexOf("(") + 1)
                                                    .Replace("(", "").Replace(")", "").Trim();
                                if (parameters.Count >= 2)
                                {
                                    fieldName = parameters[1].Replace("\"", "").Trim();
                                    //if (String.IsNullOrEmpty(fieldLabel) == false)
                                    //{
                                    //    attributes += $", SysOperationLabel('{fieldName}')";
                                    //}
                                }

                                if (parameters.Count >= 3)
                                {
                                    fieldLabel = parameters[2].Replace("\"", "").Trim();
                                    if (String.IsNullOrEmpty(fieldLabel) == false)
                                    {
                                        attributes += $", SysOperationLabel(\"{fieldLabel}\")";
                                    }
                                }
                                if(parameters.Count >= 4)
                                {
                                    fieldHelp = parameters[3].Replace("\"", "").Trim();
                                    if (String.IsNullOrEmpty(fieldHelp) == false)
                                    {
                                        attributes += $", SysOperationHelpText(\"{fieldHelp}\")";
                                    }
                                }
                                // add the group here
                                if(!String.IsNullOrEmpty(currentGroup))
                                {
                                    attributes += $", SysOperationGroupMemberAttribute(\"{currentGroup}\")";
                                }
                                attributes += "]";

                                

                                /* Crude method
                                string fieldType = addFieldLine.Substring(0, addFieldLine.IndexOf(","));
                                fieldType = fieldType.Substring(fieldType.IndexOf("(") + 1)
                                                        .Replace("(", "").Replace(")", "").Trim();

                                string fieldLabel = String.Empty;
                                string fieldHelp = String.Empty;
                                string fieldName = addFieldLine.Substring(addFieldLine.IndexOf(",") + 1)
                                                        .Replace(")", "").Replace(";", "").Trim();
                                if (fieldName.Count(c => c == ',') > 0)
                                {
                                    // that means there is a label / help label in here as well
                                    // Break the fieldname further
                                    // find the label here
                                    string fieldNameLabel = fieldName;
                                    fieldName = fieldNameLabel.Substring(0, fieldNameLabel.IndexOf(",")).Trim();

                                    // find the help label here
                                }
                                */

                                AxClassMemberVariable axVar = new AxClassMemberVariable { TypeName = fieldType, Name = fieldName };
                                axVar.Type = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.CompilerBaseType.AnyType;
                                axParmVariables.Add($"\t{fieldType} {fieldName};");
                                contractClass.AddMember(axVar);
                                
                                

                                // need to add Attributes here 
                                // add parameters to the method
                                //fieldName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fieldName);
                                AxMethod method = new AxMethod
                                {
                                    Name = "parm" + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fieldName),
                                    ReturnType = new AxMethodReturnType() { TypeName = fieldType }
                                };
                                //,{SysOperationLabelAttribute(""{}"")} // add the group & label & help attribute 
                                method.Source = 
                                    //$@"    [DataMember,SysOperationDisplayOrder('{displayOrder}')]
                                    $@"    {attributes}    
    public {fieldType} {method.Name} ({fieldType} _{fieldName} = {fieldName})
    {{
        {fieldName} = _{fieldName};
        return {fieldName};
    }}" + Environment.NewLine;
                                //contractClass.AddMethod(method);
                                axMethods.Add(method);

                                displayOrder++;
                            }

                        }
                    }

                    // Add all the axmethods here
                    string parmVarSource = string.Empty;
                    axParmVariables.ForEach(s => parmVarSource = parmVarSource + "\n" + s);
                    //contractClass.SourceCode.Declaration = $"[DataContract]\npublic final class {contractClass.Name}\n{{\n{parmVarSource}\n\n}}";
                    contractClass.SourceCode.Declaration = $"[{headerAttributes}]\npublic final class {contractClass.Name} implements SysOperationValidatable\n{{\n{parmVarSource}\n\n}}";
                    axMethods.ForEach(method => contractClass.AddMethod(method));

                    // Create validate method
                    this.applyValidateMethod(selectedClass, contractClass);


                    metaModelService.CreateClass(contractClass, modelSaveInfo);
                    Common.CommonUtil.AddElementToProject(contractClass);


                    //extensionClass.SourceCode.Declaration = $"[ExtensionOf({intrinsicStr}({selectedElement.Name}))]\npublic final class {className}\n{{\n\n}}";
                    /*

                    metaModelService.CreateClass(extensionClass, modelSaveInfo);
                    Common.CommonUtil.AddElementToProject(extensionClass);
                    */
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }

        public void applyValidateMethod(AxClass selectedClass, AxClass contractClass)
        {
            var validateMethod = selectedClass.Methods.Where(m => m.Name.Equals("Validate", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            AxMethod axValidateMethod = new AxMethod() { Name = "validate", ReturnType = new AxMethodReturnType() { Type = Microsoft.Dynamics.AX.Metadata.Core.MetaModel.CompilerBaseType.AnyType, TypeName = "boolean" } };
            if(validateMethod != null)
            {
                axValidateMethod.Source = validateMethod.Source.Replace("Object calledFrom = null", "");
            }
            else
            {
                // we need to put a dummy method here
                axValidateMethod.Source = @"    public boolean validate(Object calledFrom = null)
    {
        boolean ret;

        ret = super(calledFrom);

        return ret;
    }" + Environment.NewLine;
            }

            contractClass.AddMethod(axValidateMethod);
        }
        #endregion
    }
}
