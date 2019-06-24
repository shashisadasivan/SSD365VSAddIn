# Visual studio add-in for dynamics 365 finance and operations
Extensions to enhance the development experience, mostly with just the mouse.
These can only be done from the designer windows of the source objects

# How to install the extension
Powershell & DLL's script located in OutputDlls folder

The powershell script **CopyDLLsToExtensionFolder.ps1** included will copy all the DLL files in the current folder to the Visual studio extension folder. *Requires to be run in Administrator mode*.
  - Run a powershell shell script in admin mode
  - Navigate the folder where this powershell script exists
  - Make sure all the DLL's that need to be copied are in the same folder
  - call .\CopyDLLsToExtensionFolder.ps1

# Features

## Create Code extension
Right click a Table, Class, Data entity, Form to create a class extension for it
![Create class extension](https://github.com/shashisadasivan/SSD365VSAddIn/wiki/Create-Class-Extension)

## Create Extensions
Create extensions for tables and Security Duty

## Create Labels
Right click the element and choose Create label for properties (Fields, FieldGroups)
Currently this will add the label to the first label file (all languages) of the current model 
* Table / Table Extensions

Labels updated for Table label, developer documentation, Field Labels, Field Help text, Field group
* Menu item

Label updated for Label and help text
* Security Duty - To be implemented
* Security Privilege - To be implemented
* Security Role - To be implemented
* Forms - To be implemented
* EDT - To be implemented
* Base enums - To be implemented

## Create Inquire Security Duty from Privilege
Right privilege in designer mode and select "Create Inquire Security duty (SS D365)"

## Create Maintain Security Duty from Privilege
Right privilege in designer mode and select "Create Maintain Security duty (SS D365)"

## Create Maintain and Inquire Privilege from Menu item
Right privilege in designer mode and select "Create Maintain and view Privilege (SS D365)"

## Create Display menu item from a Form
Open the form, right click the Form name (left of designer) and select the AddIn "Create display menu item (SS D365)"

## Create Form from a Table
open the table, Right click the Table name in the designer and select the AddIn "Create Form (SS D365)"
