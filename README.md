# Visual studio add for dynamics 365 finance and operations
Extensions to enhance the development experience, mostly keyboardless

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

## Create Extensions
*No implemented yet.* At the moment I havent found any way of creating extension objects

## Create Inquire Security Duty from Privilege

## Create Maintain Security Duty from Privilege

## Create Maintain and Inquire Privilege from Menu item

## Create Display menu item from a Form
Open the form, right click the Form name (left of designer) and select the addin "Create display menu item (SS D365)"
