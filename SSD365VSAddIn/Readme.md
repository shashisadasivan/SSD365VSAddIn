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

## Create labels for Table Element
Right click Table design and choose Create label for properties.
This will be extended for other element types and also fields within the Table.
Currently this will add the label to the first label file (all languages) of the current model 

## Create Inquire Security Duty from Privilege

## Create Maintain Security Duty from Privilege

## Create Maintain and Inquire Privilege from Menu item

