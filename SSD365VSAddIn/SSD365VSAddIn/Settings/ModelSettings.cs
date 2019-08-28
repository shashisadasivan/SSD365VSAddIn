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

        public List<String> LabelsToUpdate { get; set; } = new List<string>();
    }
}
