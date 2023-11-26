using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Settings
{
    class ModelSettings
    {
        //public string DefaultLabelFile { get; set; } = String.Empty;

        public string Prefix { get; set; } = String.Empty;

        public string Suffix { get; set; } = String.Empty;
        public string Extension { get; set; } = "Extension";

        public List<String> LabelsToUpdate { get; set; } = new List<string>();
        public bool SecurityLabelAutoCreate { get; set; }
        public bool UseCamelCaseForLabels { get; set; }
        public bool CreateLabelsForEmptyProperties { get; set; }
        public bool RecreateSysLabels { get; set; }

        public AllowedLabels allowedLabels { get; set; } = new AllowedLabels();
    }

    class AllowedLabels 
    {
        public bool EDTLabel { get; set; }
        public bool EDTHelpText { get; set; }
        public bool EDTMaxDateLabel { get; set; }
        public bool BaseEnumLabel { get; set; }
        public bool BaseEnumHelp { get; set; }
        public bool BaseEnumValueLabel { get; set; }
        public bool BaseEnumExtensionValueLabel { get; set; }
        public bool ConfigurationKeyLabel { get; set; }
        public bool ConfigurationKeyDescription { get; set; }
        public bool TableExtensionFieldLabel { get; set; }
        public bool TableExtensionFieldHelpText { get; set; }
        public bool TableExtensionFieldGroupLabel { get; set; }
        public bool MenuItemLabel { get; set; }
        public bool MenuItemHelpText { get; set; }
        public bool FormExtensionDesignCaption { get; set; }
        public bool FormDesignCaption { get; set; }
        public bool FormControlLabel { get; set; }
        public bool FormControlHelpText { get; set; }
        public bool FormControlCaption { get; set; }
        public bool FormControlExportLabel { get; set; }
        public bool FormControlExtensionPlaceholderText { get; set; }
        public bool FormControlExtensionExportLabelt { get; set; }
        public bool FormExtensionControlLabel { get; set; }
        public bool FormExtensionControlHelpText { get; set; }
        public bool FormExtensionControlCaption { get; set; }
        public bool FormExtensionControlExportLabel { get; set; }
        public bool FormExtensionControlExtensionPlaceholderText { get; set; }
        public bool FormExtensionControlExtensionExportLabelt { get; set; }
        public bool SecurityDutyLabel { get; set; }
        public bool SecurityDutyDescription { get; set; }
        public bool SecurityPrivilegeLabel { get; set; }
        public bool SecurityPrivilegeDescription { get; set; }
        public bool SecurityRoleLabel { get; set; }
        public bool SecurityRoleDescription { get; set; }
        public bool DataEntityLabel { get; set; }
        public bool DataEntityDeveloperDocumentation { get; set; }
        public bool DataEntityFieldLabel { get; set; }
        public bool DataEntityFieldHelpText { get; set; }
        public bool DataEntityFieldGroupLabel { get; set; }
    }
}
