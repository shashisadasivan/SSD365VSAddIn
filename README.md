# Visual studio add-in for dynamics 365 finance and operations
Extensions to enhance the development experience, mostly with just the mouse.
These can only be done from the designer windows of the source objects

* [How to install the extension](#how-to-install-the-extension)
* [Features](#features)
  * [Settings based object creation](#settings-based-object-creation)
  * [Create Code extension](#create-code-extension)
  * [Create Extensions](#create-extensions)
  * [Create Labels](#create-labels)
  * [Create Inquire Security Duty from Privilege](#create-inquire-security-duty-from-privilege)
  * [Create Maintain Security Duty from Privilege](#create-maintain-security-duty-from-privilege)
  * [Create Maintain and Inquire Privilege from Menu item](#create-maintain-and-inquire-privilege-from-menu-item)
  * [Create Display menu item from a Form](#create-display-menu-item-from-a-form)
  * [Create Form from a Table](#create-form-from-a-table)
  * [Show the Label](#show-the-label)
  * [Create EDT from table field](#create-edt-from-table-field)
  * [Create from template](#create-from-template)
  
* [Experimental features](#experimental-features)
  * [Add missing references](#add-missing-references)
  * [Convert RunBaseBatch to SysOperation](#convert-runbasebatch-to-sysoperation)
  
# How to install the extension

**Updated installation instructions** works for non admin users
- Download all the dll files from the folder [OutputDlls](OutputDlls)
- Create a new folder e.g. c:\D365CustomAddIns
- Copy the dll files downloaded into the new folder created
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
**Note**: This AddIn works based off the settings of your model. Create / Open a project and select **Dynamics 365 > Addins > Model settings (SS D365)** this will create a settings file for this model required for the operation of most of the features


# Features

## Settings based object creation
Create settings per Model from the main menu Dynamics D365 > AddIns > Model Settings (SS D365)
The files are stored in the same folder where the DLL's are referenced by visual studio.
The labels to update are in the settings form, so that not all languages are updated. (possible bug when multiple file id are used)

## Create Code extension
Right click a Table, Class, Data entity, Form, Form data source to create a class extension for it

[Create class extension](https://github.com/shashisadasivan/SSD365VSAddIn/wiki/Create-Class-Extension)

## Create Extensions
Create extensions for Tables, Data entities, Forms, Base Enums and Security Duty

## Create Labels
Right click the element and choose Create label for properties (Fields, FieldGroups)
Currently this will add the label to the first label file (all languages) of the current model 

You can also create labels directly by opening the "Dynamics D365 > Addins > Create Labels (SS D365)"
This form will create a label and copy the Label Id to the clipboard.

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

## Show the Label
Show the label of an element regardless if the label is defined on this element or its extended data type
Right click an element and select **Show the Label (SS D365)**
This will show the label and the help text of the element
Currenty works for 
  - Extended data types (shows the label and the help text)
  - Table field
  - Data entitiy field

## Create EDT from table field
Right click a field on the table and select **Create EDT (SS D365)**
This will create a EDT of that type, move the label & help label from the field to the EDT, apply the EDT name to the Field Extended data type property.

## Create from template
Tempaltes are a set of objects which are created with this Addin. Templates are defined using the XML files stored in the Metadata folder in the model. This add in utilizes the same XML files that you may want to use as templates allowing you to replace keywords.
The these files are stored in the Folder Templates (under AxClass). A text file dictates what keywords need to be replaced in the file.
This makes it customizable & you can add your own tempaltes over in the folder
The Add in is available under *Dynamics 365 > Add Ins > Create from template*
See [Issue #31](https://github.com/shashisadasivan/SSD365VSAddIn/issues/31) on this progress


# Experimental features

## Add missing references
When you get an error in your project similar to: *The name 'ABCPercentA' does not denote a class, a table, or an extended data type*, then you can add the missing references with this addin.
*Dynamics D365 > Add Ins > Add missing references - experimental (SS D365)*
See [Issue #24](https://github.com/shashisadasivan/SSD365VSAddIn/issues/24) on this progress

## Convert RunBaseBatch to SysOperation
This is an experimental feature
Converts a existing class which implements a RunBaseBatch to SysOperation.
Currently creates a contract class for the given RunBaseBatch class based on the dialog method
