# Visual studio add-in for dynamics 365 finance and operations
Extensions to enhance the development experience, mostly with just the mouse.
These can only be done from the designer windows of the source objects

* [How to install the extension](#how-to-install-the-extension)
* [Features](#features)
  * [
  * [Create Code extension](#create-code-extension)
  * [Create Extensions](#create-extensions)
  * [Create Labels](#create-labels)
  * [Create Inquire Security Duty from Privilege](#create-inquire-security-duty-from-privilege)
  * [Create Maintain Security Duty from Privilege](#create-maintain-security-duty-from-privilege)
  * [Create Maintain and Inquire Privilege from Menu item](#create-maintain-and-inquire-privilege-from-menu-item)
  * [Create Display menu item from a Form](#create-display-menu-item-from-a-form)
  * [Create Form from a Table](#create-form-from-a-table)
  * [Add element to the current project](#add-element-to-the-current-project)
  * [Show the Label](#show-the-label)
  * [Convert RunBaseBatch to SysOperation](#convert-runbasebatch-to-sysoperation)
  
# How to install the extension

**Updated installation instructions** works for non admin users
- Download the dll file from the folder
- Create a new folder e.g. c:\D365CustomAddIns
- Copy the dll file into the folder above
- Close Visual studio & edit the file: C:\Users\<currentUser>\Documents\Visual Studio 2015\Settings\DefaultConfig.xml
- Edit the following to the xml file
```xml
<?xml version="1.0" encoding="utf-8"?>
<DynamicsDevConfig xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.microsoft.com/dynamics/2012/03/development/configuration">
	<AddInPaths xmlns:d2p1="http://schemas.microsoft.com/2003/10/Serialization/Arrays">
		<d2p1:string>C:\D365CustomAddins</d2p1:string>
	</AddInPaths>
 
</DynamicsDevConfig>
```
**Note**: remove the i:nil="true" from the AddInPath tag

**For admin users** you can chose to use the above or the one as per below
[![Install VS Add in](http://img.youtube.com/vi/4qPndob4SHk/0.jpg)](http://www.youtube.com/watch?v=4qPndob4SHk)

Powershell & DLL's script located in OutputDlls folder

The powershell script **CopyDLLsToExtensionFolder.ps1** included will copy all the DLL files in the current folder to the Visual studio extension folder. *Requires to be run in Administrator mode*.
  - Run a powershell shell script in admin mode
  - Navigate the folder where this powershell script exists
  - Make sure all the DLL's that need to be copied are in the same folder
  - Close all instances of Visual studio
  - call .\CopyDLLsToExtensionFolder.ps1

# Features

## Settings based object creation
Create settings per Model from the main menu Dynamics D365 > AddIns > Model Settings (SS D365)
The files are stored in the same folder where the DLL's are referenced by visual studio.
The labels to update are in the settings form, so that not all languages are updated. (possible bug when multiple file id are used)

## Create Code extension
Right click a Table, Class, Data entity, Form to create a class extension for it

[Create class extension](https://github.com/shashisadasivan/SSD365VSAddIn/wiki/Create-Class-Extension)

## Create Extensions
Create extensions for tables, Forms, Base Enums and Security Duty

## Create Labels
Right click the element and choose Create label for properties (Fields, FieldGroups)
Currently this will add the label to the first label file (all languages) of the current model 
* Table / Table Extensions

[![Add labels to a Table](http://img.youtube.com/vi/Kv_dlCehPI4/0.jpg)](https://www.youtube.com/watch?v=Kv_dlCehPI4)

Labels updated for Table label, developer documentation, Field Labels, Field Help text, Field group
* Menu item
* Security Duty
* Security Privilege
* Security Role
* Forms - Partially done
* EDT
* Base enums

## Create labels in code
This will create labels in the methods for the following elements
* Class methods
* Data entity methods
* Table methods

## Create Inquire Security Duty from Privilege
Right privilege in designer mode and select **Create Inquire Security duty (SS D365)**

## Create Maintain Security Duty from Privilege
Right privilege in designer mode and select **Create Maintain Security duty (SS D365)**

## Create Maintain and Inquire Privilege from Menu item
Right privilege in designer mode and select **Create Maintain and view Privilege (SS D365)**

## Create Display menu item from a Form
Open the form, right click the Form name (left of designer) and select the AddIn **Create display menu item (SS D365)**

## Create Form from a Table
Open the table, Right click the Table name in the designer and select the AddIn **Create Form (SS D365)**

## Add element to the current project
Open an element in designer mode, Right click and select **Add to Project (SS D365)**
This will add the element to the current project as long as the element belongs to the project

## Show the Label
Show the label of an element regardless if the label is defined on this element or its extended data type
Right click an element and select **Show the Label (SS D365)**
This will show the label and the help text of the element
Currenty works for 
  - Extended data types (shows the label and the help text)
  - Table field

## Convert RunBaseBatch to SysOperation
This is an experimental feature
Converts a existing class which implements a RunBaseBatch to SysOperation.
Currently creates a contract class for the given RunBaseBatch class based on the dialog method

